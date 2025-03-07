using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace Ballistics
{
    /// internal handler for updating a batch of bullets
    public class BulletUpdateBatch : IDisposable
    {
        public const int ParallelBatchSize = 32;
        public readonly LinkedNativeData<BulletManaged, BulletNative> BulletData;
        private NativeArray<int> Indices;
        private NativeArray<RaycastCommand> Commands;
        private NativeArray<RaycastHit> Hits;
        private NativeArray<BulletInteraction> Interactions;
        private readonly List<DelayedImpactHandling<SurfaceInteractionInfo>> scheduledSurfaceInteractions;
        private readonly List<DelayedImpactHandling<ImpactInfo>> scheduledImpacts;

        private InitializeUpdateJob initializeJob = new();
        private ProcessInteractionsJob processInteractionsJob = new();

        public BulletUpdateBatch(int capacity)
        {
            // initialize managed data
            scheduledImpacts = new List<DelayedImpactHandling<ImpactInfo>>(capacity);
            scheduledSurfaceInteractions = new List<DelayedImpactHandling<SurfaceInteractionInfo>>(capacity);
            // initialize native arrays
            BulletData = new LinkedNativeData<BulletManaged, BulletNative>(capacity);
            Indices = new NativeArray<int>(capacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            Commands = new NativeArray<RaycastCommand>(capacity, Allocator.Persistent, NativeArrayOptions.ClearMemory); // ClearMemory ensures maxhits is initialized
            Hits = new NativeArray<RaycastHit>(capacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            Interactions = new NativeArray<BulletInteraction>(capacity, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
            // initialize jobs
            initializeJob.Indices = Indices;
            initializeJob.Bullets = BulletData.Native;
            initializeJob.Raycasts = Commands;
            processInteractionsJob.Indices = Indices;
            processInteractionsJob.Bullets = BulletData.Native;
            processInteractionsJob.Interactions = Interactions;
            processInteractionsJob.Raycasts = Commands;
        }

        public void Reset()
        {
            var active = BulletData.CopyUsedIndices(ref Indices);
            for (int i = 0; i < active; i++) {
                var index = Indices[i];
                BulletData.Managed[index].Projectile?.DestroyBullet();
                BulletData.MarkFree(index);
            }
        }

        public int ActiveBullets() => BulletData.ActiveCount;

        public JobHandle InitializeUpdate(float timeStep)
        {
            var activeCount = BulletData.CopyUsedIndices(ref Indices);
            initializeJob.timeStep = timeStep;
            return ScheduleRaycasts(activeCount, initializeJob.Schedule(activeCount, ParallelBatchSize));
        }

        public JobHandle ProcessInteractions(InteractionsResult interactionsResult, Environment environment)
        {
            processInteractionsJob.Environment = environment;
            processInteractionsJob.BaseSeed = (uint)UnityEngine.Random.Range(0, int.MaxValue);
            return ScheduleRaycasts(interactionsResult.ActiveCount, processInteractionsJob.Schedule(interactionsResult.TotalCount, ParallelBatchSize));
        }

        private JobHandle ScheduleRaycasts(int activeCount, JobHandle dependsOn)
        {
            if (activeCount > 0)
                return RaycastCommand.ScheduleBatch(Commands.GetSubArray(0, activeCount), Hits.GetSubArray(0, activeCount), ParallelBatchSize, dependsOn);
            else
                return dependsOn;
        }

        public InteractionsResult GatherManagedInteractions(InteractionsResult lastResult, bool isLastUpdateStepInFrame)
        {
            for (int i = lastResult.ActiveCount; i < lastResult.TotalCount; i++) {
                var index = Indices[i];
                if (index < 0)
                    continue;
                ref var managed = ref BulletData.Managed[index];
                ref var native = ref BulletData.Native.GetRef(index);
                if (isLastUpdateStepInFrame)
                    managed.Projectile.UpdateBullet(new(native.Position, native.Velocity, InteractionFlags.NONE));
                if (native.LifeTime <= 0) {
                    managed.Projectile.DestroyBullet();
                    BulletData.MarkFree(index);
                }
            }

            var nextActiveCount = 0;
            var lastIndex = lastResult.ActiveCount - 1;
            for (int i = 0; i < lastResult.ActiveCount; i++) {
                var index = Indices[i];
                ref var managed = ref BulletData.Managed[index];
                ref var native = ref BulletData.Native.GetRef(index);
                if (HandleManagedInteraction(ref managed, ref native, Hits[i], out var info)) {
                    info.Index = index;
                    Interactions[info.Flags != InteractionFlags.NONE ? nextActiveCount++ : lastIndex--] = info;
                } else { // bullet has been stopped
                    managed.Projectile.DestroyBullet();
                    BulletData.MarkFree(index);
                    info.Index = -1;
                    Indices[i] = -1;
                    Interactions[lastIndex--] = info;
                }
            }
            return new InteractionsResult(nextActiveCount, lastResult.ActiveCount);
        }

        private bool HandleManagedInteraction(ref BulletManaged managed, ref BulletNative native, in RaycastHit hit, out BulletInteraction info)
        {
            info = default;
            if (native.LifeTime <= 0) // died during last ProcessInteractionsJob
                return false;
            if (managed.IsInsideMaterial) {
                if (managed.UpdateImpact(native, hit, out var exitMat, out var exitInfo)) {
                    // Debug.Log("Exit");
                    info.Flags |= InteractionFlags.EXIT; // bullet exits the material, it is currently inside of, in this update step
                    info.ExitNormal = math.half3(exitInfo.normal);
                    info.ExitPoint = exitInfo.point;
                    var exitSpreadAngle = exitMat.GetSpreadAngle();
                    info.ExitSpreadAngle = BulletInteraction.EncodeSpreadAngle(exitSpreadAngle);
                    if (native.Energy > math.distance(native.Position, info.ExitPoint) * exitMat.GetEnergyLossPerUnit()) { // has remaining energy -> schedule exit surface interaction
                        managed.Projectile.UpdateBullet(new(exitInfo.point, native.Velocity, info.Flags));
                        ScheduleSurfaceInteraction(exitMat.GetImpactHandler(), new(SurfaceInteractionInfo.InteractionType.EXIT, exitInfo, exitSpreadAngle, 1, managed, native));
                    } else {
                        // Debug.Log("Stuck inside collider");
                        return false;   // bullet stuck in current collider -> stop
                    }
                    if (exitInfo.colliderInstanceID == hit.colliderInstanceID && math.distancesq(exitInfo.point, hit.point) < BallisticsUtil.Epsilon) {
                        // when queries-hit-backfaces is enabled, mesh colliders will detect their own surface from the inside -> impact info is invalid. Wait for unity 2022.2 to disable this locally https://docs.unity3d.com/2022.2/Documentation/ScriptReference/RaycastCommand-queryParameters.html
                        return true;
                    }
                }
            }

            if (hit.colliderInstanceID != 0) {
                if (!BallisticMaterialCache.TryGet(hit.collider.sharedMaterial, out var hitMaterial))
                    return false; // hit collider without ballistic material, and no global material set
                var impact = hitMaterial.HandleImpact(native, managed.Info, hit);
                if (impact.ImpactResult == MaterialImpact.Result.IGNORE)
                    return true;
                info.Flags |= InteractionFlags.HIT;
                info.EntryPoint = hit.point;
                info.EntryNormal = math.half3(hit.normal);
                info.SpeedFactor = 1;
                managed.Projectile.UpdateBullet(new(hit.point, native.Velocity, info.Flags));
                bool enterMaterial = true;
                if (!managed.IsInsideMaterial) {
                    ScheduleSurfaceInteraction(hitMaterial.GetImpactHandler(), new(SurfaceInteractionInfo.TypeFromImpactResult(impact.ImpactResult), hit, impact.SpreadAngle, impact.SpeedFactor, managed, native));
                    if (impact.ImpactResult == MaterialImpact.Result.STOP)
                        return false;
                    if (impact.ImpactResult == MaterialImpact.Result.RICOCHET) {
                        enterMaterial = false;
                        info.RicochetSpreadAngle = BulletInteraction.EncodeSpreadAngle(impact.SpreadAngle);
                        // Debug.Log("Ricochet");
                    }
                    info.SpeedFactor = impact.SpeedFactor;
                } else {
                    // Debug.Log("Hit " + hit.collider + " and already inside " + managed.ExitInfo.collider);
                }
                if (enterMaterial) {
                    // Debug.Log("Enter");
                    info.Flags |= InteractionFlags.ENTRY;
                    info.EnergyLossPerUnit = managed.EnterMaterial(hit, native, hitMaterial, out var impactDepth);
                    if (info.EnergyLossPerUnit < 0)
                        return false; // negative energyloss -> stop bullet
                    ScheduleImpact(hitMaterial.GetImpactHandler(), new(hit, info.EnergyLossPerUnit, impactDepth, managed, native));
                }
            }
            return true;
        }

        private void ScheduleSurfaceInteraction(IImpactHandler mat, in SurfaceInteractionInfo surfaceImpactInfo) =>
            scheduledSurfaceInteractions.Add(new(mat, surfaceImpactInfo));

        private void ScheduleImpact(IImpactHandler mat, in ImpactInfo impactInfo) =>
            scheduledImpacts.Add(new(mat, impactInfo));

        public void HandleScheduledImpacts(IImpactHandler globalImpactHandler)
        {
            for (var i = scheduledImpacts.Count - 1; i >= 0; i--) {
                var impact = scheduledImpacts[i];
                HandledFlags flags = 0;
                flags |= impact.Info.BulletInfo.ImpactHandler.HandleImpact(impact.Info, flags);
                flags |= impact.MaterialImpactHandler.HandleImpact(impact.Info, flags);
                globalImpactHandler.HandleImpact(impact.Info, flags);
            }
            scheduledImpacts.Clear();
            for (var i = scheduledSurfaceInteractions.Count - 1; i >= 0; i--) {
                var impact = scheduledSurfaceInteractions[i];
                HandledFlags flags = 0;
                flags |= impact.Info.BulletInfo.ImpactHandler.HandleSurfaceInteraction(impact.Info, flags);
                flags |= impact.MaterialImpactHandler.HandleSurfaceInteraction(impact.Info, flags);
                globalImpactHandler.HandleSurfaceInteraction(impact.Info, flags);
            }
            scheduledSurfaceInteractions.Clear();
        }

        private void ReleaseBuffers()
        {
            Indices.Dispose();
            Commands.Dispose();
            Hits.Dispose();
            Interactions.Dispose();
        }

        public void Dispose()
        {
            Reset();
            BulletData.Dispose();
            ReleaseBuffers();
        }
    }

    [Flags]
    public enum InteractionFlags : uint
    {
        NONE = 0,
        HIT = 1 << 0, // a hit without entry is a ricochet
        ENTRY = 1 << 1 | HIT,
        EXIT = 1 << 2
    }

    public struct BulletInteraction
    {
        public InteractionFlags Flags;
        public int Index;
        public float SpeedFactor;

        // exit settings
        public half3 ExitNormal;
        public float3 ExitPoint;
        public ushort ExitSpreadAngle; // [0-90] deg

        // entry settings
        public half3 EntryNormal;
        public float3 EntryPoint;
        public float EnergyLossPerUnit;

        // ricochet spread angle
        public ushort RicochetSpreadAngle; // [0-90] deg

        public const float DecodeSpreadFactor = .5f * math.PI / ushort.MaxValue;
        public static ushort EncodeSpreadAngle(float angle)
            => (ushort)(math.clamp(angle / (.5f * math.PI), 0f, 1f) * ushort.MaxValue);
    }

    public struct InteractionsResult
    {
        public int ActiveCount;
        public int TotalCount;

        public InteractionsResult(int active, int total)
        {
            ActiveCount = active;
            TotalCount = total;
        }

        public void Initialize(int total)
        {
            ActiveCount = total;
            TotalCount = total;
        }
    }

    public readonly struct DelayedImpactHandling<T> where T : struct
    {
        public readonly IImpactHandler MaterialImpactHandler;
        public readonly T Info;

        public DelayedImpactHandling(IImpactHandler matHandler, T info)
        {
            MaterialImpactHandler = matHandler;
            Info = info;
        }
    }
}
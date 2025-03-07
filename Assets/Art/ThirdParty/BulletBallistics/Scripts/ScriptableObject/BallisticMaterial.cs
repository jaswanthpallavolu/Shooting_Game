using Unity.Mathematics;
using UnityEngine;

namespace Ballistics
{
    /// Defines behavior of bullets impacting a collider 
    public interface IBallisticMaterial
    {
        MaterialImpact HandleImpact(in BulletNative bullet, BulletInfo info, in RaycastHit rayHit);
        float GetEnergyLossPerUnit();
        float GetSpreadAngle();
        IImpactHandler GetImpactHandler();
    }

    public class BallisticMaterial : InitializableScriptableObject, IBallisticMaterial
    {
        [SerializeField, HideInInspector]
        private PhysicMaterial Target;
        public PhysicMaterial PhysicMaterial { get => Target; }
#if UNITY_EDITOR
        public void SetPhysicMaterial(PhysicMaterial target) => Target = target;
#endif

        public override void Initialize()
        {
            BallisticMaterialCache.Add(this);
        }

        [Header("Impact")]
        [Tooltip("Energyloss of a bullet penetrating through 1 unit of this material")]
        public float EnergyLossPerUnit = 1000;

        [Tooltip("Spread angle when the bullet exits this material")]
        [PhysicalUnit(PhysicalType.ANGLE)] public float Spread = .01f;

        [Tooltip("Handler for bullet interactions with this material")]
        [SerializeField, InlineInspector] private ImpactHandlerObject impactHandler;

        [Header("Ricochet")]
        [Tooltip("Spread angle when the bullet ricochets off this material")]
        [PhysicalUnit(PhysicalType.ANGLE)] public float RicochetSpread = .02f;

        [Tooltip("Ricochet probability on a heads on impact (0° impact angle to surface normal)")]
        [PhysicalUnit(PhysicalType.PERCENTAGE)] public Vector2 RicochetProbability = new(0f, .1f);

        [Tooltip("Proportion of velocity after ricochet")]
        [PhysicalUnit(PhysicalType.PERCENTAGE)] public Vector2 RicochetBounciness = new(.7f, .8f);

        private float EvaluateRicochetProbability(float impactAngle)
        {
            impactAngle = Mathf.Clamp01(impactAngle * (1.0f / 90.0f));
            return Mathf.Lerp(RicochetProbability.x, RicochetProbability.y, impactAngle * impactAngle);
        }

        // if impactHandler is null, we return a DummyImpactHandler here, so we do not have to null check
        [SerializeField, HideInInspector] private bool hasImpactHandler;
        public IImpactHandler ImpactHandler {
            get => hasImpactHandler ? impactHandler : DummyImpactHandler.Instance;
            set {
                impactHandler = value as ImpactHandlerObject;
                hasImpactHandler = impactHandler != null;
            }
        }

        public MaterialImpact HandleImpact(in BulletNative bullet, BulletInfo _, in RaycastHit rayHit)
        {
            var impactAngle = Vector3.Angle(-rayHit.normal, bullet.Velocity);
            if (EvaluateRicochetProbability(impactAngle) > UnityEngine.Random.value)
                return MaterialImpact.Ricochet(RicochetSpread, UnityEngine.Random.Range(RicochetBounciness.x, RicochetBounciness.y));
            return MaterialImpact.Penetrate();
        }

        public float GetEnergyLossPerUnit() => EnergyLossPerUnit;
        public float GetSpreadAngle() => Spread;
        public IImpactHandler GetImpactHandler() => ImpactHandler;

        private void OnValidate()
        {
            if (Target != null && !name.Equals(Target.name))
                name = Target.name;
            ImpactHandler = impactHandler;
        }
    }

    public readonly struct MaterialImpact
    {
        public enum Result
        {
            STOP,
            ENTER,
            RICOCHET,
            IGNORE
        }
        public readonly Result ImpactResult;
        public readonly float SpreadAngle;
        public readonly float SpeedFactor;

        public static MaterialImpact Stop() => new(Result.STOP, 0, 0);
        public static MaterialImpact Ignore() => new(Result.IGNORE, 0, 0);
        public static MaterialImpact Penetrate(float speedFactor = 1) => new(Result.ENTER, 0, speedFactor);
        public static MaterialImpact Ricochet(float spreadAngle, float speedFactor = 1) => new(Result.RICOCHET, spreadAngle, speedFactor);

        private MaterialImpact(Result impactResult, float spreadAngle, float speedFactor)
        {
            ImpactResult = impactResult;
            SpreadAngle = spreadAngle;
            SpeedFactor = speedFactor;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using Ballistics;
using UnityEngine;

namespace SG_Project
{
    public class FirearmWeaponController : MonoBehaviour
    {
        public CharacterManager Character { get; set; }
        private Weapon Weapon;
        private WeaponRecoil weaponRecoil;

        [Tooltip("Time in seconds between shots")]
        public float ShootDelay = 0.25f;

        public WeaponMode Mode = WeaponMode.SingleShot;

        [Header("Burst- / Shotgun- Mode")]
        [Tooltip("Number of bullet framents in one shotgun shell.")]
        public int BulletsPerShell = 8;

        [Tooltip("Number of bullets fired in one burst.")]
        public int BulletsPerBurst = 3;

        [Header("Controllers")]
        // public SpreadController SpreadController;
        // public MagazineController MagazineController;
        public WeaponSpread weaponSpread;

        [Header("UI")]
        public CrosshairController crosshairController;
        bool aiming = false;

        [Header("Zeroing")]
        public List<float> Distances;
        public int CurrentZeroingIndex
        {
            get
            {
                if (zeroingResults == null)
                    return -1;
                return Mathf.Clamp(currentZeroingIndex, -1, zeroingResults.Length - 1);
            }
            set
            {
                currentZeroingIndex = value;
            }
        }
        private int currentZeroingIndex = -1;
        private Zeroing.Result[] zeroingResults;

        // internal state
        public bool triggerHeld = false;
        public bool triggerReleaseRequired = false;
        public float cooldownTimer = 0;

        // public void SetTrigger(bool held)
        // {
        //     triggerHeld = held;
        //     Debug.Log("CheckShoot " + held);
        //     // CheckShoot(triggerHeld);
        // }

        void Awake()
        {
            crosshairController = UIManager.instance.crosshairController;
            Weapon = GetComponent<Weapon>();
            weaponRecoil = GetComponent<WeaponRecoil>();
            weaponSpread = GetComponent<WeaponSpread>();
        }

        private void Start()
        {
            // UpdateZeroing();
        }

        private void Update()
        {
            OnAiming();
            HandleAimOffset();
            cooldownTimer -= Time.deltaTime;
            crosshairController.SetMultiplier(weaponSpread.spread);
        }

        public void OnAiming()
        {
            bool aim = PlayerInputManager.instance.aimInput;
            if (aim)
            {
                if (!aiming)
                {
                    aiming = true;
                    weaponSpread.ResetSpread();
                    crosshairController.ToogleCrossHair(true);
                }
            }
            else
            {
                aiming = false;
                crosshairController.ToogleCrossHair(false);
            }
        }

        public void HandleAimOffset()
        {
            Transform rigAimTarget = Character.characterAnimatorManager.currentRigAimTarget;
            Vector3 aimPosition = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Character.aimOffsetRange));
            rigAimTarget.position = aimPosition;
        }

        public void WeaponShoot()
        {
            if (cooldownTimer <= 0)
            {
                Shoot(1, ShootDelay);
            }
        }

        private void CheckShoot(bool th)
        {
            if (!th && triggerReleaseRequired)
            {
                triggerReleaseRequired = false;
            }

            // && MagazineController.IsBulletAvailable()
            if (th && cooldownTimer <= 0)
            {
                // Debug.Log("CheckShoot");
                switch (Mode)
                {
                    case WeaponMode.FullAuto:
                        Shoot(1, ShootDelay);
                        break;
                    case WeaponMode.Shotgun:
                        if (!triggerReleaseRequired)
                        {
                            triggerReleaseRequired = true;
                            Shoot(BulletsPerShell, ShootDelay);
                        }
                        break;
                    case WeaponMode.Burst:
                        if (!triggerReleaseRequired)
                        {
                            triggerReleaseRequired = true;
                            StartCoroutine(ShootBurst());
                        }
                        break;
                    case WeaponMode.SingleShot:
                        if (!triggerReleaseRequired)
                        {
                            triggerReleaseRequired = true;
                            Shoot(1, ShootDelay);
                        }
                        break;
                }
            }
        }

        private IEnumerator ShootBurst()
        {
            var wait = new WaitForSeconds(ShootDelay);
            for (var i = BulletsPerBurst; i > 0; i--)
            {
                Shoot(1, i * ShootDelay);
                yield return wait;
            }
        }

        private void Shoot(int bullets, float cooldown)
        {
            // Debug.Log("Shoot " + bullets);
            for (var i = bullets; i > 0; i--)
                Weapon.Shoot(weaponSpread.CalculateShootDirection(Weapon), CurrentZeroing().Angle);
            cooldownTimer = cooldown;
            weaponRecoil.FireRecoil();
            weaponSpread.BulletFired();
            // MagazineController.BulletFired();
        }

        public void UpdateZeroing()
        {
            if (Distances.Count == 0 || !Core.Environment.EnableGravity)
            {
                zeroingResults = null;
                return;
            }
            var gravity = -Unity.Mathematics.math.length(Core.Environment.Gravity);
#if !BB_NO_AIR_RESISTANCE
            if (Core.Environment.EnableAirResistance)
            {
                using (var handle = Zeroing.ApproximateZeroingAnglesWithDrag(Distances, Weapon.BulletInfo, gravity, Core.Environment.AirDensity, Core.Environment.MaximumDeltaTime))
                    zeroingResults = handle.Get();
            }
            else
#endif
                zeroingResults = Zeroing.ZeroingAnglesNoDrag(Distances, Weapon.BulletInfo.Speed, gravity);
        }


        public Zeroing.Result CurrentZeroing()
        {
            if (CurrentZeroingIndex != -1)
                return zeroingResults[CurrentZeroingIndex];
            return new Zeroing.Result(0, 0);
        }
    }

}

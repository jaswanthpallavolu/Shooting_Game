using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SG_Project
{
    public class WeaponRecoil : MonoBehaviour
    {
        [Header("Player components")]
        PlayerManager playerManager;
        CinemachineImpulseSource impulseSource;

        [Header("Recoil")]
        public float recoilAmount = 0f;
        [Min(0.1f)] public float recoilDuration = 0.15f;
        [Min(0.25f)] public float recenterDuration = 0.25f;
        [SerializeField] float relativeY = 0.5f;
        [SerializeField] float relativeX = 0.25f;

        [Header("Weapon Model Effect")]
        Vector3 wtarget;
        Vector3 woriginal;
        GameObject weaponModel;
        [SerializeField] Vector3 weaponRotation = new Vector3(-2f, 0, 0);
        [SerializeField] float multiplier = 1f;

        float bulletCount;
        [SerializeField] float recoilVertical = 0.7f;
        [SerializeField] float recoilHorizontal = 0.45f;
        [SerializeField] float sumX, sumY;
        [SerializeField] float sumX1, sumY1;

        void Awake()
        {
            playerManager = PlayerInputManager.instance.player;
            impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        void Start()
        {

        }

        void Update()
        {
            WeaponRecoilEffect();
            if (playerManager.isRecoiling)
            {
                recoilAmount = Mathf.Clamp01(recoilAmount + Time.deltaTime / recoilDuration);
                // Recoiling();
                if (recoilAmount == 1)
                {
                    playerManager.isRecoiling = false;
                    playerManager.isReturning = true;
                    // recoilAmount = 0;



                }
            }
            else if (playerManager.isReturning)
            {

                recoilAmount = Mathf.Clamp01(recoilAmount - Time.deltaTime / recenterDuration);
                // Recentering();
                if (recoilAmount == 0)
                {
                    playerManager.isRecoiling = false;
                    playerManager.isReturning = false;
                    bulletCount = 0;

                }
            }
        }

        void Recoiling()
        {
            // float x = Mathf.Lerp(0, recoilHorizontal * relativeX, recoilAmount);
            // float y = Mathf.Lerp(0, recoilVertical * relativeY, recoilAmount);
            float x = recoilHorizontal * relativeX * recoilAmount;
            float y = recoilVertical * relativeY * recoilAmount;
            sumX += x;
            sumY += y;
            Debug.Log(x + ", " + y);
            playerManager.playerCameraController.onAimRotate1(x, y);
        }

        void Recentering()
        {
            // float x = Mathf.Lerp(0, -recoilHorizontal * relativeX, recoilAmount);
            // float y = Mathf.Lerp(0, -recoilVertical * relativeY, recoilAmount);
            float x = -recoilHorizontal * relativeX * recoilAmount;
            float y = -recoilVertical * relativeY * recoilAmount;
            sumX1 += x;
            sumY1 += y;
            if (Mathf.Abs(sumX1) <= sumX && Mathf.Abs(sumY1) <= sumY)
            {
                playerManager.playerCameraController.onAimRotate(x, y);
            }

        }

        void WeaponRecoilEffect()
        {
            weaponModel = playerManager.playerEquipmentManager.rightHandWeaponModel;
            weaponModel.transform.localEulerAngles = Vector3.Lerp(woriginal, wtarget, recoilAmount);
        }

        public void FireRecoil()
        {
            // impulseSource.GenerateImpulse();
            if (!playerManager.isRecoiling)
            {
                impulseSource.GenerateImpulse(Camera.main.transform.forward);

                if (bulletCount == 0)
                {
                    woriginal = weaponModel.transform.localEulerAngles;
                    wtarget = weaponModel.transform.localEulerAngles + weaponRotation * multiplier;
                }
                bulletCount++;
                recoilAmount = 0;
                playerManager.isRecoiling = true;
                playerManager.isReturning = false;
                sumX = 0;
                sumY = 0;
                sumX1 = 0;
                sumY1 = 0;
            }
        }
    }
}
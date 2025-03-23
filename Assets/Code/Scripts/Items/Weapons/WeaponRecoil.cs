using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace SG_Project
{
    public class WeaponRecoil : MonoBehaviour
    {
        // Kick, Vertical recoil
        [Header("Player components")]
        PlayerManager playerManager;
        Transform followTarget;
        CinemachineVirtualCamera aimModeCamera;
        [SerializeField] float targetFOV = 31;
        [SerializeField] float multiplier = 5f;

        [Header("Recoil")]
        public float recoilAmount = 0f;
        // public float recoilSpeed = 0.1f;
        public float recoilCorrectionTime = 0.1f;
        public Vector3 target;
        public Vector3 original;
        public Vector3 wtarget;
        public Vector3 woriginal;
        public Vector3 targetRotation = new Vector3(-2f, 0, 0);
        public GameObject weaponModel;

        bool upward = false;
        bool downward = false;
        int count = 0;

        void Awake()
        {
            playerManager = PlayerInputManager.instance.player;
            followTarget = playerManager.playerCameraController.followTarget;
            aimModeCamera = playerManager.playerCameraController.aimModeCamera;

            // originalRotation = followTarget.localEulerAngles;
        }

        void Start()
        {

        }

        void Update()
        {
            weaponModel = playerManager.playerEquipmentManager.rightHandWeaponModel;
            if (playerManager.isRecoiling)
            {
                followTarget.localEulerAngles = Vector3.Lerp(original, target, recoilAmount);
            }
            weaponModel.transform.localEulerAngles = Vector3.Lerp(woriginal, wtarget, recoilAmount);

            if (upward)
            {

                recoilAmount = Mathf.Clamp01(recoilAmount + (Time.deltaTime / recoilCorrectionTime));
                if (recoilAmount == 1)
                {
                    upward = false;
                    downward = true;
                }
            }
            else if (downward)
            {
                // followTarget.localEulerAngles = Vector3.Lerp(followTarget.localEulerAngles, original, recoilAmount);
                recoilAmount = Mathf.Clamp01(recoilAmount - (Time.deltaTime / recoilCorrectionTime));
                if (recoilAmount == 0)
                {
                    upward = false;
                    downward = false;
                    playerManager.isRecoiling = false;
                    count = 0;
                }
            }

            // aimModeCamera.m_Lens.FieldOfView = (int)Mathf.Lerp(aimModeCamera.m_Lens.FieldOfView, 30, recoilAmount * recoilSpeed);
        }

        public void FireRecoil()
        {

            if (count == 0)
            {
                // original = followTarget.localEulerAngles;
                woriginal = weaponModel.transform.localEulerAngles;
                wtarget = weaponModel.transform.localEulerAngles + targetRotation * multiplier;
            }
            count++;
            original = followTarget.localEulerAngles;
            target = followTarget.localEulerAngles + targetRotation;

            // woriginal = weaponModel.transform.localEulerAngles;


            playerManager.isRecoiling = true;
            upward = true;
            downward = false;
            recoilAmount = 0;
        }
    }
}
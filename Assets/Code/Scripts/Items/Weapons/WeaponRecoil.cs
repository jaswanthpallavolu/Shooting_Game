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


        [Header("Recoil")]
        public float recoilAmount = 0f;
        public float recoilSpeed = 1f;
        public float returnSpeed = 2f;
        public float recoilCorrectionTime = 0.25f;
        public Vector3 target;
        public Vector3 original;
        public Vector3 wtarget;
        public Vector3 woriginal;
        public GameObject weaponModel;
        public float degree = 5f;
        float count;
        [SerializeField] float multiplier = .1f;

        [Header("Method 3")]
        [SerializeField] float recoilVertical = 0.7f;
        [SerializeField] float recoilHorizontal = 0.45f;
        [SerializeField] float recoilDuration = 0.25f;

        void Awake()
        {
            playerManager = PlayerInputManager.instance.player;
            followTarget = playerManager.playerCameraController.followTarget;
            aimModeCamera = playerManager.playerCameraController.aimModeCamera;
        }

        void Start()
        {

        }

        void Update()
        {
            weaponModel = playerManager.playerEquipmentManager.rightHandWeaponModel;
            if (playerManager.isRecoiling || playerManager.isReturning)
            {
                followTarget.localEulerAngles = Vector3.Slerp(original, target, recoilAmount);
                // wierd behaviour at (0,0,0);
            }
            weaponModel.transform.localEulerAngles = Vector3.Lerp(woriginal, wtarget, recoilAmount);

            if (playerManager.isRecoiling)
            {
                if (recoilAmount == 1)
                {
                    playerManager.isRecoiling = false;
                    playerManager.isReturning = true;
                }
                recoilAmount = Mathf.Clamp01(recoilAmount + (Time.deltaTime / recoilCorrectionTime * recoilSpeed));
            }
            else if (playerManager.isReturning)
            {
                if (recoilAmount == 0)
                {
                    playerManager.isReturning = false;
                    count = 0;
                }
                recoilAmount = Mathf.Clamp01(recoilAmount - (Time.deltaTime / recoilCorrectionTime * returnSpeed));
            }
        }

        public void FireRecoil()
        {
            if (!playerManager.isRecoiling)
            {
                float amountVertical = recoilVertical * degree;
                float amountHorizontal = recoilHorizontal * degree / 2;
                Vector3 rotation = new Vector3(-amountVertical, amountHorizontal, 0);
                if (count == 0)
                {
                    woriginal = weaponModel.transform.localEulerAngles;
                    wtarget = weaponModel.transform.localEulerAngles + new Vector3(-2f, 0, 0) * multiplier;
                }
                count++;

                original = followTarget.localEulerAngles;
                target = followTarget.localEulerAngles + rotation;
                recoilAmount = 0;
                playerManager.isRecoiling = true;
                playerManager.isReturning = false;
            }
        }
    }
}
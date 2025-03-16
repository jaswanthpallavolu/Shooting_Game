using System.Collections;
using System.Collections.Generic;
using Ballistics;
using UnityEngine;

namespace SG_Project
{
    public class WeaponRecoil : MonoBehaviour
    {
        // Kick, Vertical recoil
        public Transform followTarget;

        [Header("Recoil")]
        public float recoilAmount = 0f;
        public float recoilSpeed = 0.1f;
        public float recoilCorrectionTime = 0.25f;
        public Vector3 currentRotation;
        public Vector3 originalRotation;
        public Vector3 targetRotation = new Vector3(-15f, 0, 0);

        void Awake()
        {
            followTarget = PlayerInputManager.instance.player.playerCameraController.followTarget;
            originalRotation = followTarget.localEulerAngles;
        }

        void Start()
        {

        }

        void Update()
        {
            // followTarget.localRotation = Quaternion.Lerp(followTarget.localRotation, originalRotation, Time.deltaTime * returnSpeed);
            // targetRotation = Vector3.Lerp(targetRotation, originalRotation, returnSpeed * Time.deltaTime);
            if (recoilAmount > 0)
            {
                Vector3 defaultRotation = originalRotation;
                currentRotation = Vector3.Lerp(defaultRotation, defaultRotation + targetRotation, recoilAmount * recoilSpeed);
                followTarget.localRotation = Quaternion.Euler(currentRotation);
                recoilAmount = Mathf.Clamp01(recoilAmount - (Time.deltaTime / recoilCorrectionTime));
            }
            // Invoke("StopRecoil", 1f);


        }

        public void FireRecoil()
        {
            originalRotation = followTarget.localEulerAngles;
            recoilAmount = Mathf.Clamp01(recoilAmount + 1f);
        }
    }
}
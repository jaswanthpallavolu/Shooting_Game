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
        public Vector3 originalRotation;
        public Vector3 recoilRotation;
        public bool isRecoiling = false;
        public float xRecoil = 0.25f;
        public float yRecoil = 0.25f;
        public float zRecoil = 0.25f;
        public float recoilSpeed = .5f;
        public float recoilResetSpeed = .5f;

        void Awake()
        {
        }

        void Start()
        {
        }

        void Update()
        {
        }

        public void FireRecoil()
        {
            Debug.Log(followTarget.localPosition);
            // originalRotation = followTarget.localPosition;
            // recoilPosition = new Vector3(
            // originalPosition.x + Random.Range(-xRecoil, xRecoil),
            // originalPosition.y + Random.Range(-yRecoil, yRecoil),
            // originalPosition.z
            // );
            // targetRotation += new Vector3(xRecoil, Random.Range(-yRecoil, yRecoil), 0);

        }

        private void StopRecoil()
        {
            isRecoiling = false;
        }
    }
}
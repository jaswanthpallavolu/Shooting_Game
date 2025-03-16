using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SG_Project
{
    public class CrosshairController : MonoBehaviour
    {
        [SerializeField] RectTransform crosshair;
        public float multiplier = 1f;
        public Vector2 size = new Vector2(30, 30);

        // Start is called before the first frame update
        void Awake()
        {
            ToogleCrossHair(false);
        }

        // Update is called once per frame
        void Update()
        {
            crosshair.sizeDelta = Vector2.Lerp(size + (size * multiplier), size, Time.deltaTime);
        }

        public void SetMultiplier(float m)
        {
            multiplier = m;
        }

        public void ToogleCrossHair(bool value)
        {
            crosshair.GetComponent<Image>().enabled = value;
        }
    }
}

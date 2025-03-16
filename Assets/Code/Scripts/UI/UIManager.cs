using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SG_Project
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public CrosshairController crosshairController;

        void Awake()
        {
            if (instance)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            crosshairController = GetComponentInChildren<CrosshairController>();
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Resolvers;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] CinemachineVirtualCamera mainCamera;
    [SerializeField] CinemachineVirtualCamera aimModeCamera;
    [SerializeField] Transform followTarget;
    [SerializeField] float xLimitMin = -20;
    [SerializeField] float xLimitMax = 50;
    Vector3 lookInput;
    float xRotation;
    bool aimMode;

    void Start()
    {
        player = GetComponent<PlayerManager>();
    }
    void Update()
    {
        lookInput = PlayerInputManager.instance.lookInput;
    }

    public void HandleAimMode(bool aimInput)
    {
        if (aimInput)
        {
            aimModeCamera.enabled = true;


            if (!aimMode)
            {
                Quaternion look = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
                float xLook = look.eulerAngles.x;
                if (xLook > 180 && xLook <= 360)
                {
                    xLook -= 360;
                }
                float xAngle = Mathf.Clamp(xLook, xLimitMin, xLimitMax);
                xRotation = xAngle;
                followTarget.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                player.transform.rotation = Quaternion.Euler(0, look.eulerAngles.y, 0);

            }

            if (aimMode)
            {
                OnAimView();
            }
            aimMode = true;
        }
        else
        {
            aimModeCamera.enabled = false;
            aimMode = false;
        }
    }

    private void OnAimView()
    {
        xRotation -= lookInput.y * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, xLimitMin, xLimitMax);
        followTarget.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        float moveX = lookInput.x * Time.deltaTime;
        player.transform.Rotate(Vector3.up * moveX);
    }
}

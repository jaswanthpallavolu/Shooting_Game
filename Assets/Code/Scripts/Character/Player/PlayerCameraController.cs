using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    PlayerManager player;
    [SerializeField] CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera aimModeCamera;
    [SerializeField] public Transform followTarget;
    [SerializeField] float xLimitMin = -20;
    [SerializeField] float xLimitMax = 50;
    [SerializeField] Vector2 defaultSpeed = new Vector2(200, 300);
    [SerializeField] Vector2 cameraSensitivity = new Vector2(50, 50);
    [SerializeField] Vector2 aimSensitivity = new Vector2(50, 50);
    [SerializeField] float lerpSpeed = 2f;
    public Vector3 lookInput;
    float xRotation;
    bool aimMode;
    float turnSmoothVelocity;
    Vector3 angles;

    void Start()
    {
        player = GetComponent<PlayerManager>();
    }
    void Update()
    {
        lookInput = PlayerInputManager.instance.lookInput;
        // mainCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis.m_MaxSpeed = defaultSpeed.x * cameraSensitivity.x / 100;
        // mainCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis.m_MaxSpeed = defaultSpeed.y * cameraSensitivity.y / 100;

        // New Logic
        followTarget.rotation *= Quaternion.AngleAxis(lookInput.x * cameraSensitivity.x / 100, Vector3.up);
        followTarget.rotation *= Quaternion.AngleAxis(-lookInput.y * cameraSensitivity.y / 100, Vector3.right);

        angles = followTarget.localEulerAngles;
        angles.z = 0;
        float angle = angles.x;

        if (angle > 180 && angle < 330)
        {
            angles.x = 330;
        }
        else if (angle < 180 && angle > 60)
        {
            angles.x = 60;
        }
        followTarget.localEulerAngles = angles;
        // Debug.Log("angle " + angle + " angles.x " + angles.x);
    }

    public void HandlePlayerRotation()
    {
        player.transform.rotation = Quaternion.Euler(0, followTarget.eulerAngles.y, 0);
        followTarget.localEulerAngles = new Vector3(followTarget.eulerAngles.x, 0, 0);
    }

    public void HandleAimMode(bool aimInput)
    {
        aimModeCamera.enabled = aimInput;
        if (aimInput)
        {
            HandlePlayerRotation();
        }
        player.animator.SetBool("aimMode", aimInput);
        return;

        if (aimInput)
        {
            aimModeCamera.enabled = true;

            if (!aimMode)
            {
                // Quaternion look = Quaternion.LookRotation(Camera.main.transform.forward, Vector3.up);
                // float xLook = look.eulerAngles.x;
                // if (xLook > 180 && xLook <= 360)
                // {
                //     xLook -= 360;
                // }
                // float xAngle = Mathf.Clamp(xLook, xLimitMin, xLimitMax);
                // xRotation = xAngle;
                // followTarget.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
                // player.transform.rotation = Quaternion.Euler(0, look.eulerAngles.y, 0);

                // transform.rotation = Quaternion.Euler(0, followTarget.rotation.eulerAngles.y, 0);
                // followTarget.localEulerAngles = new Vector3(angles.x, 0, 0);
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
        if (lookInput.magnitude >= 0.1f)
        {
            player.isReturning = false;
            // onAimRotate(lookInput.x, lookInput.y);
        }

    }

    public void onAimRotate(float x, float y)
    {
        // xRotation -= y * aimSensitivity.x * Time.deltaTime;
        // xRotation = Mathf.Clamp(xRotation, xLimitMin, xLimitMax);
        // followTarget.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        float moveX = x * aimSensitivity.y * Time.deltaTime;
        player.transform.Rotate(Vector3.up * moveX);
    }

    public void onAimRotate1(float x, float y)
    {
        xRotation -= y * aimSensitivity.x * Time.deltaTime;
        xRotation = Mathf.Clamp(xRotation, xLimitMin, xLimitMax);
        aimModeCamera.transform.rotation = Quaternion.Euler(xRotation, 0, 0);
        // float moveX = x * aimSensitivity.y * Time.deltaTime;
        // player.transform.Rotate(Vector3.up * moveX);
    }

    public void SwapShoulder()
    {
        var aimModeCameraBody = aimModeCamera.GetCinemachineComponent<Cinemachine3rdPersonFollow>();
        if (player.isAiming)
        {
            aimModeCameraBody.CameraSide = aimModeCameraBody.CameraSide == 1 ? 0 : 1;
        }
    }
}

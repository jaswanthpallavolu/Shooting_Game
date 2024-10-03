using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerCameraController playerCameraController;
    [HideInInspector] public ParkourController parkourController;

    protected override void Awake()
    {
        base.Awake();

        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerCameraController = GetComponent<PlayerCameraController>();
        parkourController = GetComponent<ParkourController>();
    }

    protected override void Start()
    {
        base.Start();
        PlayerInputManager.instance.player = this;

    }

    protected override void Update()
    {
        base.Update();
    }
}

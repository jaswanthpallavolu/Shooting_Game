using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : CharacterManager
{
    [HideInInspector] public PlayerLocomotionManager playerLocomotionManager;
    [HideInInspector] public PlayerAnimatorManager playerAnimatorManager;
    [HideInInspector] public PlayerCameraController playerCameraController;
    [HideInInspector] public ParkourController parkourController;
    [HideInInspector] public PlayerInventoryManager playerInventoryManager;
    [HideInInspector] public PlayerEquipmentManager playerEquipmentManager;

    protected override void Awake()
    {
        base.Awake();

        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
        Time.timeScale = 0;

        playerLocomotionManager = GetComponent<PlayerLocomotionManager>();
        playerAnimatorManager = GetComponentInChildren<PlayerAnimatorManager>();
        playerCameraController = GetComponent<PlayerCameraController>();
        parkourController = GetComponent<ParkourController>();
        playerInventoryManager = GetComponent<PlayerInventoryManager>();
        playerEquipmentManager = GetComponent<PlayerEquipmentManager>();
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


    public void OnCurrentRighHandWeaponIdChange(int oldId, int newId)
    {
        WeaponItem weapon = Instantiate(WorldItemDatabase.instance.GetWeaponById(newId));
        playerInventoryManager.currentRightHandWeapon = weapon;
        playerEquipmentManager.LoadRightWeapon();
    }
}

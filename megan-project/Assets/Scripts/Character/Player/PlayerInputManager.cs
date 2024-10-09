using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
// using UnityEngine.InputSystem;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    PlayerControls playerControls;
    [HideInInspector] public PlayerManager player;

    [Header("Movement Input")]
    [SerializeField] Vector2 movementInput;
    public float verticalInput, horizontalInput, moveAmount;

    [Header("Aiming")]
    public bool aimInput = false;
    public Vector2 lookInput;

    [Header("Actions")]
    public bool jumpInput = false;
    public bool sprintInput = false;

    [Header("Weapon")]
    public bool equipWeapon1 = false;
    public bool equipWeapon2 = false;
    public bool unEquip = false;


    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }
    }

    void Update()
    {
        HandleMovementInput();
        HandleAimInput();
        HandleJumpInput();
        HandleSprintInput();
        HandleEquipWeaponInput();
        HandleUnEquipWeaponInput();
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            playerControls = new PlayerControls();
        }
        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControls.PlayerMovement.Movement.canceled += i => movementInput = i.ReadValue<Vector2>();
        playerControls.PlayerMovement.Look.performed += i => lookInput = i.ReadValue<Vector2>();
        playerControls.PlayerMovement.Look.canceled += i => lookInput = i.ReadValue<Vector2>();

        playerControls.PlayerAim.Aim.performed += i => aimInput = true;
        playerControls.PlayerAim.Aim.canceled += i => aimInput = false;

        playerControls.PlayerActions.Jump.performed += i => jumpInput = true;
        playerControls.PlayerActions.Sprint.performed += i => sprintInput = true;
        playerControls.PlayerActions.Sprint.canceled += i => sprintInput = false;

        playerControls.Weapon.Weapon1.performed += i => equipWeapon1 = true;
        playerControls.Weapon.Weapon2.performed += i => equipWeapon2 = true;
        playerControls.Weapon.UnEquip.performed += i => unEquip = true;

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void HandleMovementInput()
    {
        horizontalInput = movementInput.x;
        verticalInput = movementInput.y;
        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));

        if (player.isOnLedge)
        {
            moveAmount = 0;
        }
        else if (player.movementType == MovementType.Strafe)
        {
            if (horizontalInput != 0 || verticalInput < 0)
            {
                moveAmount = 0.5f;
            }
            moveAmount = 0.5f;
        }

        if (player.movementType == MovementType.Forward)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, moveAmount, player.isSprinting);
        }
        else if (player.movementType == MovementType.Strafe)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(moveAmount > 0 ? horizontalInput : 0, moveAmount > 0 ? verticalInput : 0, player.isSprinting);
        }


    }

    private void HandleAimInput()
    {
        if (!player.canMove) return;
        player.isAiming = aimInput;
        player.playerCameraController.HandleAimMode(aimInput);
    }

    private void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            if (player.isPerformingAction) return;
            player.parkourController.HandleActions();
        }
    }

    private void HandleSprintInput()
    {
        player.playerLocomotionManager.HandleSprint(sprintInput);
    }

    private void HandleEquipWeaponInput()
    {
        if (equipWeapon1)
        {
            equipWeapon1 = false;
            // player.animator.SetBool("HG_Equip", true);
            player.playerAnimatorManager.PlayTargetAnimation("HG_Equip", true, false, true, true);
            player.animator.SetBool("handgun", true);
        }
    }

    private void HandleUnEquipWeaponInput()
    {
        if (unEquip)
        {
            unEquip = false;
            player.animator.SetBool("unEquip", true);
            player.animator.SetBool("handgun", false);
            // player.playerAnimatorManager.PlayTargetAnimation("HG_Equip",true);
        }
    }
}
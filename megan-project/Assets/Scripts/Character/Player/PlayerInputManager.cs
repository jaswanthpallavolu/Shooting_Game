using System.Collections;
using System.Collections.Generic;
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
        bool damp = true;

        moveAmount = Mathf.Clamp01(Mathf.Abs(horizontalInput) + Mathf.Abs(verticalInput));
        if (player.movementType == MovementType.Forward)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(0, !player.isOnLedge ? moveAmount : 0, false, damp);
        }
        else if (player.movementType == MovementType.Straf)
        {
            player.playerAnimatorManager.UpdateAnimatorMovementParameters(horizontalInput, verticalInput, false, damp);
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
}
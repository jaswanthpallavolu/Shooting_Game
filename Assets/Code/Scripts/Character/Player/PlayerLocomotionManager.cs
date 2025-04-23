using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class PlayerLocomotionManager : CharacterLocomotionManager
{
    PlayerManager player;

    [Header("Movement")]
    [SerializeField] float walkingSpeed = 2f;
    [SerializeField] float runningSpeed = 4f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float aimMovementSpeed = 2f;

    [SerializeField] Vector3 moveDirection;
    [SerializeField] Vector3 prevMoveDirection;
    float turnSmoothVelocity;
    float verticalInput, horizontalInput, moveAmount;

    [Header("Debug")]
    [SerializeField] float debugMoveSpeed;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<PlayerManager>();
    }

    protected override void Update()
    {
        base.Update();

        verticalInput = PlayerInputManager.instance.verticalInput;
        horizontalInput = PlayerInputManager.instance.horizontalInput;
        moveAmount = PlayerInputManager.instance.moveAmount;

        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        HandleLedgeCheck();
        HandleMovement();

    }

    private void HandleLedgeCheck()
    {
        if (player.isGrounded)
        {
            player.isOnLedge = player.environmentScanner.LedgeCheck(moveDirection, out player.ledgeData);
        }
        prevMoveDirection = moveDirection;

        if (player.isOnLedge)
        {
            HandleLedgeMovement();
        }
    }

    private void HandleLedgeMovement()
    {
        float signedAngle = Vector3.SignedAngle(player.ledgeData.surfaceHit.normal, moveDirection, Vector3.up);
        float angle = Mathf.Abs(signedAngle);

        if (Vector3.Angle(moveDirection, player.transform.forward) >= 45)
        {
            // player.canMove = false;
            // DONT MOVE, DO ROTATE
            // TURN THE BODY TO FACE THE INPUT DIRECTION
            float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float transformAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0, transformAngle, 0);
            return;
        }

        Debug.Log(angle);
        if (angle < 60)
        {
            // player.canMove = false;
            moveDirection = Vector3.zero;
        }
        else if (angle < 90)
        {
            Vector3 left = Vector3.Cross(Vector3.up, player.ledgeData.surfaceHit.normal);
            Vector3 dir = left * Mathf.Sign(signedAngle);
            player.canMove = true;
            moveDirection = dir;
        }
    }

    private void HandleMovement()
    {
        if (!player.canMove) return;
        if (player.movementType == MovementType.Forward)
        {
            ForwardMovement(moveDirection);
        }
        else if (player.movementType == MovementType.Strafe)
        {
            StrafeMovement(moveDirection);
        }
    }

    private void StrafeMovement(Vector3 moveDirection)
    {
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (moveDirection.magnitude >= 0.1f)
        {
            // TURN THE BODY TO FACE THE CAMERA FORWARD DIRECTION
            // Quaternion look = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            // float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, look.eulerAngles.y, ref turnSmoothVelocity, .1f);
            // transform.rotation = Quaternion.Euler(0f, angle, 0);
            player.playerCameraController.HandlePlayerRotation();

            float movementSpeed = GetMovementSpeed(moveAmount);
            debugMoveSpeed = movementSpeed;

            player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
    }

    private void ForwardMovement(Vector3 moveDirection)
    {
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (moveDirection.magnitude >= 0.1f)
        {
            // TURN THE BODY TO FACE THE INPUT DIRECTION
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float transformAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0, transformAngle, 0);

            float movementSpeed = GetMovementSpeed(moveAmount);
            debugMoveSpeed = movementSpeed;
            Vector3 forwardDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            player.characterController.Move(forwardDirection * movementSpeed * Time.deltaTime);
        }
    }

    private float GetMovementSpeed(float moveAmount)
    {
        if (player.isAiming)
        {
            return aimMovementSpeed;
        }

        if (player.isSprinting)
        {
            return sprintSpeed;
        }

        if (moveAmount <= 0.5f)
        {
            return walkingSpeed;
        }
        else if (moveAmount > 0.5f && moveAmount <= 1f)
        {
            return runningSpeed;
        }

        return walkingSpeed;
    }

    public void HandleSprint(bool sprintInput)
    {
        if (sprintInput && moveAmount > 0)
        {
            bool strafeForwardMovement = player.movementType == MovementType.Strafe && horizontalInput == 0 && verticalInput > 0;
            if (player.movementType == MovementType.Forward || strafeForwardMovement)
            {
                player.isSprinting = true;
            }
            else
            {
                player.isSprinting = false;
            }
        }
        else
        {
            player.isSprinting = false;
        }
    }

}

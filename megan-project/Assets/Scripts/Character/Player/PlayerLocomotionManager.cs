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
    Vector3 moveDirection;
    float turnSmoothVelocity;
    float verticalInput, horizontalInput, moveAmount;
    bool isSprinting = false;

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

        HandleMovement();
    }

    private void HandleMovement()
    {
        if (player.movementType == MovementType.Forward)
        {
            ForwardMovement();
        }
        else if (player.movementType == MovementType.Straf)
        {
            StrafMovement();
        }
    }

    private void StrafMovement()
    {
        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (moveDirection.magnitude >= 0.1f)
        {
            // TURN THE BODY TO FACE THE CAMERA FORWARD DIRECTION
            Quaternion look = Quaternion.LookRotation(Camera.main.transform.forward, Camera.main.transform.up);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, look.eulerAngles.y, ref turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0f, angle, 0);

            float movementSpeed = GetMovementSpeed(moveAmount);

            player.characterController.Move(moveDirection * movementSpeed * Time.deltaTime);
        }
    }

    private void ForwardMovement()
    {
        moveDirection = new Vector3(horizontalInput, 0, verticalInput);
        moveDirection = Camera.main.transform.TransformDirection(moveDirection);
        moveDirection.Normalize();
        moveDirection.y = 0;
        if (moveDirection.magnitude >= 0.1f)
        {
            // TURN THE BODY TO FACE THE INPUT DIRECTION
            float angle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
            float transformAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, angle, ref turnSmoothVelocity, .1f);
            transform.rotation = Quaternion.Euler(0, transformAngle, 0);

            float movementSpeed = GetMovementSpeed(moveAmount);
            Vector3 forwardDirection = Quaternion.Euler(0, angle, 0) * Vector3.forward;
            player.characterController.Move(forwardDirection * movementSpeed * Time.deltaTime);
        }
    }

    private float GetMovementSpeed(float moveAmount)
    {
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


}

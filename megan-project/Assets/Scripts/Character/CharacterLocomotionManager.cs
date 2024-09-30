using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;
    EnvironmentScanner environmentScanner;

    [Header("Grounded check & Jumping")]
    [SerializeField] protected float groundForce = -9.18f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundSphereRadius = .5f;
    [SerializeField] protected Vector3 yVelocity;
    [SerializeField] protected float groundedYVelocity = -20f;
    [SerializeField] protected float fallStartYVelocity = -5f;
    protected bool fallingVelocityHasBeenSet = false;
    protected float inAirTimer = 0;

    protected virtual void Awake()
    {
        character = GetComponent<CharacterManager>();
        environmentScanner = GetComponent<EnvironmentScanner>();
    }

    protected virtual void Update()
    {
        if (character.isPerformingAction) return;

        HandleGroundCheck();

        if (character.isGrounded)
        {

            if (environmentScanner != null)
            {
                Vector3 moveDirection = new Vector3(PlayerInputManager.instance.horizontalInput, 0, PlayerInputManager.instance.verticalInput);
                moveDirection = Camera.main.transform.TransformDirection(moveDirection);
                character.isOnLedge = environmentScanner.LedgeCheck(moveDirection);
                if (character.isOnLedge) Debug.Log("yes it is ledge");
                else Debug.Log("it is not ledge");
            }

            // IF WE ARE NOT ATTEMPTING TO JUMP OR MOVE UPWARD
            if (yVelocity.y < 0)
            {
                inAirTimer = 0;
                fallingVelocityHasBeenSet = false;
                yVelocity.y = groundedYVelocity;
            }
        }
        else
        {
            // IF WE ARE NOT JUMPING AND FALL VELOCITY HAS NOT BEEN SET
            if (!character.isJumping && !fallingVelocityHasBeenSet)
            {
                fallingVelocityHasBeenSet = true;
                yVelocity.y = fallStartYVelocity;
            }
            inAirTimer += Time.deltaTime;
            character.animator.SetFloat("inAirTimer", inAirTimer);
            yVelocity.y += groundForce * Time.deltaTime;

        }

        character.characterController.Move(yVelocity * Time.deltaTime);
    }

    protected void HandleGroundCheck()
    {
        character.isGrounded = Physics.CheckSphere(character.transform.position, groundSphereRadius, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (character)
        {
            Gizmos.DrawSphere(character.transform.position, groundSphereRadius);
        }
    }
}

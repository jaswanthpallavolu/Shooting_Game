using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocomotionManager : MonoBehaviour
{
    CharacterManager character;

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
    }

    protected virtual void Update()
    {
        if (!character.isPerformingAction)
        {

            HandleGroundCheck();

            if (character.isGrounded)
            {

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

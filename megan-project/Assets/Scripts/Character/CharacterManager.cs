using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    [HideInInspector] public Animator animator;
    [HideInInspector] public CharacterAnimatorManager characterAnimatorManager;
    [HideInInspector] public EnvironmentScanner environmentScanner;

    [Header("Flags")]
    public bool isPerformingAction = false;
    public bool applyRootMotion = false;
    public bool applyBuiltInRootMotion = false;
    public bool isJumping = false;
    public bool isSprinting = false;
    public bool isGrounded = false;
    public bool canMove = false;
    public bool canRotate = false;
    public bool isDead = false;
    public bool isOwner = true;
    public bool isAiming = false;

    [Header("Custom")]
    public MovementType movementType;

    [Header("Parkour Flags")]
    public bool isPerformingParkourAction = false;
    public bool isOnLedge = false;
    public LedgeData ledgeData;

    protected virtual void Awake()
    {
        // DontDestroyOnLoad(this);
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
        characterAnimatorManager = GetComponentInChildren<CharacterAnimatorManager>();
        environmentScanner = GetComponent<EnvironmentScanner>();
    }

    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {

    }
}

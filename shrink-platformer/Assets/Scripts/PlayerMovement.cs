using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] private Transform cameraTransform;

    public PlayerSize playerSize;

    private Vector2 movement;
    private float playerSpeed;
    [SerializeField] private float walkingPlayerSpeed;
    [SerializeField] private float sprintingPlayerSpeed;
    private float shrunkenWalkingPlayerSpeed = 5f;
    private float shrunkenSprintingPlayerSpeed = 5f;

    public float shrunkenSpeedModifier;

    private float rotationSpeed = 5f;


    private bool isSprintPressed = false;

    bool isJumpPressed = false;
    bool isJumping = false;

    float initialJumpVelocity;

    private float maxJumpHeight;

    [SerializeField] private float shrunkenJumpHeightModifier;

    [SerializeField] private float maxJumpTime;

    [SerializeField] private float normalMaxJumpHeight = 5.0f;
    private float shrunkenMaxJumpHeight;

    [SerializeField] private float coyoteTime = 0.05f;
    private float lastTimeGrounded;

    private float gravity;
    private float groundedGravity = -0.5f;

    private bool isGrounded;

    [HideInInspector] public bool isShrunken = false;

    private Vector3 velocity;

    private AudioSource audioSource;
    [SerializeField] private AudioClip jumpSound;

    private Animator anim;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        anim = GetComponent<Animator>();

        characterController = GetComponent<CharacterController>();

        HandleGravity();

        SetUpJumpVariables();


        shrunkenWalkingPlayerSpeed = walkingPlayerSpeed * shrunkenSpeedModifier;
        shrunkenSprintingPlayerSpeed = sprintingPlayerSpeed * shrunkenSpeedModifier;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        HandleSpeed();
        Movement();

        HandleGravity();
        HandleJump();

        PlatformManager();

    }

    void SetUpJumpVariables()
    {
        shrunkenMaxJumpHeight = (normalMaxJumpHeight + shrunkenJumpHeightModifier) * playerSize.shrinkScale;

        //Math to make jump variables accurate
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    public void OnJump(InputValue jumpVal)
    {
        //gets the value from isJumpPressed as a float (0 = not pressed, 1 = pressed) and converts it to a boolean
        isJumpPressed = jumpVal.Get<float>() > 0;

        SetUpJumpVariables();

    }

    void HandleJump()
    {
        bool canCoyoteTime = Time.time - lastTimeGrounded <= coyoteTime;

        if (isShrunken == false)
        {
            maxJumpHeight = normalMaxJumpHeight;
        } else
        {
            maxJumpHeight = shrunkenMaxJumpHeight;
        }


        if ((characterController.isGrounded || canCoyoteTime) && !isJumping && isJumpPressed)
        {
            isJumping = true;
            PlaySoundEffect();
            anim.SetTrigger("IsJumping");
            currentVerticalMovement.y = initialJumpVelocity * 0.5f;


            //Detects when player lands
        }
        else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
            
    }

    public void ForceFall()
    {
        currentVerticalMovement.y = -1f;
    }

    private Vector3 currentVerticalMovement;

    void HandleGravity ()
    {
        //Detects when player is falling or releases the jump button and speeds the fall up
        bool isFalling = currentVerticalMovement.y <= 0 || !isJumpPressed;
        float fallMultiplier = 2.5f;

        if (characterController.isGrounded)
        {
            currentVerticalMovement.y = groundedGravity;
            lastTimeGrounded = Time.time;
        } else if (isFalling) {
            float previousYVelocity = currentVerticalMovement.y;
            float newYVelocity = currentVerticalMovement.y + (gravity * fallMultiplier * Time.fixedDeltaTime);

        //Also clamps falling velocity so you dont fall above -40 speed
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -40.0f);
            currentVerticalMovement.y = nextYVelocity;
        } else {
            //Uses velocity verlet to make jump trajectories consistent between framerates
            float previousYVelocity = currentVerticalMovement.y;
            float newYVelocity = currentVerticalMovement.y + (gravity * Time.fixedDeltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentVerticalMovement.y = nextYVelocity;
        }
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    private void PlaySoundEffect()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(jumpSound);
    }
    private void Movement()
    {
        float MoveX = movement.x;
        float MoveZ = movement.y;

        //Detects the position of the camera and moves the player based off its position
        Vector3 cameraForward = cameraTransform.forward.normalized;
        Vector3 cameraRight = cameraTransform.right.normalized;
        cameraForward.y = 0; cameraRight.y = 0;

        Vector3 moveDirection = ((cameraForward * MoveZ) + (cameraRight * MoveX)).normalized;
        Vector3 horizontalMovement = moveDirection * playerSpeed;

        Vector3 finalMovement = new Vector3(horizontalMovement.x, currentVerticalMovement.y, horizontalMovement.z);

            characterController.Move(finalMovement * Time.fixedDeltaTime);

        if (horizontalMovement.magnitude > 0.01f)
        {
            anim.SetBool("IsWalking", true);
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        } else
        {
            anim.SetBool("IsWalking", false);
        }
         
    }

    private void OnSprint(InputValue sprintVal)
    {
        isSprintPressed = sprintVal.Get<float>() > 0;
    }

    public void HandleSpeed()
    {
        if (isShrunken)
        {
            if (isSprintPressed)
            {
                playerSpeed = shrunkenSprintingPlayerSpeed;
            } else
            {
                playerSpeed = shrunkenWalkingPlayerSpeed;
            }
        }
        if (!isShrunken)
        {
            if (isSprintPressed)
            {
                playerSpeed = sprintingPlayerSpeed;
            }
            else
            {
                playerSpeed = walkingPlayerSpeed;
            }
        }

    }

    void PlatformManager()
    {
        if (!isShrunken)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ShrunkenOnlyPlatform"), true);
        } else
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ShrunkenOnlyPlatform"), false);
        }
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;

    [SerializeField] private Transform cameraTransform;


    private Vector2 movement;
    private float playerSpeed;
    [SerializeField] private float walkingPlayerSpeed = 5f;
    [SerializeField] private float sprintingPlayerSpeed = 5f;
    [SerializeField] private float shrunkenWalkingPlayerSpeed = 5f;
    [SerializeField] private float shrunkenSprintingPlayerSpeed = 5f;

    private float rotationSpeed = 5f;


    private bool isSprintPressed = false;

    bool isJumpPressed = false;
    bool isJumping = false;

    float initialJumpVelocity;

    private float maxJumpHeight = 5.0f;

    [SerializeField] private float maxJumpTime;

    [SerializeField] private float normalMaxJumpHeight = 5.0f;
    [SerializeField] private float shrunkenMaxJumpHeight = 2.0f;

    float gravity;
    float groundedGravity = -0.5f;


    private bool isGrounded;

    [HideInInspector] public bool isShrunken = false;

    private Vector3 velocity;
    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        SetUpJumpVariables();
    }

    // Update is called once per frame
    void Update()
    {
        HandleSpeed();
        Movement();

        //Debug.Log("Shrunken: " + isShrunken);
        //Debug.Log("Sprinting " + isSprintPressed);

        HandleGravity();
        HandleJump();

    }

    void SetUpJumpVariables()
    {
        //Math to make jump variables accurate
        float timeToApex = maxJumpTime / 2;
        gravity = (-2 * maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        initialJumpVelocity = (2 * maxJumpHeight) / timeToApex;
    }

    private void OnJump(InputValue jumpVal)
    {
        //gets the value from isJumpPressed as a float (0 = not pressed, 1 = pressed) and converts it to a boolean
        isJumpPressed = jumpVal.Get<float>() > 0;
        Debug.Log(isJumpPressed);
    }

    void HandleJump()
    {
        if (isShrunken == false)
        {
            maxJumpHeight = normalMaxJumpHeight;
        } else
        {
            maxJumpHeight = shrunkenMaxJumpHeight;
        }


        if (!isJumping && characterController.isGrounded == true && isJumpPressed)
        {
            isJumping = true;
            currentMovement.y = initialJumpVelocity * 0.5f;

            //Detects when player lands
        } else if (!isJumpPressed && isJumping && characterController.isGrounded)
        {
            isJumping = false;
        }
            
    }

    private Vector3 currentMovement;

    void HandleGravity ()
    {
        //Detects when player is falling or releases the jump button and speeds the fall up
        bool isFalling = currentMovement.y <= 0 || !isJumpPressed;
        float fallMultiplier = 2.0f;

        if (characterController.isGrounded)
        {
            currentMovement.y = groundedGravity;
        } else if (isFalling) {
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * fallMultiplier * Time.deltaTime);
        //Also clamps falling velocity so you dont fall above -40 speed
            float nextYVelocity = Mathf.Max((previousYVelocity + newYVelocity) * 0.5f, -40.0f);
            currentMovement.y = nextYVelocity;
        } else {
            //Uses velocity verlet to make jump trajectories consistent between framerates
            float previousYVelocity = currentMovement.y;
            float newYVelocity = currentMovement.y + (gravity * Time.deltaTime);
            float nextYVelocity = (previousYVelocity + newYVelocity) * 0.5f;
            currentMovement.y = nextYVelocity;
        }
    }

    void OnMove(InputValue moveVal)
    {
        movement = moveVal.Get<Vector2>();
    }

    private void Movement()
    {
        float MoveX = movement.x;
        float MoveZ = movement.y;

        //Detects the position of the camera and moves the player based off its position
        Vector3 cameraForward = cameraTransform.forward.normalized;
        Vector3 cameraRight = cameraTransform.right.normalized;
        cameraForward.y = 0; cameraRight.y = 0;

        Vector3 horizontalMovement = (cameraForward * MoveZ) + (cameraRight * MoveX);

        currentMovement = new Vector3(horizontalMovement.x * playerSpeed, currentMovement.y, horizontalMovement.z * playerSpeed);

            characterController.Move(currentMovement * Time.deltaTime);

        if (horizontalMovement.magnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalMovement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
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
}

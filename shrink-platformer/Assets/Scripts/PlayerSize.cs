using Unity.Cinemachine;
using UnityEngine;

public class PlayerSize : MonoBehaviour
{
    private CameraManager cameraManager;
    private PlayerMovement playerMovement;

    private Transform playerTransform;
    private CharacterController characterController;

    private bool shrinkButtonPressed;
    private bool isShrunk = false;
    private float shrinkScale = 0.2f;
    private float shrinkSpeed = 1f;

    private Vector3 originalScale;
    private float originalCharacterControllerHeight;
    private Vector3 originalCharacterControllerCenter;


    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {
        cameraManager = new CameraManager();
        playerMovement = new PlayerMovement();
        originalScale = transform.localScale;
        originalCharacterControllerHeight = characterController.height;
        originalCharacterControllerCenter = characterController.center;

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void OnSizeChange()
    {

        if (!isShrunk && !playerMovement.isShrunken)
        {
            ShrinkPlayer();
            
            Debug.Log("Shrinking");

        }
        else if (isShrunk && playerMovement.isShrunken)
        {
            RegrowPlayer();
            Debug.Log("Growing");

        }
    }

    private void ShrinkPlayer()
    {
        transform.localScale = originalScale * shrinkScale;
        characterController.height = originalCharacterControllerHeight;
        characterController.center = originalCharacterControllerCenter;
      
        isShrunk = true;
        playerMovement.isShrunken = true;
        cameraManager.OnPlayerShrink();

    }

    private void RegrowPlayer()
    {
        transform.localScale = originalScale;
        characterController.height = originalCharacterControllerHeight;
        characterController.center = originalCharacterControllerCenter;

        isShrunk = false;
        playerMovement.isShrunken = false;
        cameraManager.OnPlayerRegrow();

    }
}

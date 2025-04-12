using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerSize : MonoBehaviour
{
    public CameraManager cameraManager;
    public PlayerMovement playerMovement;

    private Transform playerTransform;
    private CharacterController characterController;

    private bool shrinkButtonPressed;
    private bool isShrunk = false;
    public float shrinkScale = 0.15f;

    private Vector3 originalScale;
    private float originalCharacterControllerHeight;
    private Vector3 originalCharacterControllerCenter;

    private float originalCharacterControllerStepOffset;
    private float originalCharacterControllerSkinWidth;

    private float shrunkenSkinWidth = 0.03f;
    private void Awake()
    {
        playerTransform = GetComponent<Transform>();
        characterController = GetComponent<CharacterController>();
    }
    void Start()
    {

        originalScale = transform.localScale;
        originalCharacterControllerHeight = characterController.height;
        originalCharacterControllerCenter = characterController.center;

        originalCharacterControllerStepOffset = characterController.stepOffset;
        originalCharacterControllerSkinWidth = characterController.skinWidth;

    }

    private float sizeChangeCooldown = 1.5f;
    private bool canChangeSize = true;
    public void OnSizeChange()
    {

        if (!isShrunk && !playerMovement.isShrunken && canChangeSize)
        {
            ShrinkPlayer();
            StartCoroutine(SizeChangeCooldownRoutine());

        }
        else if (isShrunk && playerMovement.isShrunken && canChangeSize)
        {
            RegrowPlayer();
            StartCoroutine(SizeChangeCooldownRoutine());


        }
    }

    private IEnumerator SizeChangeCooldownRoutine()
    {
        canChangeSize = false;
        yield return new WaitForSeconds(sizeChangeCooldown);
        canChangeSize = true;
        Debug.Log("Can Change Size Again");
    }

    private void ShrinkPlayer()
    {
        transform.localScale = originalScale * shrinkScale;
        
        characterController.height = originalCharacterControllerHeight;

        characterController.center = originalCharacterControllerCenter;

        characterController.stepOffset = originalCharacterControllerStepOffset * shrinkScale;

        characterController.skinWidth = shrunkenSkinWidth;


        characterController.Move(Vector3.up * 0.5f);

        isShrunk = true;
        playerMovement.isShrunken = true;
        cameraManager.OnPlayerShrink();

    }

    private void RegrowPlayer()
    {
        transform.localScale = originalScale;
        characterController.height = originalCharacterControllerHeight;
        characterController.center = originalCharacterControllerCenter;

        characterController.stepOffset = originalCharacterControllerStepOffset;
        characterController.skinWidth = originalCharacterControllerSkinWidth;


        isShrunk = false;
        playerMovement.isShrunken = false;

        cameraManager.OnPlayerRegrow();

    }

}

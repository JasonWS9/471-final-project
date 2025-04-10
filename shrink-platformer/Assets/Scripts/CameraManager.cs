using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    private CinemachineOrbitalFollow orbitCam;
    private CinemachineDecollider decollider;
    private CinemachineRotationComposer rotationComposer;

    private float shrunkenOrbitModifier = 0.2f;

    private float normalTopOrbitHeight;
    private float normalTopOrbitRadius;

    private float normalMiddleOrbitHeight;
    private float normalMiddleOrbitRadius;

    private float normalBottomOrbitHeight;
    private float normalBottomOrbitRadius;


    public float shrunkenTopOrbitHeight;
    public float shrunkenTopOrbitRadius;

    public float shrunkenMiddleOrbitHeight;
    public float shrunkenMiddleOrbitRadius;

    public float shrunkenBottomOrbitHeight;
    public float shrunkenBottomOrbitRadius;

    private float normalDecolliderRadius;
    [SerializeField] private float shrunkenDecolliderRadius = 2.0f;

    private float normalRotationComposerOffset;
    private float shrunkenRotationComposerOffset = 0.225f;


    void Start()
    {
        orbitCam = GetComponent<CinemachineOrbitalFollow>();
        decollider = GetComponent<CinemachineDecollider>();
        rotationComposer = GetComponent<CinemachineRotationComposer>();

        normalTopOrbitHeight = orbitCam.Orbits.Top.Height;
        normalTopOrbitRadius = orbitCam.Orbits.Top.Radius;

        normalMiddleOrbitHeight = orbitCam.Orbits.Center.Height;
        normalMiddleOrbitRadius = orbitCam.Orbits.Center.Radius;

        normalBottomOrbitHeight = orbitCam.Orbits.Bottom.Height;
        normalBottomOrbitRadius = orbitCam.Orbits.Bottom.Radius;

        shrunkenTopOrbitHeight = normalTopOrbitHeight * shrunkenOrbitModifier;
        shrunkenTopOrbitRadius = normalTopOrbitRadius * shrunkenOrbitModifier;

        shrunkenMiddleOrbitHeight = normalMiddleOrbitHeight * shrunkenOrbitModifier;
        shrunkenMiddleOrbitRadius = normalMiddleOrbitRadius * shrunkenOrbitModifier;

        shrunkenBottomOrbitHeight = normalBottomOrbitHeight * shrunkenOrbitModifier;
        shrunkenBottomOrbitRadius = normalBottomOrbitRadius * shrunkenOrbitModifier;

        normalDecolliderRadius = decollider.CameraRadius;

        normalRotationComposerOffset = rotationComposer.TargetOffset.y;
       
    }

    public void OnPlayerShrink()
    {
        

        orbitCam.Orbits.Top.Height = shrunkenTopOrbitHeight;
        orbitCam.Orbits.Top.Radius = shrunkenTopOrbitRadius;

        orbitCam.Orbits.Center.Height = shrunkenMiddleOrbitHeight;
        orbitCam.Orbits.Center.Radius = shrunkenMiddleOrbitRadius;

        orbitCam.Orbits.Bottom.Height = shrunkenBottomOrbitHeight;
        orbitCam.Orbits.Bottom.Radius = shrunkenBottomOrbitRadius;

        decollider.CameraRadius = shrunkenDecolliderRadius;

        rotationComposer.TargetOffset.y = shrunkenRotationComposerOffset;
    }

    public void OnPlayerRegrow()
    {
        orbitCam.Orbits.Top.Height = normalTopOrbitHeight;
        orbitCam.Orbits.Top.Radius = normalTopOrbitRadius;

        orbitCam.Orbits.Center.Height = normalMiddleOrbitHeight;
        orbitCam.Orbits.Center.Radius = normalMiddleOrbitRadius;

        orbitCam.Orbits.Bottom.Height = normalBottomOrbitHeight;
        orbitCam.Orbits.Bottom.Radius = normalBottomOrbitRadius;

        decollider.CameraRadius = normalDecolliderRadius;

        rotationComposer.TargetOffset.y = normalRotationComposerOffset;

    }
}

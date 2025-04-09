using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{


    [SerializeField] public CinemachineOrbitalFollow orbitCam;

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



    void Start()
    {
        normalTopOrbitHeight = orbitCam.Orbits.Top.Height;
        normalTopOrbitRadius = orbitCam.Orbits.Top.Radius;

        normalMiddleOrbitHeight = orbitCam.Orbits.Center.Height;
        normalMiddleOrbitRadius = orbitCam.Orbits.Center.Radius;

        normalBottomOrbitHeight = orbitCam.Orbits.Bottom.Height;
        normalBottomOrbitRadius = orbitCam.Orbits.Bottom.Radius;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnPlayerShrink()
    {
        orbitCam.Orbits.Top.Height = shrunkenTopOrbitHeight;
        orbitCam.Orbits.Top.Radius = shrunkenTopOrbitRadius;

        orbitCam.Orbits.Center.Height = shrunkenMiddleOrbitHeight;
        orbitCam.Orbits.Center.Radius = shrunkenMiddleOrbitRadius;

        orbitCam.Orbits.Bottom.Height = shrunkenBottomOrbitHeight;
        orbitCam.Orbits.Bottom.Radius = shrunkenBottomOrbitRadius;
    }

    public void OnPlayerRegrow()
    {
        orbitCam.Orbits.Top.Height = normalTopOrbitHeight;
        orbitCam.Orbits.Top.Radius = normalTopOrbitRadius;

        orbitCam.Orbits.Center.Height = normalMiddleOrbitHeight;
        orbitCam.Orbits.Center.Radius = normalMiddleOrbitRadius;

        orbitCam.Orbits.Bottom.Height = normalBottomOrbitHeight;
        orbitCam.Orbits.Bottom.Radius = normalBottomOrbitRadius;
    }
}

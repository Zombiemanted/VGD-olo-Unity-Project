using System.Threading;
using Unity.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 cameraRotation;
    InputAction lookVector;
    Camera playerCam;
    Transform camHolder;

    public float ForwardBackNow; //vert
    public float SidewaysNow; //horiz

    public float ishowspeed = 5f; //you'll never guess
    public float howMuchBoof = 10f; //jump power
    public float XLibSensitivity = 0.5f;
    public float YRetSensitivity = 0.5f;
    public float rotationalLimitations = 90.0f;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        lookVector = GetComponent<PlayerInput>().currentActionMap.FindAction("Look");
        cameraRotation = Vector2.zero;
        camHolder = transform.GetChild(0); //why'd he do that whos holding it
    }

    private void Update()
    {
        //this moves my camera
        playerCam.transform.position = camHolder.position;

        cameraRotation.x += lookVector.ReadValue<Vector2>().x * XLibSensitivity;
        cameraRotation.y += lookVector.ReadValue<Vector2>().y * YRetSensitivity;

        cameraRotation.y = Mathf.Clamp(cameraRotation.y, -rotationalLimitations, rotationalLimitations);

        playerCam.transform.rotation = Quaternion.Euler(-cameraRotation.y, cameraRotation.x, 0);
        transform.localRotation = Quaternion.AngleAxis(cameraRotation.x, Vector3.up);

        //bro move
        Vector3 temp = rb.linearVelocity;

        temp.x = ForwardBackNow * ishowspeed;
        temp.z = SidewaysNow * ishowspeed;

        rb.linearVelocity = (temp.x * transform.forward) + (temp.y * transform.up) + (temp.z * transform.right);
    }

    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();
        ForwardBackNow = inputAxis.y;
        SidewaysNow = inputAxis.x;
    }
}

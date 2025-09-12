using NUnit.Framework.Constraints;
using System.Threading;
using Unity.Collections;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Rigidbody rb;
    Vector2 cameraRotation;
    InputAction lookVector;
    Transform playerCam;
    Vector3 cameraOffset;
    public Vector3 respawn;

    public float ForwardBackNow; //vert
    public float SidewaysNow; //horiz

    public float ishowspeed = 10f; //you'll never guess
    public float howMuchBoof = 10f; //jump power
    public float XLibSensitivity = 0.5f;
    public float YRetSensitivity = 0.5f;
    public float rotationalLimitations = 90.0f;

    public int healthy = 5;
    public int maxHealthy = 10;

    public void Start()
    {
        respawn = new Vector3(0, 1, 0);
        cameraOffset = new Vector3(0, .5f, .15f);
        rb = GetComponent<Rigidbody>();
        playerCam = transform.GetChild(0);
        lookVector = GetComponent<PlayerInput>().currentActionMap.FindAction("Look");
        cameraRotation = Vector2.zero;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (healthy <= 0) //Can i nest logic please
        {
            print("GameOverLol"); //memory managers be damned im adding like 2 bytes
            transform.position = respawn;
            healthy = 5; //maxhealthy is only achieved via items Dingleberry
        }

        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //Reloads scene

        //this moves my camera
        //playerCam.transform.position = transform.position + cameraOffset;

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

    public void Move(InputAction.CallbackContext context) //take a wild fucking guess
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();
        ForwardBackNow = inputAxis.y;
        SidewaysNow = inputAxis.x;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InstantDeath")
        {
            healthy = 0;
        }
        
    }
}

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
    Camera playerCam;
    public Vector3 respawn;
    public float dist = .5f; //groundDetectionLength
    Ray ray;
    RaycastHit hit;

    public float ForwardBackNow; //vert
    public float SidewaysNow; //horiz

    public float ishowspeed = 10f; //you'll never guess
    public float howMuchBoof = 10f; //jump power

    public int healthy = 5;
    public int maxHealthy = 10;

    public void Start()
    {
        respawn = new Vector3(0, 1, 0);
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        ray = new Ray(transform.position, transform.forward);

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
        Quaternion playerRotation = playerCam.transform.rotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.localRotation = playerRotation;

        //bro move
        Vector3 temp = rb.linearVelocity;

        temp.x = ForwardBackNow * ishowspeed;
        temp.z = SidewaysNow * ishowspeed;

        //bro jump
        ray.origin = transform.position;
        ray.direction = -transform.up;

        rb.linearVelocity = (temp.x * transform.forward) + (temp.y * transform.up) + (temp.z * transform.right);
    }

    public void Move(InputAction.CallbackContext context) //take a wild fucking guess
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();
        ForwardBackNow = inputAxis.y;
        SidewaysNow = inputAxis.x;
    }
    public void Jump() //take another wild fucking guess
    {
        if (Physics.Raycast(ray,dist))
            rb.AddForce(transform.up * howMuchBoof);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InstantDeath")
        {
            healthy = 0;
        }
        
    }
}

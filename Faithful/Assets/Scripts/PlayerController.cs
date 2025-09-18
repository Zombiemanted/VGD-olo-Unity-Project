using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
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
    
    Ray ray;

    public float ForwardBackNow; //vert
    public float SidewaysNow; //horiz

    public float ishowspeed = 10f; //you'll never guess
    public float howMuchBoof = 10f; //jump power
    public float dist = 1f; //groundDetectionLength

    public int healthy = 5;
    public int maxHealthy = 7;

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
        if (healthy <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //take it off
            healthy = 5; //maxhealthy is only achieved via items Dingleberry
        }

        // transform.position = respawn;

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
    public void Jump() //take another wild fucking guess (it also doesnt work)
    {
        if (Physics.Raycast(ray,dist))
            rb.AddForce(transform.up * howMuchBoof, ForceMode.Impulse);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InstantDeath")
        {
            healthy = 0;
        }
        if ((other.tag == "HealthPickup") && (healthy < maxHealthy))
        {
            healthy++;
            //Destroy(other.gameObject);
        }
        if (other.tag == "Hazard")
        {
            healthy--;
            //Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.Tag == "Hazard")
    }
}
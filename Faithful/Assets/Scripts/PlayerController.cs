using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    Camera playerCam;
    Rigidbody rb;
    Ray jumpRay;
    Ray interactRay;
    RaycastHit interactHit;
    GameObject pickupObj;

    public PlayerInput input;
    public Transform weaponSlot;
    public WeaponMain currentWeapon;

    float verticalMove;
    float horizontalMove;

    public float speed = 5f;
    public float jumpHeight = 5f;
    public float groundDetectLength = 1.1f;
    public float interactDistance = 10f;

    public int health = 5;
    public int maxHealth = 7;

    public bool attacking = false;

    public void Start()
    {
        input = GetComponent<PlayerInput>();
        jumpRay = new Ray(transform.position, -transform.up);
        interactRay = new Ray(transform.position, transform.forward);
        rb = GetComponent<Rigidbody>();
        playerCam = Camera.main;
        weaponSlot = transform.GetChild(0);

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (health <= 0)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


        // rotate me
        Quaternion playerRotation = playerCam.transform.rotation;
        playerRotation.x = 0;
        playerRotation.z = 0;
        transform.rotation = playerRotation;

        // move me
        Vector3 temp = rb.linearVelocity;

        temp.x = verticalMove * speed;
        temp.z = horizontalMove * speed;

        jumpRay.origin = transform.position;
        jumpRay.direction = -transform.up;

        interactRay.origin = transform.position;
        interactRay.direction = playerCam.transform.forward;

        if (Physics.Raycast(interactRay, out interactHit, interactDistance))
        {
            if (interactHit.collider.gameObject.tag == "weapon")
            {
                pickupObj = interactHit.collider.gameObject;
            }
        }
        else
            pickupObj = null;

        if (currentWeapon)
            if (currentWeapon.holdToAttack && attacking)
                currentWeapon.fire();

        rb.linearVelocity = (temp.x * transform.forward) +
                            (temp.y * transform.up) +
                            (temp.z * transform.right);
    }
    public void Move(InputAction.CallbackContext context)
    {
        Vector2 inputAxis = context.ReadValue<Vector2>();

        verticalMove = inputAxis.y;
        horizontalMove = inputAxis.x;
    }
    public void Jump()
    {
        if (Physics.Raycast(jumpRay, groundDetectLength))
            rb.AddForce(transform.up * jumpHeight, ForceMode.Impulse);
        Debug.Log("Jumping");

    }
    public void fireModeSwitch()
    {
        if (currentWeapon.weaponID == 1)
        {
            currentWeapon.GetComponent<Sniper>().changeFireMode();
        }
    }
    public void Attack(InputAction.CallbackContext context)
    {
        if (currentWeapon)
        {
            if (currentWeapon.holdToAttack)
            {
                if (context.ReadValueAsButton())
                    attacking = true;
                else
                    attacking = false;
            }

            else
                if (context.ReadValueAsButton())
                currentWeapon.fire();
        }
    }
    public void Reload()
    {
        if (currentWeapon)
            if (!currentWeapon.reloading)
                currentWeapon.reload();
    }
    public void Interact()
    {
        if (pickupObj)
        {
            if (pickupObj.tag == "Weapon")
            {
                if (currentWeapon)
                    DropWeapon();

                pickupObj.GetComponent<WeaponMain>().equip(this);
            }
            pickupObj = null;
        }
        else
            Reload();
    }
    public void DropWeapon()
    {
        if (currentWeapon)
        {
            currentWeapon.GetComponent<WeaponMain>().unequip();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "InstantDeath")
        {
            health = 0;
        }
        if ((other.tag == "HealthPickup") && (health < maxHealth))
        {
            health++;
            //Destroy(other.gameObject);
        }
        if (other.tag == "Hazard")
        {
            health--;
            //Destroy(other.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if (collision.gameObject.Tag == "Hazard")
    }
}
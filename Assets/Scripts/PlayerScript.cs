using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{

    [Header("Movement")]
    public float moveSpeed;
    private float defaultMoveSpeed = 4f;
    public float sprintSpeed;
    public float jumpHeight;
    private bool isSprinting = false;
    public float groundDrag;
    public float airControlMultiplier = 0.4f;
    public bool canMove = true;

    

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;
    public RaycastHit hitInfo;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;
    public Text pressToTalk;
    public DialogSystem dialogSystem;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            isSprinting = !isSprinting;
            Sprint();
        }

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            Jump();
        }

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();

        if (grounded)
        {
            rb.linearDamping = groundDrag;
        }
        else
        {
            rb.linearDamping = 0;
        }

        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward); //create a ray shot out of the camera
        Physics.Raycast(ray, out hitInfo, 10f); //shoot out ray and store whatever it hit in hitInfo

        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * 10, Color.red); //show ray

        if (hitInfo.collider != null && hitInfo.collider.CompareTag("Door"))
        {
            pressToTalk.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                hitInfo.collider.GetComponent<DoorScript>().Open(); //get the doorScript from the collided door, then run the Open() function
            }
        }
        else
        {
            if (!dialogSystem.canTalk && !dialogSystem.isTalking)
            {
                pressToTalk.gameObject.SetActive(false);
            }
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        if (canMove)
        {
            moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

            if (grounded)
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
            }
            else
            {
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airControlMultiplier, ForceMode.Force);
            }
        }

    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

    private void Sprint()
    {
        if (isSprinting)
        {
            moveSpeed = sprintSpeed;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }
    }

    private void Jump()
    {
        rb.AddForce(new Vector3(0, jumpHeight, 0));
    }


    //To-do
    /* neighbor door system
     * leaning (should be easy with Cinemachine)
     * door peaking (maybe)
     * flashlight (should be easy)
     * 
     * create a seperate script that runs through dialog. Can be added onto other objects as a secondary script. Can be called from other scripts
     */
}


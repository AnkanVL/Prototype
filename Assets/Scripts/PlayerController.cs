using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;
    public float runSpeed = 9f;
    public float maxStamina = 100f; //so sprint dont last forever
    public float staminaDrainRate = 25f;
    public float staminaRegenRate = 10f;
    private float currentStamina;
    public bool isWalking = false;

    [Header("Jumpscare Integration")]
    private bool isFrozen = false;     // Stops all input during monster catch

    [Header("Look Settings")]
    public float mouseSensitivity = 100f;
    public float controllerSensitivity = 100f;
    public Transform cameraTransform;
    private float xRotation = 0f;

    private CharacterController controller;
    private Vector3 velocity;

    [Header("Physics")]
    public float gravity = -15f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        Cursor.lockState = CursorLockMode.Locked;
        if (cameraTransform == null)
            cameraTransform = GetComponentInChildren<Camera>().transform;

        //Auto-register with CheckpointManager if it exist
        if (CheckpointManager.Instance != null)
            CheckpointManager.Instance.player = transform;
    }
     void Update()
    {
        if (isFrozen) return;

        HandleRotation();
        HandleMovement();
    }

     void HandleRotation()
    {
        //Mouse + Controller support (add "RightStickHorizontal" and "RightStickVertical" in Input Manager if using gamepad)
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        float controllerX = Input.GetAxis("RightStickHorizontal") * controllerSensitivity * Time.deltaTime;
        float controllerY = Input.GetAxis("RightStickVertical") * controllerSensitivity * Time.deltaTime;

        xRotation -= (mouseY + controllerY);
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * (mouseX + controllerX));
    }
    
     void HandleMovement()
    {
        // Stamina System (Drain When Sprinting)
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) || Input.GetButton("Fire3");
        bool canRun = wantsToRun && currentStamina > 0f;

        float currentSpeed = canRun ? runSpeed : walkSpeed;

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * currentSpeed *  Time.deltaTime);

        //Updating Stamina
        if (canRun && move.magnitude > 0.1f)
            currentStamina -= staminaDrainRate * Time.deltaTime;
        else if (currentStamina < maxStamina)
            currentStamina += staminaRegenRate * Time.deltaTime;

        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);

        // Update walking flag (useful for footstep sound and Animation)
        isWalking = move.magnitude > 0.1f && !canRun;

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

    }
    // ====================== PUBLIC METHODS FOR HORROR ======================

    /// <summary>
    /// Called automatically from CheckpointManager during jumpscare
    /// </summary> 
    public void FreezzePlayerForJumpscare()
    {
        isFrozen = true;
        velocity = Vector3.zero;
        controller.Move(Vector3.zero); // Stop any residual movement
        Debug.Log("<color=red>Player frozen for jumpscare</color>");
    }
    /// <summary>
    /// Called automatically after respawn
    /// </summary>
    
    public void UnfreezePlayer()
    {
        isFrozen = false;
        Debug.Log("<color=green>Player unfrozen - back in the nightmare</color>");
    }
    // Optional: Quick stamina reset on checkpoint (feels fair)
    public void ResetStamina()
    {
        currentStamina = maxStamina;
    }

}

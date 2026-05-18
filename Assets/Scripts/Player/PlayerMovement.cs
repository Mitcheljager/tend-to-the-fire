using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    [Header("Components")]
    public CharacterController controller;
    public PlayerState playerState;
    public PlayerCamera playerCamera;
    public PlayerStamina playerStamina;
    [Header("Movement")]
    public float baseSpeed = 2f;
    public float runSpeed = 4f;
    public bool isRunning = false;
    public float staminaRunningCutoff = 0.25f;
    [Header("Gravity")]
    public Vector3 gravityDirection = new(0f, -1f, 0f);
    public float gravity = 10f;
    public float maxVelocity = 120f;
    public Transform groundCheck;
    public float groundDistance = 0.25f;
    public LayerMask groundMask;
    [Header("State")]
    public float currentSpeed = 0f;
    public Vector3 move = Vector3.zero;
    public Vector3 velocity = Vector3.zero;
    public float inputX = 0f;
    public float inputZ = 0f;
    public bool isGrounded = false;

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && IsGravityPositive()) velocity = gravityDirection * 10;

        if (playerState.isDead) return;

        SetMovement();
        SetRunning();
        SetSpeed();
    }

    private void SetMovement() {
        SetMovementValues();
        controller.Move(move * currentSpeed * Time.deltaTime);

        SetVelocityValues();
        controller.Move(velocity * Time.deltaTime);
    }

    private void SetRunning() {
        if (!Input.GetKey(KeyCode.LeftShift) || playerStamina.isExhausted) {
          isRunning = false;
          return;
        }

        isRunning = true;
    }

    private void SetSpeed() {
        if (move.magnitude == 0f) {
            currentSpeed = 0f;
            return;
        }

        currentSpeed = isRunning ? runSpeed : baseSpeed;
    }

    private void SetMovementValues() {
        inputX = isGrounded ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        inputZ = isGrounded ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        move = transform.right * inputX + transform.forward * inputZ;

        if (move.magnitude > 1) move.Normalize();
    }

    private void SetVelocityValues() {
        velocity += gravityDirection * gravity * Time.deltaTime;

        float velocityMagnitude = velocity.magnitude;
        if (velocityMagnitude > maxVelocity) velocity = velocity.normalized * maxVelocity;
    }

    public bool IsGravityPositive() {
        return (velocity.x + velocity.y + velocity.z) * (gravityDirection.x + gravityDirection.y + gravityDirection.z) > 0;
    }

    public void SetPosition(Vector3 position) {
        controller.Move(position - transform.position);
    }
}

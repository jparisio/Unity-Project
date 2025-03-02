using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;

    [Header("References")]
    [SerializeField] public CharacterController characterController;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform cam; // Assign Cinemachine Virtual Camera Transform

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("States")]
    public LocomotionState locomotionState;
    public FishCastState fishCastState;
    public IdleState idleState;

    private void Awake()
    {
        InitializeStateMachine();
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void InitializeStateMachine()
    {
        stateMachine = new StateMachine<PlayerController>();

        idleState = new IdleState(this);
        locomotionState = new LocomotionState(this);
        fishCastState = new FishCastState(this);

        stateMachine.Initialize(locomotionState);
    }

    public void HandleMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 movementInput = new Vector3(horizontal, 0, vertical);

        if (movementInput.magnitude > 0.1f) // Prevents small jittery input
        {
            Vector3 moveDirection = GetCameraRelativeDirection(movementInput);
            characterController.Move(moveDirection * moveSpeed * Time.deltaTime);

            // Rotate only when moving forward
            if (vertical > 0.1f)
            {
                RotateTowardsCameraForward();
            }
        }

        // Pass movement input to Animator
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.z);
    }

    private Vector3 GetCameraRelativeDirection(Vector3 inputDirection)
    {
        // Get camera forward (ignoring tilt)
        Vector3 cameraForward = cam.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Vector3 cameraRight = cam.right;
        cameraRight.y = 0;
        cameraRight.Normalize();

        // Convert input direction to world-space movement
        return (cameraForward * inputDirection.z + cameraRight * inputDirection.x).normalized;
    }

    private void RotateTowardsCameraForward()
    {
        Vector3 cameraForward = cam.forward;
        cameraForward.y = 0; // Keep rotation horizontal
        Quaternion targetRotation = Quaternion.LookRotation(cameraForward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

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
            RotatePlayer(moveDirection);
            
        }

        // Pass movement input to Animator
        animator.SetFloat("MoveX", movementInput.x);
        animator.SetFloat("MoveY", movementInput.z);
    }

    private Vector3 GetCameraRelativeDirection(Vector3 direction)
    {
        Vector3 forward = cam.forward;
        Vector3 right = cam.right;

        forward.y = 0f;
        right.y = 0f;

        forward.Normalize();
        right.Normalize();

        return forward * direction.z + right * direction.x;
    }

    private void RotatePlayer(Vector3 direction)
    {
        if (direction.magnitude > 0.1f)
        {
            // Smoothly rotate towards movement direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

}

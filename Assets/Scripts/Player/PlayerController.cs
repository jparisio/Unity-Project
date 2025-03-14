using System.Collections;
using MoreMountains.Feedbacks;
using Unity.Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;

    [Header("References")]
    [SerializeField] public CharacterController characterController;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform cam; 
    public Transform cutPlane;
    public CinemachineCamera normalCam;
    public CinemachineCamera zoomedCam;
    public Material fishTexture;
    public ParticleSystem slashParticles;
    public MMF_Player feedbacks;
    public MMFeedbacks slashFeedbacks;
    public CinemachineInputAxisController inputAxisController;

    [Header("Movement")]
    [SerializeField] public float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    public Vector3 previousMoveDirection;
    private Vector3 gravity = Vector3.down * 5f;

    [Header("States")]
    public LocomotionState locomotionState;
    public FishCastState fishCastState;
    public IdleState idleState;
    public SliceState sliceState;

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
        sliceState = new SliceState(this);

        stateMachine.Initialize(idleState);
    }

    public void HandleMovement()
    {

        Vector3 movementInput = GetInputVector();

        if (movementInput.magnitude > 0.1f)
        {
            Vector3 moveDirection = GetCameraRelativeDirection(movementInput);

            gravity = characterController.isGrounded ? Vector3.zero : Vector3.down * 5f;

            characterController.Move((moveDirection * moveSpeed + gravity) * Time.deltaTime);
            RotatePlayer(moveDirection);
            previousMoveDirection = moveDirection;
            
        }

        //add gravity 
        // if(characterController.isGrounded == false)
        // {
        //     characterController.Move(Vector3.down * 9.8f * Time.deltaTime);
        // }
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

    public Vector3 GetInputVector()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            return new Vector3(horizontal, 0, vertical);
        }


}

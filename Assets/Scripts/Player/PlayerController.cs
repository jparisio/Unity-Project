using System;
using UnityEditor.Animations;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;

    [Header("References")]
    [SerializeField] public CharacterController characterController;
    [SerializeField] public Animator animator;
    [SerializeField] private Transform cam;


    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;

    [Header("States")]
    public LocomotionState locomotionState;
    public FishCastState fishCastState;
    public IdleState idleState;

    private void Awake()
    {
        InitalizeStateMachine();
    }

    private void Start()
    {

    }

    private void Update()
    {
        stateMachine.Update();

    }


    private void InitalizeStateMachine(){
        stateMachine = new StateMachine<PlayerController>();

        idleState = new IdleState(this);
        locomotionState = new LocomotionState(this);
        fishCastState = new FishCastState(this);

        // Set initial state to idle state
        stateMachine.Initialize(locomotionState);
    }

    public void HandleMovement(){
        // Get the input from the input system
        Vector3 movementInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        // Move the player
        characterController.Move(movementInput * Time.deltaTime * moveSpeed);
    }

    public void HandleRotation(){
        //move in the diretion of the camera
    }

}

using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public StateMachine<PlayerController> stateMachine;
    // public Rigidbody rb;
    // public Animator playerAnimator;
    public IdleState idleState;
    public RunState runState;
    public JumpState jumpState;


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
        runState = new RunState(this);
        jumpState = new JumpState(this);

        // Set initial state to idle state
        stateMachine.Initialize(idleState);
    }

}

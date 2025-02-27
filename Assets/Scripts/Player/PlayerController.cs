using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class PlayerController : MonoBehaviour
{
    public StateMachine stateMachine;
    public Rigidbody rb;
    public Animator playerAnimator;


    private void Awake()
    {

    }
    private void Start()
    {
        stateMachine = new StateMachine(this);

        // Set initial state to idle state
        stateMachine.Initialize(stateMachine.idleState);
    }

    private void Update()
    {
        stateMachine.Update();

    }

}

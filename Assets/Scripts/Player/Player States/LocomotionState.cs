using UnityEngine;

public class LocomotionState : IState
{

private PlayerController player;

    public LocomotionState(PlayerController player){
        this.player = player;
        
    }

    public void Enter()
    {
        Debug.Log("Entering locomotion");
        player.animator.SetBool("isMoving", true);
    }

    public void Update()
    {

        player.HandleMovement();
        
        //handle anim blending for the running state
        HandleAnims();

        // Transition to Idle State when no input and no speed
        if(player.GetInputVector() == Vector3.zero && player.characterController.velocity.magnitude < 0.1f){
            player.stateMachine.ChangeState(player.idleState);
        }
    }


    public void Exit()
    {
        Debug.Log("Exiting Locomotion State");
        player.animator.SetBool("isMoving", false);
    }


    private void HandleAnims()
    {
       Vector3 localVelocity = player.transform.InverseTransformDirection(player.characterController.velocity);

        // Clamp the values to be within -1 and 1
        float clampedX = Mathf.Clamp(localVelocity.x, -1f, 1f);
        float clampedY = Mathf.Clamp(localVelocity.z, -1f, 1f);

        player.animator.SetFloat("X", clampedX, 0.2f, Time.deltaTime);
        player.animator.SetFloat("Y", clampedY, 0.2f, Time.deltaTime);

    }
}

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
        player.animator.SetBool("isWalking", true);
    }

    public void Update()
    {
        player.HandleMovement();
        
        Vector3 localVelocity = player.transform.InverseTransformDirection(player.characterController.velocity);

        player.animator.SetFloat("X", localVelocity.x, 0.2f, Time.deltaTime); // Left (-) / Right (+)
        player.animator.SetFloat("Y", localVelocity.z, 0.2f, Time.deltaTime); // Forward (+) / Backward (-)


        if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0){
            player.stateMachine.ChangeState(player.idleState);
        }

    }

    public void Exit()
    {
        Debug.Log("Exiting Locomotion State");
        player.animator.SetBool("isWalking", false);
    }
}

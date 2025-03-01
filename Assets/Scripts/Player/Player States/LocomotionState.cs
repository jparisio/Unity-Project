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
        
        player.animator.SetFloat("X", player.characterController.velocity.x);
        player.animator.SetFloat("Y", player.characterController.velocity.z);

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

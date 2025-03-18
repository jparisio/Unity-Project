using UnityEngine;

public class IdleState : IState
{

private PlayerController player;

    public IdleState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle");
    }

    public void Update()
    {
        player.HandleMovement();
        
        if(player.GetInputVector() != Vector3.zero){
            player.stateMachine.ChangeState(player.locomotionState);
        }

        if(Input.GetKey("f")){
            player.stateMachine.ChangeState(player.sliceState);
        }

        if (Input.GetMouseButtonDown(0)){
            player.stateMachine.ChangeState(player.fishWindState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

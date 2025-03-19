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
            return;
        }

        if(Input.GetKey("f")){
            player.stateMachine.ChangeState(player.sliceState);
            return;
        }

        if (Input.GetMouseButtonDown(0)){
            player.stateMachine.ChangeState(player.fishWindState);
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

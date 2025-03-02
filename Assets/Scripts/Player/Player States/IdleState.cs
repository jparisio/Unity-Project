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
        if(player.GetInputVector() != Vector3.zero){
            player.stateMachine.ChangeState(player.locomotionState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

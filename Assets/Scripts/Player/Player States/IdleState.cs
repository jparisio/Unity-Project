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
        if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0){
            player.stateMachine.ChangeState(player.locomotionState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

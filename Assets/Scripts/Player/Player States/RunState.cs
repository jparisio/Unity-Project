using UnityEngine;
public class RunState : IState
{

    private PlayerController player;

    public RunState(PlayerController player){
        this.player = player;
        
    }

    public void Enter()
    {
        Debug.Log("Entering Run State");
    }

    public void Update()
    {
        if (Input.GetKey("s")){
            player.stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKey("j")){
            player.stateMachine.ChangeState(player.jumpState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Run State");
    }
}
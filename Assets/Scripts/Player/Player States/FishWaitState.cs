using UnityEngine;

public class FishWaitState : IState
{
    private PlayerController player;

    public FishWaitState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering fish wait");
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.stateMachine.ChangeState(player.idleState);
            return;
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State");
    }
}
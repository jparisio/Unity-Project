using UnityEngine;

public class FishCastState : IState
{
    //cast the fishning rod
    private PlayerController player;

    public FishCastState(PlayerController player){
        this.player = player;
        
    }

    public void Enter()
    {
        Debug.Log("Entering fish cast");

        player.animator.SetBool("isFishing", true);
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State");
        player.animator.SetBool("isFishing", false);
    }
}

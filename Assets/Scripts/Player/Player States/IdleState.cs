using Unity.VisualScripting.FullSerializer;
using UnityEngine;
public class IdleState : IState
{
    private PlayerController player;

    public IdleState(PlayerController player){
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entering Idle State");
    }

    public void Update()
    {
        if (Input.GetKey("w")){
            player.stateMachine.ChangeState(player.runState);
        }

        //  if (Input.GetKey("s")){
        //     player.playerAnimator.SetBool("walking", false);
        // }
        
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

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
    }

    public void Update()
    {

    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State");
    }
}

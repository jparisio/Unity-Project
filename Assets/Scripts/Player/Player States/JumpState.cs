using UnityEngine;
public class JumpState : IState
{
    private PlayerController player;
    public JumpState(PlayerController player){
        this.player = player;
    }
    public void Enter()
    {
        Debug.Log("Entering jump State");
    }

    public void Update()
    {
        // Debug.Log("Executing Idle State");
        // Add Idle logic here
        // if()
    }

    public void Exit()
    {
        Debug.Log("Exiting jump State");
    }
}

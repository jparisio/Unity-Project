using System;
using UnityEngine;

public class IdleState : IState
{

private PlayerController player;
private float buffer = 0f;

    public IdleState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle");
        if(player.stateMachine.PreviousState == player.sliceState){
            buffer = 1f;
        } else {
            buffer = 0f;
        }
    }

    public void Update()
    {
        player.HandleMovement();
        
        if(player.GetInputVector() != Vector3.zero){
            player.stateMachine.ChangeState(player.locomotionState);
            return;
        }

        // if(Input.GetKey("f")){
        //     player.stateMachine.ChangeState(player.sliceState);
        //     return;
        // }

        if (Input.GetMouseButtonDown(0) && buffer <= 0f){
            player.stateMachine.ChangeState(player.fishWindState);
            return;
        }

        if(buffer > 0f){
            buffer -= Time.deltaTime;
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting Idle State");
    }
}

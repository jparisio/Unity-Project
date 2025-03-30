using System;
using UnityEngine;

public class IdleState : IState
{

private PlayerController player;
private float buffer = 0f;


private float successLockTimer = 0f;
private bool isInSuccessAnimation;


    public IdleState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle");
        if(player.stateMachine.PreviousState == player.sliceState)
        {
            SoundManager.PlaySound(SoundType.SUCCESS, 0.5f);
            player.animator.SetTrigger("Success");
            successLockTimer = player.successAnimationLength;
            isInSuccessAnimation = true;
            
            // Immediately freeze movement
            player.characterController.enabled = false;
        }



        if(player.stateMachine.PreviousState == player.sliceState){
            buffer = 1f;
        } else {
            buffer = 0f;
        }
    }

    public void Update()
    {
        if(isInSuccessAnimation)
        {
            successLockTimer -= Time.deltaTime;
            
            // Keep character controller disabled during animation
            if(!player.characterController.enabled)
            {
                player.characterController.enabled = true;
                player.characterController.enabled = false;
            }

            if(successLockTimer <= 0)
            {
                isInSuccessAnimation = false;
                player.characterController.enabled = true;
            }
            return;
        }

        player.HandleMovement();
        
        if(player.GetInputVector() != Vector3.zero){
            player.stateMachine.ChangeState(player.locomotionState);
            return;
        }

        // if(Input.GetKey("f")){
        //     player.stateMachine.ChangeState(player.sliceState);
        //     return;
        // }

        if (Input.GetMouseButtonDown(0) && buffer <= 0f && player.isInFishArea){
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

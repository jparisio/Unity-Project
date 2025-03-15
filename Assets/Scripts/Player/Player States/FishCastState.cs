using UnityEngine;

public class FishCastState : IState
{
    //cast the fishning rod
    private PlayerController player;
    private float animationDuration;
    private float elapsedTime;


    public FishCastState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering fish cast");

        player.animator.SetBool("isFishing", true);

        AnimationClip castClip = player.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        animationDuration = castClip.length;
        elapsedTime = 0f;
    }
    public void Update()
    {
        if (Input.GetMouseButtonDown(0)){
            player.stateMachine.ChangeState(player.idleState);
            return;
        }

        elapsedTime += Time.deltaTime;
        if (elapsedTime >= animationDuration)
        {
            player.stateMachine.ChangeState(player.fishWaitState);
        }

    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State");
        player.animator.SetBool("isFishing", false);
    }
}

using UnityEngine;

public class FishReelState : IState
{
    private PlayerController player;
    private float animationDuration;
    private float elapsedTime;

    public FishReelState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering reel state");
        player.animator.SetBool("isReeling", true);

        AnimationClip castClip = player.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        animationDuration = castClip.length;
        elapsedTime = 0f;
    }

    public void Update()
    {
        // Handle mouse input and transition to idle state immediately
        if (Input.GetMouseButtonDown(0))
        {
            player.stateMachine.ChangeState(player.idleState);
            return;
        }

        // Count elapsed time for the reel animation
        elapsedTime += Time.deltaTime;

        // When the animation is finished, transition to idle state
        if (elapsedTime >= animationDuration)
        {
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish reeling State");
        player.animator.SetBool("isWinding", false);
        player.animator.SetBool("isFishing", false);
        player.animator.SetBool("isCasting", false);
        player.animator.SetBool("isReeling", false);
    }
}
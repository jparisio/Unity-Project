using UnityEngine;
using System.Collections;

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
        ReelLine();
    }

    public void Update()
    {
        if (player.fishBob == null)
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

    private void ReelLine()
    {
        if (player.fishBob == null) return;

        Rigidbody rb = player.fishBob.GetComponent<Rigidbody>();
        
        if (rb != null)
        {
            player.StartCoroutine(MoveFishBobToPlayer(rb));
        }
    }

    // Coroutine to move fishBob smoothly
    private IEnumerator MoveFishBobToPlayer(Rigidbody fishBobRb)
    {
        yield return new WaitForSeconds(1f);
        float speed = 20f; 
        Vector3 targetPosition = player.startLine.position;

        while (Vector3.Distance(fishBobRb.position, targetPosition) > 0.1f)
        {
            fishBobRb.position = Vector3.MoveTowards(fishBobRb.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        GameObject.Destroy(fishBobRb.gameObject); 
    }

}
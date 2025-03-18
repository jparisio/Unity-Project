using UnityEngine;
using System.Collections;

public class FishWaitState : IState
{
    private PlayerController player;
    private Coroutine waitCoroutine;

    public FishWaitState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering fish wait");
        player.animator.SetBool("isFishing", true);

        float waitTime = Random.Range(3f, 13f); // Random wait time between 3 and 13 seconds
        waitCoroutine = player.StartCoroutine(WaitForCatch(waitTime));
    }

    private IEnumerator WaitForCatch(float waitTime)
    {
        // Wait for the random amount of time
        yield return new WaitForSeconds(waitTime);

        // After waiting, change to the fishReelState
        player.stateMachine.ChangeState(player.fishReelState);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (waitCoroutine != null)
            {
                player.StopCoroutine(waitCoroutine); // Stop the waiting coroutine if it's still running
            }
            player.stateMachine.ChangeState(player.idleState); // Transition to idle state
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State");
        player.animator.SetBool("isWinding", false);
        player.animator.SetBool("isFishing", false);
        player.animator.SetBool("isCasting", false);

        if (waitCoroutine != null)
        {
            player.StopCoroutine(waitCoroutine);
        }
    }
}
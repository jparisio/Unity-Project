using System.Collections;
using UnityEngine;

public class SuccessState : IState
{
    private PlayerController player;
    private Coroutine animationCoroutine;

    public SuccessState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering Success State");
        // Reset slicing animation first
        player.animator.SetBool("isSlicing", false);
        // Trigger success animation
        player.animator.SetTrigger("Success");
        
        // Start animation tracking
        animationCoroutine = player.StartCoroutine(WaitForAnimationCompletion());
    }

    public void Update()
    {
        // No update logic needed for success state
    }

    private IEnumerator WaitForAnimationCompletion()
    {
        // Wait for animation transition
        yield return new WaitForSeconds(0.1f);
        
        // Wait for animation length
        float animationLength = player.animator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(animationLength);

        // Transition to idle
        player.stateMachine.ChangeState(player.idleState);
    }

    public void Exit()
    {
        Debug.Log("Exiting Success State");
        if (animationCoroutine != null)
        {
            player.StopCoroutine(animationCoroutine);
        }
    }
}

using System.Collections;
using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

public class FishCastState : IState
{
    //cast the fishning rod
    private PlayerController player;
    private float animationDuration;
    private float elapsedTime;
    private Rigidbody fishBobRb;


    public FishCastState(PlayerController player){
        this.player = player;
    }

    public void Enter()
    {
        Debug.Log("Entering fish cast");

        player.animator.SetBool("isCasting", true);

        AnimationClip castClip = player.animator.GetCurrentAnimatorClipInfo(0)[0].clip;
        animationDuration = castClip.length;
        elapsedTime = 0f;

        // Instantiate a ball that's an rb and add a force to it (impulse) player local forward
        player.StartCoroutine(castBob());
    }
    public void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= animationDuration)
        {
            player.stateMachine.ChangeState(player.fishWaitState);
        }

    }

    public void Exit()
    {
        Debug.Log("Exiting fish cast State"); 
    }

    private IEnumerator castBob()
    {
        yield return new WaitForSeconds(.3f);
        player.fishBob = GameObject.Instantiate(player.fishBobPrefab, player.transform.position, player.transform.rotation);
        fishBobRb = player.fishBob.GetComponent<Rigidbody>();
        fishBobRb.AddForce((player.transform.forward + player.transform.up) * player.charge, ForceMode.Impulse); 
        Transform targetTransform = player.transform.Find("CamTarget");
        if (targetTransform != null)
        {
            player.fishBob.GetComponent<Rope>().SetEndPoint(targetTransform);
        }
        else
        {
            Debug.LogWarning("Empty object with the specified name not found in children.");
        }
    }
}

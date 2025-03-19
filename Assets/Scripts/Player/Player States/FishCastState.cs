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
    private Rope rope;


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
        yield return new WaitForSeconds(.6f);
        player.fishBob = GameObject.Instantiate(player.fishBobPrefab, player.startLine.position, player.fishingRod.transform.rotation);
        fishBobRb = player.fishBob.GetComponent<Rigidbody>();
        fishBobRb.AddForce((player.transform.forward + player.transform.up) * player.charge, ForceMode.Impulse); 
        player.StartCoroutine(createLine());
    }

    private IEnumerator createLine(){
      yield return new WaitForSeconds(.07f);
       Transform targetTransform = player.startLine;
       if (targetTransform != null)
        {
            rope = player.fishBob.GetComponent<Rope>();
            rope.SetEndPoint(targetTransform);
        }
        else
        {
            Debug.Log("Empty object with the specified name not found in children.");
        }
    }
}

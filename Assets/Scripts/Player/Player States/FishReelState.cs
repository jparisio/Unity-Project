using UnityEngine;
using System.Collections;

public class FishReelState : IState
{
    private PlayerController player;
    private float animationDuration;
    private float elapsedTime;

    private const float reelDuration = 10f;

    private FishingReelMinigameUI reelMinigame;

    public FishReelState(PlayerController player){
        this.player = player;

        reelMinigame = player.reelMinigameUI.GetComponent<FishingReelMinigameUI>();
    }

    public void Enter()
    {
        Debug.Log("Entering reel state");
        player.animator.SetBool("isReeling", true);

        player.reelMinigameUI.SetActive(true);

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
            SpawnFish();
            
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish reeling State");
        player.animator.SetBool("isWinding", false);
        player.animator.SetBool("isFishing", false);
        player.animator.SetBool("isCasting", false);
        player.animator.SetBool("isReeling", false);

        player.reelMinigameUI.SetActive(false);
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
        float speed = 5f; 
        Vector3 targetPosition = player.startLine.position;

        while (Vector3.Distance(fishBobRb.position, targetPosition) > 0.1f)
        {
            fishBobRb.position = Vector3.MoveTowards(fishBobRb.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        GameObject.Destroy(fishBobRb.gameObject); 
    }

private void SpawnFish()
{
    Debug.Log("Fish successfully caught!");
    
    // Get a random fish from the database
    FishData randomFish = player.fishDatabase.GetRandomFish();

    // Set the spawn position (near the player)
    Vector3 spawnPosition = player.transform.position + new Vector3(2f, 0f, 2f);  // Spawn 2 units offset from the player

    // Instantiate the fish prefab near the player using UnityEngine.Object.Instantiate
    UnityEngine.Object.Instantiate(randomFish.fishPrefab, spawnPosition, Quaternion.identity);

    // Debug log to show which fish was spawned
    Debug.Log($"Spawned: {randomFish.fishName} (Rarity: {randomFish.rarity}, Value: {randomFish.value})");
}



}
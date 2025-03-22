using UnityEngine;
using System.Collections;
using GogoGaga.OptimizedRopesAndCables;

public class FishReelState : IState
{
    private PlayerController player;
    private FishingReelMinigameUI reelMinigame;
    private GameObject fishInstance;

    public FishReelState(PlayerController player)
    {
        this.player = player;
        reelMinigame = player.reelMinigameUI.GetComponent<FishingReelMinigameUI>();
    }

    public void Enter()
    {
        Debug.Log("Entering reeling state");

        player.animator.SetBool("isReeling", true);

        reelMinigame.gameObject.SetActive(true);
      

        // Pull a random fish from the fish database
        FishData randomFish = player.fishDatabase.GetRandomFish();
        Debug.Log($"Caught {randomFish.fishName} (Rarity: {randomFish.rarity}, Value: {randomFish.value})");
        reelMinigame.setMoveSpeed(randomFish.value);

        if (player.fishBob != null)
        {
            // Instantiate without parenting first to get original scale
            fishInstance = Object.Instantiate(
                randomFish.fishPrefab,
                player.fishBob.transform.position,
                Quaternion.identity
            );

            // Store original scale before parenting
            Vector3 originalScale = fishInstance.transform.localScale;

            // Parent to bobber and reset local position/rotation
            fishInstance.transform.SetParent(player.fishBob.transform, true);
            fishInstance.transform.localPosition = Vector3.zero;
            fishInstance.transform.localRotation = Quaternion.identity;

            // Counteract parent scale to maintain original size
            fishInstance.transform.localScale = new Vector3(
                originalScale.x / player.fishBob.transform.localScale.x,
                originalScale.y / player.fishBob.transform.localScale.y,
                originalScale.z / player.fishBob.transform.localScale.z
            );

            fishInstance.transform.rotation = Quaternion.identity;
            fishInstance.SetActive(false);

            Vector3 directionToPlayer = player.transform.position - fishInstance.transform.position;
            directionToPlayer.y = 0;  // keep the rotation only on the y-axis
            if(directionToPlayer != Vector3.zero)
            {
                fishInstance.transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }

            Debug.Log($"Caught {randomFish.fishName} (Rarity: {randomFish.rarity}, Value: {randomFish.value})");
        }

        ReelLine();
    }

    public void Update()
    {
        if (player.fishBob == null)
        {
            if(reelMinigame.minigameFailed)
            {
                player.stateMachine.ChangeState(player.idleState);
                GameObject.Destroy(fishInstance);
                return;
            } else {
                player.stateMachine.ChangeState(player.sliceState);
                return;
            }
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish reeling State");
        player.animator.SetBool("isReeling", false);

        reelMinigame.gameObject.SetActive(false);
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
        float speed = 7f;
        Vector3 playerBottom = player.transform.position - new Vector3(0, player.characterController.height / 2f, 0);
        Vector3 targetPosition = playerBottom + player.transform.forward * 3f;
        player.fishBob.GetComponent<RopeWindEffect>().windForce = 0f;

        while (Vector3.Distance(fishBobRb.position, targetPosition) > 0.1f)
        {
            fishBobRb.position = Vector3.MoveTowards(fishBobRb.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }   

        PopFishUp();
    }

    private void PopFishUp()
    {
        fishInstance.SetActive(true);
        fishInstance.transform.SetParent(null);

        // Convert SkinnedMeshRenderer to a static mesh
        SkinnedMeshRenderer skinnedRenderer = fishInstance.GetComponentInChildren<SkinnedMeshRenderer>();
        if (skinnedRenderer != null)
        {
            Debug.Log("Baking Skinned Mesh before applying physics");

            Mesh bakedMesh = new Mesh();
            skinnedRenderer.BakeMesh(bakedMesh); // Bake the mesh

            // Create a new GameObject for the baked fish
            GameObject bakedFish = new GameObject("BakedFish");
            bakedFish.transform.position = fishInstance.transform.position;
            bakedFish.transform.rotation = fishInstance.transform.rotation;
            bakedFish.transform.localScale = fishInstance.transform.lossyScale;

            // Add MeshFilter & MeshRenderer
            MeshFilter mf = bakedFish.AddComponent<MeshFilter>();
            mf.mesh = bakedMesh;

            MeshRenderer mr = bakedFish.AddComponent<MeshRenderer>();
            mr.materials = skinnedRenderer.sharedMaterials;

            // Set correct layer for slicing
            bakedFish.layer = LayerMask.NameToLayer("Sliceable");

            // Destroy the skinned mesh object
            GameObject.Destroy(fishInstance);
            fishInstance = bakedFish; // Replace reference with baked fish
            Debug.Log($"Fish Layer: {LayerMask.LayerToName(fishInstance.layer)}");

        }

        // Add Collider
        AddFishCollider();

        // Add Rigidbody
        Rigidbody rb = fishInstance.AddComponent<Rigidbody>();
        rb.excludeLayers = LayerMask.GetMask("Water");

        // Apply physics
        rb.AddForce(Vector3.up * 7f, ForceMode.Impulse);
        rb.AddTorque(Vector3.up * 6f, ForceMode.Impulse);

        // Destroy the bobber
        GameObject.Destroy(player.fishBob.gameObject);

        Debug.Log(fishInstance.GetComponent<MeshFilter>()?.mesh);
    }

    private void AddFishCollider()
    {
        fishInstance.AddComponent<BoxCollider>();
        //size it to the fishes scale
        fishInstance.GetComponent<BoxCollider>().size = fishInstance.transform.localScale;
        fishInstance.GetComponent<BoxCollider>().isTrigger = true;
    }

    // private void SpawnFish()
    // {
    //     Debug.Log("Fish successfully caught!");

    //     // Get a random fish from the database
    //     FishData randomFish = player.fishDatabase.GetRandomFish();

    //     // Set the spawn position (near the player)
    //     Vector3 spawnPosition = player.transform.position + new Vector3(2f, 0f, 2f);  // Spawn 2 units offset from the player

    //     // Instantiate the fish prefab near the player using UnityEngine.Object.Instantiate
    //     UnityEngine.Object.Instantiate(randomFish.fishPrefab, spawnPosition, Quaternion.identity);

    //     // Debug log to show which fish was spawned
    //     Debug.Log($"Spawned: {randomFish.fishName} (Rarity: {randomFish.rarity}, Value: {randomFish.value})");
    // }


}
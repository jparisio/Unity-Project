using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;

public class FishLineController : MonoBehaviour
{
    private Rope rope;
    private PlayerController player;
    private Rigidbody rb;

    void Start()
    {
        rope = GetComponent<Rope>();
        player = GameObject.FindFirstObjectByType<PlayerController>();
        rb = player.fishBob.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //this should be moved into a controller class
        if (rope != null)
        {
            float distance = Vector3.Distance(player.startLine.position, player.fishBob.transform.position); 
            rope.ropeLength = Mathf.Max(1, Mathf.RoundToInt(distance / 2)); 
            // Debug.Log("Distance: " + distance);
            
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"Collided with: {collision.gameObject.name}");
        //i set the ground to water tag for now for testing
        if (collision.gameObject.CompareTag("Water"))
        {
            rb.isKinematic = true;
        }
    }
}

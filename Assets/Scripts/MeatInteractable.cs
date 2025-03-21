using Unity.VisualScripting;
using UnityEngine;


public class MeatInteractable : MonoBehaviour, IInteractable
{

    private float minSize = 0.01f;

    public void Interact()
    {
        Debug.Log("Meat Interacted");
        //destroy the object attatched to this script 
        Destroy(gameObject);
    }

    void Start()
    {
        
    }

    void Update()
    {
        float volume = GetBoundingBoxVolume(gameObject);
        if (volume < minSize)
        {
            Destroy(gameObject);
        }

        if(transform.position.y < -1000)
        {
            Destroy(gameObject);
        }
    }


    private float GetBoundingBoxVolume(GameObject obj)
    {
        // Get the mesh renderer of the object
        MeshRenderer meshRenderer = obj.GetComponent<MeshRenderer>();

        // Get the bounds of the mesh renderer
        Bounds bounds = meshRenderer.bounds;

        // Calculate the volume of the bounding box
        float volume = bounds.size.x * bounds.size.y * bounds.size.z;

        //this is like if its a stupid skinny peiece just delete it
        if(bounds.size.x < minSize/5 || bounds.size.y < minSize/5 || bounds.size.z < minSize/5)
        {
            return 0;
        }

        return volume;
    }
}

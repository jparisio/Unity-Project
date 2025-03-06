using UnityEngine;


public class MeatInteractable : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        Debug.Log("Meat Interacted");
        //destroy the object attatched to this script 
        Destroy(gameObject);
    }
}

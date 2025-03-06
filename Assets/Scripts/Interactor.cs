using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private LayerMask interactableLayer;
    
    [SerializeField] private InteractionHintUI interactionHintPrefab;
    private InteractionHintUI currentHint;

    private Transform lastInteractable;

    void Update()
    {
        int layerMask = LayerMask.GetMask("Sliceable");
        if(Physics.SphereCast(rayOrigin.position, 0.5f, rayOrigin.forward, out RaycastHit hit, 5f, layerMask))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                // If we haven't created a hint yet, instantiate one
                if (currentHint == null || lastInteractable != hit.collider.transform)
                {
                    if (currentHint != null)
                    {
                        currentHint.HideHint(); // Hide the previous one if it's different
                    }

                    lastInteractable = hit.collider.transform;

                    // Spawn the UI hint near the interactable object
                    currentHint = Instantiate(interactionHintPrefab, hit.collider.transform.position, Quaternion.identity);
                    currentHint.ShowHint(hit.collider.transform);
                }

                // When the player presses E, interact
                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                    currentHint.HideHint(); // Hide after interacting
                }
            }
            else
            {
                HideHintIfExists();
            }
        }
        else
        {
            HideHintIfExists();
        }
    }

    private void HideHintIfExists()
    {
        if (currentHint != null)
        {
            currentHint.HideHint();
            currentHint = null;
        }
    }
}

using UnityEngine;
using DG.Tweening;

public class InteractionHintUI : MonoBehaviour
{
    private Transform target; // The object this UI should follow
    private CanvasGroup canvasGroup; // To fade in/out the UI

    [SerializeField] private float appearDuration = 0.5f;
    [SerializeField] private float disappearDuration = 0.3f;
    private Vector3 offset = new Vector3(0, 0.25f, 0); 

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        // canvasGroup.alpha = 0f; // Start hidden
        transform.localScale = Vector3.zero; // Start at zero scale
    }

    // Call this to show the UI near an interactable object
    public void ShowHint(Transform interactable)
    {
        target = interactable;
        gameObject.SetActive(true);

        // Tween scale to create a popping effect
        transform.DOScale(Vector3.one, appearDuration).SetEase(Ease.OutElastic);
        // canvasGroup.DOFade(1f, appearDuration).SetEase(Ease.InOutSine);
    }

    // Call this to hide the UI
    public void HideHint()
    {
        // canvasGroup.DOFade(0f, disappearDuration).SetEase(Ease.InBack)
        //     .OnComplete(() => Destroy(gameObject));
        transform.DOScale(Vector3.zero, disappearDuration * 0.1f).SetEase(Ease.InBack)
        .OnComplete(() => Destroy(gameObject));
    }

    private void Update()
    {
        if (target == null) return;

        // Base position with offset
        Vector3 newPosition = target.position + offset;

        // Direction from target to the camera
        Vector3 toCamera = (Camera.main.transform.position - newPosition).normalized;

        // Move slightly towards the camera
        float moveAmount = 1.2f;
        newPosition += toCamera * moveAmount;

        // Apply new position
        transform.position = newPosition;

        // Make sure it always faces the player
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }


}

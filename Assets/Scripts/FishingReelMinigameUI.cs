using UnityEngine;
using UnityEngine.UI;

public class FishingReelMinigameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform outerBar;  // The track container
    public RectTransform innerBar;  // The moving indicator

    [Header("Settings")]
    public float moveSpeed = 50f; // Movement speed (pixels per second)

    // Boundaries calculated from the outer bar
    private float leftBound;
    private float rightBound;

    // Flag to signal minigame failure
    public bool minigameFailed { get; private set; } = false;

    private void OnEnable()
    {
        // Reset indicator position (centered)
        innerBar.anchoredPosition = Vector2.zero;
        minigameFailed = false;
        // Calculate boundaries based on outerBar width
        float halfWidth = outerBar.rect.width / 2;
        leftBound = -halfWidth;
        rightBound = halfWidth;
    }

    private void Update()
    {
        if (minigameFailed)
            return;

        // Move right when left mouse is held, left when released.
        if (Input.GetMouseButton(0))

        {
            Debug.Log("Mouse held: moving right");
            innerBar.anchoredPosition += new Vector2(moveSpeed * Time.deltaTime, 0);
        }
        else
        {
            innerBar.anchoredPosition -= new Vector2(moveSpeed * Time.deltaTime, 0);
        }

        // Clamp position so it stays within the outer bar
        innerBar.anchoredPosition = new Vector2(
            Mathf.Clamp(innerBar.anchoredPosition.x, leftBound, rightBound),
            innerBar.anchoredPosition.y
        );

        // Check for failure: if the indicator touches either end.
        if (Mathf.Approximately(innerBar.anchoredPosition.x, leftBound) ||
            Mathf.Approximately(innerBar.anchoredPosition.x, rightBound))
        {
            minigameFailed = true;
            Debug.Log("Reel minigame failed: indicator touched the boundary!");
            // Optionally trigger feedback effects or notify a game manager here.
        }
    }
}

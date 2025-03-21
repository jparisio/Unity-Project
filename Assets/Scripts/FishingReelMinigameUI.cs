using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FishingReelMinigameUI : MonoBehaviour
{
    [Header("UI Elements")]
    public RectTransform outerBar;  // The track container (will get a continuous shake)
    public RectTransform innerBar;  // The moving indicator

    [Header("Settings")]
    public float maxSpeed = 500f;
    public float acceleration = 2000f;

    private float currentVelocity = 0f;
    private float leftBound;
    private float rightBound;

    public bool minigameFailed { get; private set; } = false;

    private void OnEnable()
    {
        innerBar.anchoredPosition = Vector2.zero;
        innerBar.localScale = Vector3.one;
        currentVelocity = 0f;
        minigameFailed = false;

        float halfWidth = outerBar.rect.width / 2;
        leftBound = -halfWidth;
        rightBound = halfWidth;

        outerBar.DOShakeAnchorPos(1f, new Vector2(5, 5), 10, 90, false, true)
                .SetLoops(-1, LoopType.Restart);
    }

    private void Update()
    {
        if (minigameFailed) return;

        // Always push left, click to push right
        if (Input.GetMouseButton(0))
        {
            currentVelocity += acceleration * Time.deltaTime;
        }
        else
        {
            currentVelocity -= acceleration * Time.deltaTime;
        }

        currentVelocity = Mathf.Clamp(currentVelocity, -maxSpeed, maxSpeed);
        innerBar.anchoredPosition += new Vector2(currentVelocity * Time.deltaTime, 0);

        float xPos = innerBar.anchoredPosition.x;
        if (xPos < leftBound || xPos > rightBound)
        {
            FailMinigame();
        }

        innerBar.DOScale(1.05f, 0.1f).SetLoops(2, LoopType.Yoyo);
    }

    private void FailMinigame()
    {
        minigameFailed = true;
        Debug.Log("Reel minigame failed!");

        innerBar.DOKill();
        outerBar.DOKill();

        innerBar.DOScale(1.3f, 0.2f).SetLoops(2, LoopType.Yoyo);
        innerBar.DOPunchAnchorPos(new Vector2(15, 0), 0.3f, 10, 1);
    }

    public void setMoveSpeed(int value)
    {
        maxSpeed += value;
    }
}

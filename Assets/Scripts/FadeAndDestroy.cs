using UnityEngine;

public class FadeAndDestroy : MonoBehaviour
{
    private Renderer objectRenderer;
    private Material objectMaterial;
    private Color initialColor;
    private float fadeDuration = 5f; // Time in seconds to fade out
    private float fadeTimer = 0f;

    void Start()
    {
        // Get the Renderer and Material
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectMaterial = objectRenderer.material;
            initialColor = objectMaterial.color;
        }
        else
        {
            Debug.LogError("Renderer not found on " + gameObject.name);
            Destroy(gameObject); // Destroy immediately if no Renderer
        }
    }

    void Update()
    {
        if (objectMaterial == null) return;

        // Increase timer
        fadeTimer += Time.deltaTime;

        // Calculate fade progress
        float alpha = Mathf.Lerp(1f, 0f, fadeTimer / fadeDuration);
        objectMaterial.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);

        // Destroy when fully transparent
        if (alpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
}

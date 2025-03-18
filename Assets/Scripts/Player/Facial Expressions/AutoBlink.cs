using UnityEngine;
using System.Collections;

public class AutoBlink : MonoBehaviour
{
    private SkinnedMeshRenderer faceMesh;
    private int blinkBlendShapeIndex;

    void Start()
    {
        faceMesh = GetComponent<SkinnedMeshRenderer>();
        blinkBlendShapeIndex = faceMesh.sharedMesh.GetBlendShapeIndex("blendShape1.Blink");
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(3f, 6f));
            yield return StartCoroutine(Blink()); 
        }
    }

    IEnumerator Blink()
    {
        // Close eyes
        for (float i = 0; i <= 100; i += 10)
        {
            faceMesh.SetBlendShapeWeight(blinkBlendShapeIndex, i);
            yield return new WaitForSeconds(0.02f);
        }

        // Open eyes
        for (float i = 100; i >= 0; i -= 10)
        {
            faceMesh.SetBlendShapeWeight(blinkBlendShapeIndex, i);
            yield return new WaitForSeconds(0.02f);
        }
    }
}

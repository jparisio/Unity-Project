using UnityEngine;
using EzySlice;
using System.Collections;

public class SliceState : IState
{

private PlayerController player;
private Coroutine slowTimeCoroutine;

    public SliceState(PlayerController player){
        this.player = player;
        
    }

    public void Enter()
    {
        //Debug.Log("Entering slice");
        player.animator.SetBool("isSlicing", true);
        player.cutPlane.gameObject.SetActive(true);
        player.zoomedCam.Priority = 10;    
        player.normalCam.Priority = 0;
        if(slowTimeCoroutine != null) player.StopCoroutine(slowTimeCoroutine);
        slowTimeCoroutine = player.StartCoroutine(SlowTime(0.2f));
    }

    public void Update()
    {
        // Get the UP direction of the plane
        Vector3 forward = player.cutPlane.up; 

        // Convert to 2D space (ignore Z component)
        Vector2 blendInput = new Vector2(forward.x, forward.y);

        // Normalize to keep values between -1 and 1
        blendInput.Normalize();

        // Pass values into animator
        player.animator.SetFloat("X", blendInput.x);
        player.animator.SetFloat("Y", blendInput.y);

        if (Input.GetKeyDown("e"))
        {
            // Rotate cut plane 180 degrees
            player.cutPlane.rotation *= Quaternion.Euler(0, 0, 180);
            Slice();
        }


        if(Input.GetKey("q")){
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void Exit()
    {
        //Debug.Log("Exiting slice State");
        player.animator.SetBool("isSlicing", false);
        player.cutPlane.gameObject.SetActive(false);
        player.zoomedCam.Priority = 0;
        player.normalCam.Priority = 10;

        if(slowTimeCoroutine != null) player.StopCoroutine(slowTimeCoroutine);
        slowTimeCoroutine = player.StartCoroutine(SlowTime(1f));
    }


     public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(player.cutPlane.position, new Vector3(5, 0.1f, 5), player.cutPlane.rotation, LayerMask.GetMask("Sliceable"));

        if (hits.Length <= 0)
            return;

        for (int i = 0; i < hits.Length; i++)
        {
            SlicedHull hull = SliceObject(hits[i].gameObject, player.fishTexture);
            if (hull != null)
            {
                GameObject bottom = hull.CreateLowerHull(hits[i].gameObject, player.fishTexture);
                GameObject top = hull.CreateUpperHull(hits[i].gameObject, player.fishTexture);
                AddHullComponents(bottom);
                AddHullComponents(top);
                Object.Destroy(hits[i].gameObject);
            }
        }
    }

    public void AddHullComponents(GameObject obj)
    {
        obj.layer = 9;
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;

        rb.AddExplosionForce(200, obj.transform.position, 20);
    }

    public SlicedHull SliceObject(GameObject obj, Material crossSectionMaterial = null)
    {
        // Ensure the object has a MeshFilter before slicing
        if (obj.GetComponent<MeshFilter>() == null)
            return null;

        return obj.Slice(player.cutPlane.position, player.cutPlane.up, crossSectionMaterial);
    }

    private IEnumerator SlowTime(float targetTimeScale)
    {
        float startValue = Time.timeScale;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.unscaledDeltaTime * 2f;
            Time.timeScale = Mathf.Lerp(startValue, targetTimeScale, elapsedTime);
            Time.fixedDeltaTime = 0.02f * Time.timeScale; // Adjust physics step size
            yield return null;
        }

        Time.timeScale = targetTimeScale; // Ensure exact final value
    }
}


using UnityEngine;
using EzySlice;
using System.Collections;
using MoreMountains;
using MoreMountains.FeedbacksForThirdParty;
using Unity.Cinemachine;

public class SliceState : IState
{

private PlayerController player;
private Coroutine slowTimeCoroutine;
private int slashCount = 0;
private int slashCountMax = 5;
private MMF_ChromaticAberration_URP chromabb;

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
        slowTimeCoroutine = player.StartCoroutine(SlowTime(0.1f));
        slashCount = 0;

        //disbale interactor 
        player.GetComponent<Interactor>().enabled = false;
        //start cool feedbacks
        chromabb = player.feedbacks.GetFeedbackOfType<MMF_ChromaticAberration_URP>();
        chromabb.RemapIntensityOne = 1f;
        player.feedbacks.PlayFeedbacks();

        //lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //disable freelook camera movement 
       player.inputAxisController.enabled = false;

       
    }

    public void Update()
    {


        if (Input.GetMouseButtonDown(0)) 
        {
            // Rotate cut plane 180 degrees
            player.cutPlane.rotation *= Quaternion.Euler(0, 0, 180);
            Slice();
            // player.impulseSource.GenerateImpulse(.1f);
            player.slashFeedbacks.PlayFeedbacks();
            slashCount++;
        }

        // Get the UP direction of the plane
        Vector3 forward = player.cutPlane.localRotation * Vector3.up;

        Vector2 blendInput = new Vector2(forward.x, forward.y);

        // Normalize to keep values between -1 and 1
        blendInput.Normalize();
        player.animator.SetFloat("X", -blendInput.y);
        player.animator.SetFloat("Y", -blendInput.x);


        if (slashCount >= slashCountMax){
            player.stateMachine.ChangeState(player.idleState);
        }
    }

    public void Exit()
    {
        //Debug.Log("Exiting slice State");
        player.animator.SetBool("isSlicing", false);
        player.StartCoroutine(DisableCutPlane());
        player.zoomedCam.Priority = 0;
        player.normalCam.Priority = 10;

        if(slowTimeCoroutine != null) player.StopCoroutine(slowTimeCoroutine);
        slowTimeCoroutine = player.StartCoroutine(SlowTime(1f));

        //re-enable interactor
        player.GetComponent<Interactor>().enabled = true;

        //reset chrom abb
        chromabb.RemapIntensityOne = 0f;
        chromabb.Play(Vector3.zero);

        //unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        //enable freelook camera movement
        player.inputAxisController.enabled = true;
    }


     public void Slice()
    {
        Collider[] hits = Physics.OverlapBox(player.cutPlane.position, new Vector3(5, 0.1f, 5), player.cutPlane.rotation, LayerMask.GetMask("Sliceable"));

        if (hits.Length <= 0){
            return;
        }
        
        //hit something so create sparks
        player.slashParticles.Play();

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
        obj.layer = LayerMask.NameToLayer("Sliceable");
        Rigidbody rb = obj.AddComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        MeshCollider collider = obj.AddComponent<MeshCollider>();
        collider.convex = true;
        //script to fade away
        obj.AddComponent<MeatInteractable>();

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

    private IEnumerator DisableCutPlane()
    {
        //we need this buffer so the slash particles can play
        yield return new WaitForSeconds(0.1f);
        player.cutPlane.gameObject.SetActive(false);
    }
}


using UnityEngine;

public class SliceState : IState
{

private PlayerController player;

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
    }
}

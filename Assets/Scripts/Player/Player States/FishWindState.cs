using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public class FishWindState : IState
{
    private PlayerController player;
    private float charge;
    private readonly float maxCharge = 5.0f;   // Maximum charge value (e.g., 5 seconds)
    private readonly float chargeRate = 1.0f;    // Charge per second
    private Image windMeter;
    private MMF_Player windFeedback;
    private MMF_SquashAndStretchSpring jiggleFeedback;
    private bool jiggle;

    public FishWindState(PlayerController player)
    {
        this.player = player;

    }

    public void Enter()
    {
        Debug.Log("Entering fish wind");
        // Reset charge when entering the state
        charge = 0f;
        // Optionally, start the wind-up animation.
        player.animator.SetBool("isWinding", true);

        //meter and feedbacks
        windMeter = player.fishMeter.GetComponentInChildren<Image>();
        windFeedback = player.fishMeter.GetComponentInChildren<MMF_Player>();
        jiggleFeedback = windFeedback.GetFeedbackOfType<MMF_SquashAndStretchSpring>();
        jiggle = false;
        jiggleFeedback.Play(Vector3.one);
    }

    public void Update()
    {
        // Accumulate charge while the mouse button is held
        if (Input.GetMouseButton(0))
        {
            charge += chargeRate * Time.deltaTime;
            // Clamp the charge so it never exceeds the maximum
            charge = Mathf.Clamp(charge, 0f, maxCharge);

            HandleWindMeter();
        }
        
        // When the mouse button is released, transition to the next state
        if (Input.GetMouseButtonUp(0))
        {
            // Optionally, store the charge value in the player or pass it to the next state
            // player.charge = charge; // This could affect cast power later
            
            // Transition to the fish cast state (or whatever the next state is)
            player.charge = Mathf.Clamp(charge * 10f, 8f, 20f);
            player.stateMachine.ChangeState(player.fishCastState);
        }
    }

    public void Exit()
    {
        Debug.Log("Exiting fish wind State");
        // Reset the animator flag if needed
        player.animator.SetBool("isWinding", false);

        windMeter.fillAmount = 0f;

        // Optionally, reset the wind-up meter UI
        // player.ui.UpdateWindMeter(0f);
    }

    private void HandleWindMeter(){

            windMeter.fillAmount = charge / maxCharge * 3f;

            if(windMeter.fillAmount >= 1 && !jiggle)
            {
                jiggleFeedback.Play(Vector3.one);
                jiggle = true;
            } else if(windMeter.fillAmount < 1) {
                jiggle = false;
            }

    }
}

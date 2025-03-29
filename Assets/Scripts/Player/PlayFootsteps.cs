using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
   public void PlaySound()
   {
        SoundManager.PlaySound(SoundType.FOOTSTEP, 0.5f);
   }
}
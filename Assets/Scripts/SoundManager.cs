using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum SoundType
{
    SUCCESS,
    FAILURE,
    FOOTSTEP,
    REELING,
    CAST,
}

[RequireComponent(typeof(AudioSource))]

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] soundlist;
    private static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1)
    {
        instance.audioSource.PlayOneShot(instance.soundlist[(int)sound], volume);
    }

    private AudioClip currentLoopedClip;
    
    public static void PlayLoopedSound(SoundType sound, float volume = 1)
    {
        if (instance.audioSource.isPlaying && instance.currentLoopedClip == instance.soundlist[(int)sound]) 
            return;

        instance.currentLoopedClip = instance.soundlist[(int)sound];
        instance.audioSource.clip = instance.currentLoopedClip;
        instance.audioSource.loop = true;
        instance.audioSource.volume = volume;
        instance.audioSource.Play();
    }

    public static void StopLoopedSound()
    {
        instance.audioSource.loop = false;
        instance.audioSource.Stop();
        instance.currentLoopedClip = null;
    }
}
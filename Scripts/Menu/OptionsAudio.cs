using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsAudio : MonoBehaviour
{
    public AudioSource sfx;
    public AudioSource voice;
    public AudioClip sfxSound;
    public AudioClip voiceSound;

    public void PlaySFX()
    {
        if(!sfx.isPlaying && sfx.gameObject.activeInHierarchy)
        {
            sfx.clip = sfxSound;
            sfx.Play();
        }
    }
    public void PlayVoice()
    {
        if(!voice.isPlaying && voice.gameObject.activeInHierarchy)
        {
            voice.clip = voiceSound;
            voice.Play();
        }
    }

    public void PlayMaster()
    {
        PlaySFX();
        PlayVoice();
    }

    public void StopSounds()
    {
        sfx.Stop();
        voice.Stop();
    }
}

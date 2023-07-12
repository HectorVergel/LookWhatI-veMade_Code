using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicController : MonoBehaviour
{
    public AudioClip musicClip;
    AudioSource audioSource;
    public bool finalBoss = false;
    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if(!finalBoss)
        {
            StartCoroutine( FadeIn());
        }
    }
    IEnumerator FadeIn()
    {
        audioSource.clip = musicClip;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += 0.5f * Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1;
    }
}

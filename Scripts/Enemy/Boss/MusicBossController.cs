using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Experimental.Rendering.Universal; 

public class MusicBossController : MonoBehaviour
{
    public Light2D globalLight;
    public Color nightColor;
    public float nightIntensity = 1f;
    public float nightFadeTime;
    public AudioClip fakeMusic;
    public AudioClip realMusic;
    AudioSource audioSource;
    public AudioMixerGroup musicMixer;
    public float fadeInSpeed;
    public DialogueManager dialogue;
    BossSpawner spawner;

    public void Start()
    {
        spawner = GetComponent<BossSpawner>();
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(WaitToStart());
    }
    IEnumerator WaitToStart()
    {
        yield return new WaitForFixedUpdate();
        InstantNightMode();
        bool isPlaying = false;
        DialogueManager[] dialogs = FindObjectsOfType<DialogueManager>();
        foreach (var item in dialogs)
        {
            if(item.playing)
            {
                isPlaying = true;
            }
        }
        while(isPlaying)
        {
            isPlaying = false;
            dialogs = FindObjectsOfType<DialogueManager>();
            foreach (var item in dialogs)
            {
                if(item.playing)
                {
                    isPlaying = true;
                }
            }
            yield return null;
        }
        FadeNightMode();
        CheckStart();
    }

    IEnumerator FadeIn(AudioClip musicClip)
    {
        audioSource.clip = musicClip;
        audioSource.volume = 0;
        audioSource.Play();
        while (audioSource.volume < 1)
        {
            audioSource.volume += fadeInSpeed * Time.deltaTime;
            yield return null;
        }
        audioSource.volume = 1;
    }
    IEnumerator StartDialogue()
    {
        while(audioSource.isPlaying)
        {
            yield return null;
        }

        audioSource.Stop();
        dialogue.StartDialogue();

        while(dialogue != null)
        {
            yield return null;
        }
        StartCoroutine(FadeIn(realMusic));
        audioSource.loop = true;
        spawner.Spawn();
    }

    void CheckStart()
    {
        if(PlayerPrefs.GetInt("BossMusic",0) == 0)
        {
            PlayerPrefs.SetInt("BossMusic",1);
            StartCoroutine(FadeIn(fakeMusic));
            audioSource.loop = false;
            StartCoroutine(StartDialogue());
        }
        else
        {
            StartCoroutine(FadeIn(realMusic));
            audioSource.loop = true;
            spawner.Spawn();
        }
    }

    void InstantNightMode()
    {
        if(PlayerPrefs.GetInt("BossMusic",0) == 1)
        {
            globalLight.color = nightColor;
            globalLight.intensity = nightIntensity;
        }
    }
    void FadeNightMode()
    { 
        if(PlayerPrefs.GetInt("BossMusic",0) == 0)
        {
            StartCoroutine(NightFadeIn());
        }
    }
    IEnumerator NightFadeIn()
    {
        float timer = 0;
        while(timer < nightFadeTime)
        {
            timer+=Time.deltaTime;
            globalLight.color = Color.Lerp(Color.white,nightColor,timer/nightFadeTime);
            globalLight.intensity = Mathf.Lerp(1,nightIntensity,timer/nightFadeTime);
            yield return null;
        }
        globalLight.color = nightColor;
        globalLight.intensity = nightIntensity;
    }
}

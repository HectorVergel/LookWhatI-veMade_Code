using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SoundButtons : MonoBehaviour,ISelectHandler,IPointerEnterHandler
{
    public AudioClip sound;
    void ISelectHandler.OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        DoSound();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        DoSound();
    }

    void DoSound()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSound : MonoBehaviour
{
    public bool justEnd = false;
    public AudioSource audioSource;
    public AudioClip sound;
    private void OnEnable() {
        if(!justEnd)
        {
            audioSource.PlayOneShot(sound);
        }
    }
    private void OnDisable() {
        if(audioSource.gameObject.activeInHierarchy)
        {
            audioSource.PlayOneShot(sound);
        }
    }
}

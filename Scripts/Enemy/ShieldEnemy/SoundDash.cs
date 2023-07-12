using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDash : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip sound;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
        audioSource.pitch = 1+ Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }
    void Update()
    {
        if(!audioSource.isPlaying)
        {
            Destroy(this.gameObject);
        }
    }
}

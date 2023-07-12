using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitSoundEffects : MonoBehaviour
{
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioClip sound2;
    [SerializeField] AudioClip sound3;

    AudioSource audios;
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void PlaySound()
    {
        int randomIndex = Random.Range(1, 3);
        switch (randomIndex)
        {
            case 1:
                audios.clip = sound1;
                break;
            case 2:
                audios.clip = sound2;
                break;
            case 3:
                audios.clip = sound3;
                break;
            default:
                break;
        }
    }
}

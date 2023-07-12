using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CheckPoint : MonoBehaviour
{
    public int checkPointNumber;
    Animator anim;
    AudioSource audioSource;
    public AudioClip sound;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() {
        anim = GetComponent<Animator>();
        if(PlayerPrefs.GetInt("CheckPoint",0) == checkPointNumber)
        {
            anim.Play("Using");
        }
        else
        {
            anim.Play("NotUsing");
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && PlayerPrefs.GetInt("CheckPoint",0) != checkPointNumber)
        {
            if(GetComponent<AchievementPopUp>() != null)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
            audioSource.PlayOneShot(sound);
            PlayerPrefs.SetInt("CheckPoint",checkPointNumber);
            anim.SetTrigger("Activated");
            var checkpoints = FindObjectsOfType<CheckPoint>();
            foreach (var item in checkpoints)
            {
                if(item != this)
                {
                    item.anim.Play("NotUsing");
                }
            }
        }
    }
}

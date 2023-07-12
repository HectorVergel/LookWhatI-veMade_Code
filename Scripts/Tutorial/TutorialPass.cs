using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPass : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            PlayerPrefs.SetInt("Skill1",0);
            PlayerPrefs.SetInt("Skill2",0);
            PlayerPrefs.SetInt("Dash",0);
            PlayerPrefs.SetInt("Roll",0);
        }
    }
}

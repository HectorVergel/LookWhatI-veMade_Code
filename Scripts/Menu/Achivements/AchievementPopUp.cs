using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementPopUp : MonoBehaviour
{
    public GameObject popUp;
    public int achievementID;
    public string achievementName;
    public void Activate()
    {
        if(PlayerPrefs.GetInt("Ach" + achievementID.ToString(),0) == 0 && PlayerPrefs.GetInt("Zen",0) == 0)
        {
            PlayerPrefs.SetInt("Ach" + achievementID.ToString(),1);
            var pref = Instantiate(popUp);
            pref.GetComponent<PopUpText>().achievementName = this.achievementName;
            pref.GetComponent<PopUpText>().SetAll();
        }
    }
}

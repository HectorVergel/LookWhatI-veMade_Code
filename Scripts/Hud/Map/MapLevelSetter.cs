using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MapLevelSetter : MonoBehaviour
{
    public string levelName;
    public string cityName;
    public Sprite levelIcon;
    public void SetSprite(Sprite notDiscovered)
    {
        if(PlayerPrefs.GetInt(levelName,0) == 0)
        {
            GetComponent<Image>().sprite = notDiscovered;
            GetComponentInChildren<TextMeshProUGUI>().text = "???";
        }
        else
        {
            GetComponent<Image>().sprite = levelIcon;
            GetComponentInChildren<TextMeshProUGUI>().text = cityName;
        }
    }
}

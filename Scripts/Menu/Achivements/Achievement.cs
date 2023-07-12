using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Achievement : MonoBehaviour
{
    public string achievementName;
    public Sprite iconSprite;
    public Sprite bloquedSprite;
    public Image icon;
    public GameObject panel;
    private void OnEnable() 
    {
        SetState();
    }
    public void SetState()
    {
        if(PlayerPrefs.GetInt(achievementName,0) == 1)
        {
            icon.sprite = iconSprite;
            panel.SetActive(false);
        }
        else
        {
            icon.sprite = bloquedSprite;
            panel.SetActive(true);
        }
    }
}

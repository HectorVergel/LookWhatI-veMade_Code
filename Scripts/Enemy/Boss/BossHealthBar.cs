using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    public Image back;
    public Image fill;
    public Image border;
    public Sprite rageSprite;
    public Sprite normalSprite;


    public void SetHelath(float current, float max,bool rage)
    {
        back.color = new Color(0,0,0,1);
        fill.color = new Color(1,1,1,1);
        border.color = new Color(1,1,1,1);
        fill.fillAmount = 1;
        if(rage)
        {
            border.sprite = rageSprite;
        }
        else
        {
            border.sprite = normalSprite;
        }

        fill.fillAmount = current/max;
    }
}

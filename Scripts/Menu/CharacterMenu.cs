using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Sprite meleSprite;
    public Sprite wizardSprite;
    private void OnEnable() {
        if(PlayerPrefs.GetInt("Character",0) == 0)
        {
            GetComponent<Image>().sprite = meleSprite;
        }
        else
        {
            GetComponent<Image>().sprite = wizardSprite;
        }
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Image characterImage;
    public static Hud instance;
    public GameObject ability1;
    public GameObject ability2;
    public GameObject dash;
    public GameObject roll;
    public Image ability1N;
    public Image ability2N;
    public Image ability1B;
    public Image ability2B;
    public Image dashN;
    public Image rollN;
    public Image dashB;
    public Image rollB;
    public Sprite mele1;
    public Sprite mele2;
    public Sprite wizard1;
    public Sprite wizard2;
    public Sprite dashSprite;
    public Sprite rollSprite;
    public Sprite wizardCharacter;
    public Sprite meleCharacter;
    public string skill1;
    public string skill2;
    public string dashName;
    public string rollName;
    public GameObject map;

    // Start is called before the first frame update
    private void Awake() {
        instance = this;
    }
    void Start()
    {
        ResetAbiltyUI();
    }

    public void ResetAbiltyUI()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            ability1N.sprite = mele1;
            ability1B.sprite = mele1;
            ability2N.sprite = mele2;
            ability2B.sprite = mele2;
            characterImage.sprite = meleCharacter;

        }
        else
        {
            ability1N.sprite = wizard1;
            ability1B.sprite = wizard1;
            ability2N.sprite = wizard2;
            ability2B.sprite = wizard2;
            characterImage.sprite = wizardCharacter;
        }
        dashN.sprite = dashSprite;
        dashB.sprite = dashSprite;
        rollN.sprite = rollSprite;
        rollB.sprite = rollSprite;
        if(PlayerPrefs.GetInt(skill1,0) == 0)
        {
            ability1.SetActive(false);
        }
        else
        {
            ability1.SetActive(true);
        }

        if(PlayerPrefs.GetInt(skill2,0) == 0)
        {
            ability2.SetActive(false);
        }
        else
        {
            ability2.SetActive(true);
        }

        if(PlayerPrefs.GetInt(dashName,0) == 0)
        {
            dash.SetActive(false);
        }
        else
        {
            dash.SetActive(true);
        }

        if(PlayerPrefs.GetInt(rollName,0) == 0)
        {
            roll.SetActive(false);
        }
        else
        {
            roll.SetActive(true);
        }

        if(PlayerPrefs.GetInt("Map",0) == 0)
        {
            map.SetActive(false);
        }
        else
        {
            map.SetActive(true);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConsumablesInventory : MonoBehaviour
{
    public GameObject hp1;
    public GameObject hp2;
    public GameObject hp3;
    public GameObject mana;
    public GameObject tp;
    public GameObject xp;

    public TextMeshProUGUI texthp1;
    public TextMeshProUGUI texthp2;
    public TextMeshProUGUI texthp3;
    public TextMeshProUGUI textmana;
    public TextMeshProUGUI texttp;
    public TextMeshProUGUI textxp;

    private void OnEnable() 
    {
        SetAll();
    }


    public void SetAll()
    {
        if(Consumables.instance.CheckHP1())
        {
            hp1.SetActive(true);
            texthp1.text = PlayerPrefs.GetInt(Consumables.instance.potionHP1,0).ToString();
        }
        else
        {
            hp1.SetActive(false);
        }

        if(Consumables.instance.CheckHP2())
        {
            hp2.SetActive(true);
            texthp2.text = PlayerPrefs.GetInt(Consumables.instance.potionHP2,0).ToString();
        }
        else
        {
            hp2.SetActive(false);
        }

        if(Consumables.instance.CheckHP3())
        {
            hp3.SetActive(true);
            texthp3.text = PlayerPrefs.GetInt(Consumables.instance.potionHP3,0).ToString();
        }
        else
        {
            hp3.SetActive(false);
        }

        if(Consumables.instance.CheckMana())
        {
            mana.SetActive(true);
            textmana.text = PlayerPrefs.GetInt(Consumables.instance.potionMana,0).ToString();
        }
        else
        {
            mana.SetActive(false);
        }

        if(Consumables.instance.CheckTP())
        {
            tp.SetActive(true);
            texttp.text = PlayerPrefs.GetInt(Consumables.instance.potionTP,0).ToString();
        }
        else
        {
            tp.SetActive(false);
        }

        if(Consumables.instance.CheckXP())
        {
            xp.SetActive(true);
            textxp.text = PlayerPrefs.GetInt(Consumables.instance.potionXP,0).ToString();
        }
        else
        {
            xp.SetActive(false);
        }
    }
}


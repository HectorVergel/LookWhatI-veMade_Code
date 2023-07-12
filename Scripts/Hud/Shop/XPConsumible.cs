using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class XPConsumible : MonoBehaviour
{
    TextMeshProUGUI amount;
    public GameObject soldOut;
    bool sold = false;
    private void Start() {
        amount = GetComponent<TextMeshProUGUI>();
        if(PlayerPrefs.GetInt("XPConsumible",Shop.instance.numberOfXPConsumibles) <= 0)
        {
            sold = true;
            soldOut.SetActive(true);
        }
        else
        {
            sold = false;
            soldOut.SetActive(false);
        }
    }
    private void Update() {
        amount.text = PlayerPrefs.GetInt("XPConsumible",Shop.instance.numberOfXPConsumibles).ToString();
        if(PlayerPrefs.GetInt("XPConsumible",Shop.instance.numberOfXPConsumibles) <= 0 && !sold)
        {
            sold = true;
            soldOut.SetActive(true);
        }
    }
}

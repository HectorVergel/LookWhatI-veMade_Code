using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Coins : MonoBehaviour
{
    public static Coins instance;
    public TextMeshProUGUI coinsLabel;
    public TextMeshProUGUI newCoinsLabel;
    public float alphaSpeed;
    public float numberStayTime;
    public float exchangeSpeed;
    int cointainerCoins;
    int currentCoins;


    private void Awake() {
        instance = this;
    }

    private void Start() 
    {
        newCoinsLabel.gameObject.SetActive(false);
        currentCoins = PlayerPrefs.GetInt("Coins",0);
        coinsLabel.text = currentCoins.ToString();
        cointainerCoins = 0;
    }

    // private void Update() {
    //     if(Input.GetKeyDown("r"))
    //     {
    //         AddCoins(15);
    //     }

    //     if(Input.GetKeyDown("t"))
    //     {
    //         RemoveCoins(20);
    //     }
    // }
    public void AddCoins(int amount)
    {
        PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins",0) + amount);
        //coinsLabel.text = PlayerPrefs.GetInt("Coins",0).ToString();
        StopAllCoroutines();
        StartCoroutine(AddCoinsDisplay(amount));
    }

    public bool CheckIfHaveCoins(int amount)
    {
        if(amount > currentCoins)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public void RemoveCoins(int amount)
    {
        PlayerPrefs.SetInt("Coins",PlayerPrefs.GetInt("Coins",0) - amount);
        currentCoins -= amount;
        coinsLabel.text = currentCoins.ToString();
    }

    IEnumerator AddCoinsDisplay(int coins)
    {
        newCoinsLabel.gameObject.SetActive(true);
        cointainerCoins +=coins;
        newCoinsLabel.text = "+" + cointainerCoins.ToString();
        //fade in
        while(newCoinsLabel.color.a < 1)
        {
            Color col = new Color(0,1,0,newCoinsLabel.color.a + alphaSpeed * Time.unscaledDeltaTime);
            newCoinsLabel.color = col;
            yield return null;
        }
        newCoinsLabel.color = new Color(0,1,0,1);
        //stay
        float timer = 0;
        while(timer < numberStayTime)
        {
            timer+=Time.unscaledDeltaTime;
            yield return null;
        }
        StartCoroutine(ExchangeCoins());
    }

    IEnumerator ExchangeCoins()
    {
        float fakeContainer = cointainerCoins;
        while(cointainerCoins > 0)
        {
            fakeContainer -= exchangeSpeed * Time.unscaledDeltaTime;
            if((int)fakeContainer < cointainerCoins)
            {
                int difference = (cointainerCoins - (int)fakeContainer);
                currentCoins += difference;
                cointainerCoins -= difference;
                newCoinsLabel.text = "+" + cointainerCoins.ToString();
                coinsLabel.text = currentCoins.ToString();
            }
            yield return null;
        }
        newCoinsLabel.text = "+" + cointainerCoins.ToString();
        coinsLabel.text = currentCoins.ToString();
        //fadeOut
        newCoinsLabel.color = new Color(0,1,0,0);
        newCoinsLabel.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinsShop : MonoBehaviour
{
    public TextMeshProUGUI coins;
    public TextMeshProUGUI coinsSpended;
    public float numberVelocity;
    public float numberStayTime;
    public float redSpeed;
    bool redPoint = false;
    private void OnEnable() {
        SetCoins();
        redPoint = false;
        coins.color = new Color(1,1,1,1);
        coinsSpended.color = new Color(1,0,0,0);
    }

    public void SetCoins()
    {
        coins.text = PlayerPrefs.GetInt("Coins",0).ToString();
    }

    public void RemoveCoins(int amount)
    {
        StopAllCoroutines();
        StartCoroutine(RemoveCoinsDisplay(amount));
    }

    public void RedCoins()
    {
        if(!redPoint)
        {
            StartCoroutine(RedPoints());
        }
    }

    IEnumerator RemoveCoinsDisplay(int coins)
    {
        coinsSpended.gameObject.SetActive(true);
        coinsSpended.text = "-" + coins.ToString();
        coinsSpended.color = new Color(1,0,0,0);
        //fade in
        while(coinsSpended.color.a < 1)
        {
            Color col = new Color(1,0,0,coinsSpended.color.a + numberVelocity * Time.deltaTime);
            coinsSpended.color = col;
            yield return null;
        }
        coinsSpended.color = new Color(1,0,0,1);
        //stay
        yield return new WaitForSeconds(numberStayTime);
        //fade out
        while(coinsSpended.color.a > 0)
        {
            Color col = new Color(1,0,0,coinsSpended.color.a - numberVelocity * Time.deltaTime);
            coinsSpended.color = col;
            yield return null;
        }
        coinsSpended.color = new Color(1,0,0,0);
        coinsSpended.gameObject.SetActive(false);
    }

    IEnumerator RedPoints()
    {
        redPoint = true;
        coins.color = new Color(1,1,1,1);
        while(coins.color.g > 0)
        {
            Color col = new Color(1,coins.color.g - redSpeed * Time.deltaTime,coins.color.b - redSpeed * Time.deltaTime,1);
            coins.color = col;
            yield return null;
        }
        coins.color = new Color(1,0,0,1);
        while(coins.color.g < 1)
        {
            Color col = new Color(1,coins.color.g + redSpeed * Time.deltaTime,coins.color.b + redSpeed * Time.deltaTime,1);
            coins.color = col;
            yield return null;
        }
        coins.color = new Color(1,1,1,1);
        redPoint = false;
    }
}

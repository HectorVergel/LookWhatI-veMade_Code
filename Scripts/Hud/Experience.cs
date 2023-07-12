using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Experience : MonoBehaviour
{
    bool goingDown;
    public float barSpeed;
    public static Experience instance;
    public TextMeshProUGUI currentLVL;
    public TextMeshProUGUI nextLVL;
    public Image image;
    public int initialMaxXP;
    public float functionVariable;
    int maxXP;

    public static Action OnLevelUp;
    private void Awake() {
        instance = this;
    }
    private void Start()
    {
        maxXP = PlayerPrefs.GetInt("MaxXP",initialMaxXP);
        currentLVL.text = PlayerPrefs.GetInt("LVL",0).ToString();
        goingDown = false;
        image.fillAmount = 0f;
        StartCoroutine(GoUp());
    }

    public void AddXP(int xp)
    {
        if(PlayerPrefs.GetInt("XP",0) + xp >= maxXP)
        {
            OnLevelUp?.Invoke();
            int newXP = PlayerPrefs.GetInt("XP",0) + xp - maxXP;
            PlayerPrefs.SetInt("LVL",PlayerPrefs.GetInt("LVL",0) + 1);
            UpdateMaxXP();
            StatsSystem.instance.AddPoint();
            PlayerPrefs.SetInt("XP", newXP);
            StopAllCoroutines();
            StartCoroutine(GoDown());
            currentLVL.text = PlayerPrefs.GetInt("LVL",0).ToString();
        }
        else
        {
            PlayerPrefs.SetInt("XP",PlayerPrefs.GetInt("XP",0) + xp);
            StopCoroutine(GoUp());
            StartCoroutine(GoUp());
        }
    }
    void UpdateMaxXP()
    {
        maxXP = Mathf.RoundToInt(initialMaxXP * Mathf.Pow(functionVariable,PlayerPrefs.GetInt("LVL",0)));
        PlayerPrefs.SetInt("MaxXP",maxXP);
    }

    public void Add1Level()
    {
        int xpToLevelUp = maxXP - PlayerPrefs.GetInt("XP",0);
        AddXP(xpToLevelUp);
    }
    IEnumerator GoDown()
    {
        goingDown = true;
        while(image.fillAmount < 1f)
        {
            image.fillAmount += barSpeed * Time.deltaTime;
            yield return null;
        }
        image.fillAmount = 1f;

        while(image.fillAmount > 0f)
        {
            image.fillAmount -= barSpeed * 2 * Time.deltaTime;
            yield return null;
        }
        image.fillAmount = 0f;
        while(image.fillAmount < (float)PlayerPrefs.GetInt("XP",0)/ maxXP)
        {
            image.fillAmount += barSpeed * Time.deltaTime;
            yield return null;
        }
        image.fillAmount = (float)PlayerPrefs.GetInt("XP",0)/ maxXP;
        goingDown = false;
    }
    IEnumerator GoUp()
    {
        while(goingDown)
        {
            yield return null;
        }
        while(image.fillAmount < (float)PlayerPrefs.GetInt("XP",0)/ maxXP)
        {
            image.fillAmount += barSpeed * Time.deltaTime;
            yield return null;
        }
        image.fillAmount = (float)PlayerPrefs.GetInt("XP",0)/ maxXP;
    }
}

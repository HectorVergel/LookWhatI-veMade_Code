using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class StatsSystem : MonoBehaviour
{
    public bool redPoint;
    public float redSpeed;
    public static StatsSystem instance;
    public float maxLevels;
    public float initialHealth;
    public float maxHealth;
    public float initialStrength;
    public float maxStrength;
    public float initialMana;
    public float maxMana;
    public float initialLuck;
    public float maxLuck;

    float textHealth;
    float textMana;
    float textLuck;
    float textStrength;


    public StatProgres progres;

    public EventSystem eventSystem;


    public Image healthButton;
    [SerializeField] Image strengthButton;
    [SerializeField] Image manaButton;
    [SerializeField] Image luckButton;

    [SerializeField] Sprite lockButton;

    [SerializeField] Sprite MAX;
    [SerializeField] Sprite buttonUp;


    [SerializeField] TextMeshProUGUI pointsLabel;

    [SerializeField] TextMeshProUGUI healthLabel;
    [SerializeField] TextMeshProUGUI strengthLabel;
    [SerializeField] TextMeshProUGUI manaLabel;
    [SerializeField] TextMeshProUGUI luckLabel;
    [SerializeField] GameObject resetButton;
    public int resetCost;
    public TextMeshProUGUI resetCostText;


    [System.NonSerialized] public float currentHP;
    [System.NonSerialized] public float currentStrength;
    [System.NonSerialized] public float currentMana;
    [System.NonSerialized] public float currentLuck;

    public bool goingDown;
    public float textSpeed;
    public AudioSource audioSource;
    public AudioClip upgradeSound;
    public AudioClip cantUpgradeSound;
    public AudioClip moneySound;

    private void Awake() 
    {
        instance = this;
        SetStats();
        resetCostText.text = resetCost.ToString();
    }
    public void SetNumbersLabels()
    {
        healthLabel.text = currentHP.ToString();
        luckLabel.text = (currentLuck * 100).ToString();
        if(Character.currentType == CharacterType.Warrior)
        {
            strengthLabel.text = currentStrength.ToString();
        }
        else
        {
            strengthLabel.text = (1 + Mathf.RoundToInt(PlayerPrefs.GetInt("Strength",0) * 0.5f + 0.01f)).ToString();
        }
        manaLabel.text = currentMana.ToString();
        pointsLabel.text = PlayerPrefs.GetInt("Points", 0).ToString();

        textHealth = currentHP;
        if(Character.currentType == CharacterType.Warrior)
        {
            textStrength = currentStrength;
        }
        else
        {
            textStrength = 1 + Mathf.RoundToInt(PlayerPrefs.GetInt("Strength",0) * 0.5f + 0.01f);
        }
        textLuck = currentLuck;
        textMana = currentMana;
    }

    public void SetVisuals()
    {
        SetButtons();
        pointsLabel.text = PlayerPrefs.GetInt("Points", 0).ToString();
    }
    IEnumerator RedPoints()
    {
        audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(cantUpgradeSound);
        redPoint = true;
        TextMeshProUGUI text = pointsLabel.GetComponentInChildren<TextMeshProUGUI>();
        text.color = new Color(1,1,1,1);
        while(text.color.g > 0)
        {
            Color col = new Color(1,text.color.g - redSpeed * Time.unscaledDeltaTime,text.color.b - redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(1,0,0,1);
        while(text.color.g < 1)
        {
            Color col = new Color(1,text.color.g + redSpeed * Time.unscaledDeltaTime,text.color.b + redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(1,1,1,1);
        redPoint = false;
    }
    IEnumerator RedResetCost()
    {
        audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(cantUpgradeSound);
        TextMeshProUGUI text = resetCostText;
        text.color = new Color(1,1,1,1);
        while(text.color.g > 0)
        {
            Color col = new Color(1,text.color.g - redSpeed * Time.unscaledDeltaTime,text.color.b - redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(1,0,0,1);
        while(text.color.g < 1)
        {
            Color col = new Color(1,text.color.g + redSpeed * Time.unscaledDeltaTime,text.color.b + redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(1,1,1,1);
    }
    IEnumerator GreenPoints()
    {
        audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0f);
        audioSource.PlayOneShot(moneySound);
        TextMeshProUGUI text = pointsLabel.GetComponentInChildren<TextMeshProUGUI>();
        text.color = new Color(1,1,1,1);
        while(text.color.r > 0)
        {
            Color col = new Color(text.color.r - redSpeed * Time.unscaledDeltaTime,1,text.color.b - redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(0,1,0,1);
        float timi = 0;
        while(timi < 0.75f)
        {
            timi+=Time.unscaledDeltaTime;
            yield return null;
        }
        while(text.color.r < 1)
        {
            Color col = new Color(text.color.r + redSpeed * Time.unscaledDeltaTime,1,text.color.b - redSpeed * Time.unscaledDeltaTime,1);
            text.color = col;
            yield return null;
        }
        text.color = new Color(1,1,1,1);
    }

    IEnumerator GoDown(TextMeshProUGUI texto, float amount, float max, float operatorText, float current)
    {
        goingDown = true;
        float localSpeed = textSpeed * (max - amount) * 1.5f;
        operatorText = current;
        while (operatorText > amount )
        {

            operatorText -= localSpeed * Time.unscaledDeltaTime;
            texto.text = Mathf.RoundToInt(operatorText).ToString();
            yield return null;
        }

        operatorText = amount;

        texto.text = operatorText.ToString();
        CheckAllDown();
    }
    IEnumerator GoUp(TextMeshProUGUI texto, float amount, float max, float current, float operatorText)
    {
        
        float localSpeed = textSpeed * (max - amount);
        operatorText = current - (max - amount) / maxLevels;
        while (goingDown)
        {
            yield return null;
        }
        while (operatorText < current)
        {

            operatorText  += localSpeed * Time.unscaledDeltaTime;
            texto.text = Mathf.RoundToInt(operatorText).ToString();
            yield return null;
        }

        operatorText = current;

        texto.text = operatorText.ToString();
    }
    void CheckAllDown()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            if (float.Parse(strengthLabel.text) == initialStrength && float.Parse(manaLabel.text) == initialMana && float.Parse(healthLabel.text) == initialHealth && float.Parse(luckLabel.text)*100 == initialLuck*100)
            {
                goingDown = false;
            }
        }
        else
        {
            if (float.Parse(strengthLabel.text) == 1 && float.Parse(manaLabel.text) == initialMana && float.Parse(healthLabel.text) == initialHealth && float.Parse(luckLabel.text)*100 == initialLuck*100)
            {
                goingDown = false;
            }
        }
    }
    void SetButtons()
    {
        if(PlayerPrefs.GetInt("Points",0) <= 0)
        {
            if(!(currentLuck == maxLuck)) 
            {
                luckButton.sprite = lockButton;
            }
            else
            {
                luckButton.sprite = MAX;
            }
            if (!(currentHP == maxHealth))
            {
                healthButton.sprite = lockButton;
            }
            else
            {
                healthButton.sprite = MAX;
            }
            if (!(currentMana == maxMana))
            {
                manaButton.sprite = lockButton;
            }
            else
            {
                manaButton.sprite = MAX;
            }
            if (!(currentStrength == maxStrength))
            {
                strengthButton.sprite = lockButton;
            }
            else
            {
                strengthButton.sprite = MAX;
            }
            
            
        
        }
        else
        {
            if(PlayerPrefs.GetInt("Health",0) < maxLevels)
            {
                healthButton.sprite = buttonUp;
            }
            else
            {
                healthButton.sprite = MAX;
            }

            if(PlayerPrefs.GetInt("Strength",0) < maxLevels)
            {
                strengthButton.sprite = buttonUp;
            }
            else
            {
                strengthButton.sprite = MAX;
            }

            if(PlayerPrefs.GetInt("Mana",0) < maxLevels)
            {
                manaButton.sprite = buttonUp;
            }
            else
            {
                manaButton.sprite = MAX;
            }

            if(PlayerPrefs.GetInt("Luck",0) < maxLevels)
            {
                luckButton.sprite = buttonUp;
            }
            else
            {
                luckButton.sprite = MAX;
            }
        }

        if(GetComponent<AchievementPopUp>() != null)
        {
            if(currentHP == maxHealth)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
            if(currentLuck == maxLuck)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
            if(currentMana == maxMana)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
            if(currentStrength == maxStrength)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
        }
    }
    
    void ResetNextStat(string st)
    {
        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            if(item.stat == st)
            {
                item.Check();
            }
        }
    }
    public void LevelUpHealth()
    {   
        if(PlayerPrefs.GetInt("Health",0) < maxLevels && PlayerPrefs.GetInt("Points",0) > 0)
        {
            PlayerPrefs.SetInt("Health",PlayerPrefs.GetInt("Health",0) + 1);
            SetStats();
            RemovePoint();
            progres.UpHealth();
            StopCoroutine(GoUp(healthLabel, initialHealth, maxHealth, currentHP,textHealth));
            StartCoroutine(GoUp(healthLabel, initialHealth, maxHealth, currentHP, textHealth));
            HealthSystemPlayer.instance.AddHPWithStat((maxHealth-initialHealth)/maxLevels);
            ResetNextStat("Health");
            audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(upgradeSound);
            HealthBarPortions [] portions = FindObjectsOfType<HealthBarPortions>();
            foreach (var item in portions)
            {
                item.SetPortions();
            }
        }
        else if(PlayerPrefs.GetInt("Points",0) <= 0 &&  !redPoint)
        {
            StartCoroutine(RedPoints());
        }
        
    }
    public void LevelUpStrength()
    {   
        if(PlayerPrefs.GetInt("Strength",0) < maxLevels && PlayerPrefs.GetInt("Points",0) > 0)
        {
            PlayerPrefs.SetInt("Strength", PlayerPrefs.GetInt("Strength", 0) + 1);
            SetStats();
            RemovePoint();
            progres.UpStrength();
            StopCoroutine(GoUp(strengthLabel, initialStrength, maxStrength, currentStrength, textStrength));
            if(Character.currentType == CharacterType.Warrior)
            {
                StartCoroutine(GoUp(strengthLabel, initialStrength, maxStrength, currentStrength, textStrength));
            }
            else
            {
                StartCoroutine(GoUp(strengthLabel, 1, 6, 1 + Mathf.RoundToInt(PlayerPrefs.GetInt("Strength",0) * 0.5f + 0.01f), textStrength));
            }

            ResetNextStat("Strength");
            audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(upgradeSound);
        }
        else if(PlayerPrefs.GetInt("Points",0) <= 0 &&  !redPoint)
        {
            StartCoroutine(RedPoints());
        }
        
    }
    public void LevelUpMana()
    {
        if (PlayerPrefs.GetInt("Mana", 0) < maxLevels && PlayerPrefs.GetInt("Points",0) > 0)
        {
            PlayerPrefs.SetInt("Mana", PlayerPrefs.GetInt("Mana", 0) + 1);
            SetStats();
            RemovePoint();
            progres.UpMana();
            StopCoroutine(GoUp(manaLabel, initialMana, maxMana, currentMana, textMana));
            StartCoroutine(GoUp(manaLabel, initialMana, maxMana, currentMana, textMana));
            Mana.instance.AddManaWithStat((maxMana-initialMana)/maxLevels);
            ResetNextStat("Mana");
            audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(upgradeSound);
            HealthBarPortions [] portions = FindObjectsOfType<HealthBarPortions>();
            foreach (var item in portions)
            {
                item.SetPortions();
            }
        }
        else if(PlayerPrefs.GetInt("Points",0) <= 0 &&  !redPoint)
        {
            StartCoroutine(RedPoints());
        }
            
    }
    public void LevelUpLuck()
    {
        if (PlayerPrefs.GetInt("Luck", 0) < maxLevels && PlayerPrefs.GetInt("Points",0) > 0) 
        {
            PlayerPrefs.SetInt("Luck", PlayerPrefs.GetInt("Luck", 0) + 1);
            SetStats();
            RemovePoint();
            progres.UpHumor();
            StopCoroutine(GoUp(luckLabel, initialLuck * 100, maxLuck * 100, currentLuck * 100,textLuck));
            StartCoroutine(GoUp(luckLabel, initialLuck * 100, maxLuck * 100, currentLuck * 100,textLuck));
            ResetNextStat("Luck");
            audioSource.pitch = 1f + UnityEngine.Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(upgradeSound);
        }
        else if(PlayerPrefs.GetInt("Points",0) <= 0 &&  !redPoint)
        {
            StartCoroutine(RedPoints());
        }
            
    }

    void SetStats()
    {
        currentHP = initialHealth +  (maxHealth-initialHealth)/maxLevels * PlayerPrefs.GetInt("Health",0);
        currentStrength = initialStrength + (maxStrength-initialStrength)/maxLevels * PlayerPrefs.GetInt("Strength",0);
        currentMana = initialMana + (maxMana-initialMana)/maxLevels * PlayerPrefs.GetInt("Mana",0);
        currentLuck =initialLuck + (maxLuck-initialLuck)/maxLevels * PlayerPrefs.GetInt("Luck",0);
    }

    public void ResetLevels()
    {
        if((PlayerPrefs.GetInt("Health",0) > 0 || PlayerPrefs.GetInt("Mana",0) > 0 || PlayerPrefs.GetInt("Strength",0) > 0 || PlayerPrefs.GetInt("Luck",0) > 0))
        if(Coins.instance.CheckIfHaveCoins(resetCost))
        {
            Coins.instance.RemoveCoins(resetCost);
            StopAllCoroutines();
            StartCoroutine(GreenPoints());
            StartCoroutine(GoDown(luckLabel, initialLuck*100, maxLuck*100, textLuck, currentLuck*100));
            StartCoroutine(GoDown(manaLabel, initialMana, maxMana, textMana, currentMana));
            if(Character.currentType == CharacterType.Warrior)
            {
                StartCoroutine(GoDown(strengthLabel, initialStrength, maxStrength, textStrength, currentStrength));
            }
            else
            {
                StartCoroutine(GoDown(strengthLabel, 1, 6, textStrength, 1 + Mathf.RoundToInt(PlayerPrefs.GetInt("Strength",0) * 0.5f + 0.01f)));
            }
            StartCoroutine(GoDown(healthLabel, initialHealth, maxHealth, textHealth, currentHP));
            PlayerPrefs.SetInt("Health",0);
            PlayerPrefs.SetInt("Mana",0);
            PlayerPrefs.SetInt("Strength",0);
            PlayerPrefs.SetInt("Luck",0);
            PlayerPrefs.SetInt("Points", PlayerPrefs.GetInt("LVL",0));
            SetStats();
            SetVisuals();
            Mana.instance.SetMaxMana();
            HealthSystemPlayer.instance.SetMaxHP();
            progres.StartGoingDown();
            HealthBarPortions [] portions = FindObjectsOfType<HealthBarPortions>();
            foreach (var item in portions)
            {
                item.SetPortions();
            }
        }
        else
        {
            StartCoroutine(RedResetCost());
        }
    }
    public void AddPoint()
    {
        PlayerPrefs.SetInt("Points",PlayerPrefs.GetInt("Points",0) + 1);
        SetVisuals();
    }

    public void RemovePoint()
    {
        PlayerPrefs.SetInt("Points",PlayerPrefs.GetInt("Points",0) - 1);
        SetVisuals();
    }
    
}

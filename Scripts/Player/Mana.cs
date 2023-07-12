using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Mana : MonoBehaviour
{
    Image image;
    Image follow;
    bool following = false;
    public float manaRegenAmount;
    public bool regenMana = true;
    public float cooldownToRegen;
    float timer = 0;
    public static Mana instance;
    public float currentMana;
    public float followVelocity;

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        image = GameObject.FindWithTag("ManaBar").GetComponent<Image>();
        follow = GameObject.FindWithTag("FollowMana").GetComponent<Image>();
        currentMana = StatsSystem.instance.currentMana;
        image.fillAmount = currentMana/StatsSystem.instance.currentMana;
        if(SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Tutorial"))
        {
            cooldownToRegen = 0;
            manaRegenAmount = 100;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(regenMana)
        {
            RegenerateMana();
        }
        else
        {
            TimerToRegenMana();
        }

        image.fillAmount = currentMana/StatsSystem.instance.currentMana;
    }
    public void MaxMana()
    {
        currentMana = StatsSystem.instance.currentMana;
    }
    public void SetMaxMana()
    {
        if(currentMana > StatsSystem.instance.currentMana)
        {
            currentMana = StatsSystem.instance.currentMana;
        }
        image.fillAmount = currentMana/StatsSystem.instance.currentMana;
    }

    void RegenerateMana()
    {
        if(currentMana < StatsSystem.instance.currentMana)
        {
            currentMana += manaRegenAmount/200 * Time.deltaTime * StatsSystem.instance.currentMana;
        }
        else
        {
            currentMana = StatsSystem.instance.currentMana;
        }
    }

    void TimerToRegenMana()
    {
        if(timer < cooldownToRegen)
        {
            timer += Time.deltaTime;
        }
        else
        {
            regenMana = true;
        }
    }

    public void AddMana(float percent)
    {
      currentMana += StatsSystem.instance.currentMana * percent;
      if(currentMana > StatsSystem.instance.currentMana)
      {
        currentMana = StatsSystem.instance.currentMana;
      }
    }

    public void AddManaWithStat(float amount)
    {
      currentMana += amount;
      if(currentMana > StatsSystem.instance.currentMana)
      {
        currentMana = StatsSystem.instance.currentMana;
      }
    }

    public void UseMana(float manaCost)
    {
        //manaAudio.Stop();
        follow.fillAmount = image.fillAmount;
        currentMana -= manaCost;
        regenMana = false;
        timer = 0;
        image.fillAmount = currentMana/StatsSystem.instance.currentMana;
        if(!following)
        {
            StartCoroutine(FollowMana());
        }
    }
    public bool HaveManaTo(float manaCost)
    {
        if(manaCost > currentMana)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    IEnumerator FollowMana()
    {
        following = true;
        while(follow.fillAmount > image.fillAmount)
        {
            follow.fillAmount -= followVelocity * Time.deltaTime;
            yield return null;
        }
        following = false;
    }
}

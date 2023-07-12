using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public float popUpTime;
    public float fadeSpeed;
    public Image fill;
    public Image follow;
    HealthSystemEnemy healthEnemy;
    public Image [] images;
    bool fadingOut = false;
    float timeToFadeOut = 0;
    bool following = false;
    bool fading = false;
    public float followSpeed;

    private void Start() {
        healthEnemy = GetComponent<HealthSystemEnemy>();
        foreach (var im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b,0);
        }
        fill.fillAmount = healthEnemy.currentHealth / healthEnemy.MAX_HEALTH;
    }

    public void PopUp()
    {
        if(PlayerPrefs.GetInt("HealthBars",1) == 1)
        {
            follow.fillAmount = fill.fillAmount;
            fill.fillAmount = healthEnemy.currentHealth / healthEnemy.MAX_HEALTH;
            timeToFadeOut = 0;
            if(fill.color.a < 1 && !fading)
            {
                StopAllCoroutines();
                StartCoroutine(FadeIn());
            }
            if(!following)
            {
                StartCoroutine(FollowHealth());
            }
            
        }
    }

    private void Update() {
        //fadeOut
        if(fill.color.a > 0 && !fadingOut && timeToFadeOut > popUpTime)
        {
            fadingOut = true;
            StartCoroutine(FadeOut());
        }
        else
        {
            timeToFadeOut += Time.deltaTime;
        }
    }
    public void DestroyHealthBar()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        fading = true;
        while(fill.color.a < 1)
        {
            foreach (var im in images)
            {
                im.color = new Color(im.color.r,im.color.g,im.color.b, im.color.a + fadeSpeed*2 * Time.deltaTime);
            }
            yield return null;
        }
        foreach (var im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b, 1);
        }
        fading = false;
    }

    IEnumerator FadeOut()
    {
        while(fill.color.a > 0)
        {
            foreach (var im in images)
            {
                im.color = new Color(im.color.r,im.color.g,im.color.b, im.color.a - fadeSpeed * Time.deltaTime);
            }
            yield return null;
        }
        foreach (var im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b, 0);
        }
        fadingOut = false;
    }

    IEnumerator FollowHealth()
    {
        following = true;
        while(follow.fillAmount >= fill.fillAmount)
        {
            follow.fillAmount -= followSpeed * Time.deltaTime;
            yield return null;
        }
        following = false;
    }
}

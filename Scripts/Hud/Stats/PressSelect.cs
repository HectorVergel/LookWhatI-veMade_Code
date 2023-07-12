using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PressSelect : MonoBehaviour
{
    public float alphaSpeed;
    public float stayTime;
    Image image;
    public bool poping = false;
    public bool statsOne;
    GameObject otherExclamation;

    private void OnEnable() {
        image = GetComponent<Image>();
        if(statsOne)
        {
            poping = false;
            image.color = new Color(1,1,1,0);
            var others = FindObjectsOfType<PressSelect>();
            foreach (var item in others)
            {
                if(item != this)
                {
                    otherExclamation = item.gameObject;
                    otherExclamation.SetActive(false);
                }
            }
        }
    }
    private void OnDisable() {
        if(statsOne)
        {
            otherExclamation.SetActive(true);
            otherExclamation.GetComponent<PressSelect>().poping = false;
            otherExclamation.GetComponent<PressSelect>().image.color = new Color(1,1,1,0);
        }
    }
    private void Update() {
        if(PlayerPrefs.GetInt("Points",0) > 0 && !poping)
        {
            poping = true;
            StopAllCoroutines();
            StartCoroutine(FadeIn());
        }

        if(PlayerPrefs.GetInt("Points",0) <= 0 && poping)
        {
            poping = false;
            StopAllCoroutines();
            image.color = new Color(image.color.r,image.color.g,image.color.b, 0);
        }
    }

    IEnumerator FadeIn()
    {
        while(image.color.a < 1)
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b, image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b, 1);
        yield return new WaitForSeconds(stayTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while(image.color.a > 0)
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b, image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b, 0);
        StartCoroutine(FadeIn());
    }
    public void Restart()
    {
        
    }
}

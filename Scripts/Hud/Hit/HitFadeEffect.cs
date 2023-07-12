using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitFadeEffect : MonoBehaviour
{
    Image hitVignette;

    [SerializeField] float maxStayTime;
    [SerializeField] float alphaSpeed;

    void Start()
    {
        
        hitVignette = GetComponent<Image>();
    }

    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        float timer = 0;
        while (hitVignette.color.a < 1)
        {
            hitVignette.color = new Color(hitVignette.color.r, hitVignette.color.g, hitVignette.color.b, hitVignette.color.a + alphaSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        hitVignette.color = new Color(hitVignette.color.r, hitVignette.color.g, hitVignette.color.b, 1);
        while(timer < maxStayTime)
        {
            timer += Time.unscaledDeltaTime;
            yield return null;
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while (hitVignette.color.a > 0)
        {
            hitVignette.color = new Color(hitVignette.color.r, hitVignette.color.g, hitVignette.color.b, hitVignette.color.a - alphaSpeed * Time.unscaledDeltaTime);
            yield return null;
        }
        hitVignette.color = new Color(hitVignette.color.r, hitVignette.color.g, hitVignette.color.b, 0);
        
    }
}

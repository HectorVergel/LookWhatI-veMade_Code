using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipFade : MonoBehaviour
{
    Image image;
    public float alphaSpeed;
    public float stayTime;
    void Start()
    {
        image = GetComponent<Image>();
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        image.color = new Color(1,1,1,0);
        while(image.color.a < 1)
        {
            image.color = new Color(1,1,1, image.color.a + alphaSpeed* Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,1);
        yield return new WaitForSeconds(stayTime);
        while(image.color.a > 0)
        {
            image.color = new Color(1,1,1, image.color.a - alphaSpeed* Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,0);
        StartCoroutine(Fade());
    }
}

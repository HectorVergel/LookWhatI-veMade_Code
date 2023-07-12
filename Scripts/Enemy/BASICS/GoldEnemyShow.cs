using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldEnemyShow : MonoBehaviour
{
    public Image im;
    public TextMeshProUGUI textGold;
    public float fadeSpeed;
    public float waitTime;
    public float offset;

    private void Start() {
        StartCoroutine(FadeIn());
    }
    private void OnDrawGizmos() {
        Gizmos.DrawLine(transform.position,transform.position + new Vector3(offset,0,0));
    }

    IEnumerator FadeIn()
    {
        if(textGold.text.Length == 1)
        {
            
        }
        else if(textGold.text.Length == 2)
        {
            transform.position = transform.position + new Vector3(offset,0,0);
        }
        else
        {
            transform.position = transform.position + new Vector3(offset*2,0,0);
        }



        im.color = new Color(im.color.r,im.color.g,im.color.b, 0);
        textGold.color = new Color(textGold.color.r,textGold.color.g,textGold.color.b, 0);
        while(im.color.a < 1)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b, im.color.a + fadeSpeed * Time.deltaTime);
            textGold.color = new Color(textGold.color.r,textGold.color.g,textGold.color.b, textGold.color.a + fadeSpeed * Time.deltaTime);
            yield return null;
        }
        im.color = new Color(im.color.r,im.color.g,im.color.b, 1);
        textGold.color = new Color(textGold.color.r,textGold.color.g,textGold.color.b, 1);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        while(im.color.a > 0)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b, im.color.a - fadeSpeed * Time.deltaTime);
            textGold.color = new Color(textGold.color.r,textGold.color.g,textGold.color.b, textGold.color.a - fadeSpeed * Time.deltaTime);
            yield return null;
        }
        im.color = new Color(im.color.r,im.color.g,im.color.b, 0);
        textGold.color = new Color(textGold.color.r,textGold.color.g,textGold.color.b, 0);
        Destroy(this.gameObject);
    }
}

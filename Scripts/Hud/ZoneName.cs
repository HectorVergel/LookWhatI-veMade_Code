using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ZoneName : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip zoneTextSound;
    TextMeshProUGUI text;
    public string zoneName;
    public float velocity;
    public float timeStay;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        text = GetComponent<TextMeshProUGUI>();
        StartCoroutine(FadeInAndOut()); //llamar esto cuando el sceneManager acaba de hacer la transicion
    }

    IEnumerator FadeInAndOut()
    {
        audioSource.clip = zoneTextSound;
        //audioSource.Play();
        text.text = zoneName;
        text.color = new Color(text.color.r,text.color.g,text.color.b,0);
        while(text.color.a < 1)
        {
            text.color = new Color(text.color.r,text.color.g,text.color.b,text.color.a + velocity * Time.deltaTime);
            yield return null;
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,1);
        yield return new WaitForSeconds(timeStay);
        while(text.color.a > 0)
        {
            text.color = new Color(text.color.r,text.color.g,text.color.b,text.color.a - velocity * Time.deltaTime);
            yield return null;
        }
        text.color = new Color(text.color.r,text.color.g,text.color.b,0);
    }
}

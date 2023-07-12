using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextDialogue : MonoBehaviour
{
    public static TextDialogue instance;
    public float speed;
    TextMeshProUGUI textDisplay;

    private void Awake() {
        instance = this;
    }
    private void Start() {
        textDisplay = GetComponent<TextMeshProUGUI>();
    }
    public void StartFadeIn(string txt, DialogueManager manager)
    {
        
        textDisplay.text = txt;
        if(PlayerPrefs.GetInt("Subtitles", 1) == 1)
        {
            StartCoroutine(FadeIn(manager));
        }
        else
        {
            manager.fadeIn = false;
            textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,0);
        }
    }
    public void StartFadeOut(DialogueManager manager)
    {
        if(PlayerPrefs.GetInt("Subtitles", 1) == 1)
        {
            StartCoroutine(FadeOut(manager));
        }
        else
        {
            manager.fadeOut = false;
            textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,0);
        }
    }
    IEnumerator FadeIn(DialogueManager manager)
    {
        textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,0);
        while(textDisplay.color.a < 1)
        {
            textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,textDisplay.color.a + speed * Time.deltaTime);
            yield return null;
        }
        textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,1);
        manager.fadeIn = false;
    }

    IEnumerator FadeOut(DialogueManager manager)
    {
        textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,1);
        while(textDisplay.color.a > 0)
        {
            textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,textDisplay.color.a - speed * Time.deltaTime);
            yield return null;
        }
        textDisplay.color = new Color(textDisplay.color.r,textDisplay.color.g,textDisplay.color.b,0);
        manager.fadeOut = false;
    }
}

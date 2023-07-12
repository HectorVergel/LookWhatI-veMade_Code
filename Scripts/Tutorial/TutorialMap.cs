using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialMap : MonoBehaviour
{
    bool goingUp = false;
    public Image image;
    public float alphaSpeed;
    public Color colorDone;
    public bool triggered;
    public GameObject dialogueToPlay;
    bool done;

    private void Start() {
        if(PlayerPrefs.GetInt("Map",0) == 1)
        {
            Destroy(this.gameObject);
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,0);
    }
    public void StartTutorial()
    {
        if(!triggered)
        {
            PlayerPrefs.SetInt("Map",1);
            FindObjectOfType<Hud>().ResetAbiltyUI();
            StopOtherDialogs();
            StartCoroutine(Up());
            PlayDialogue();
        }
    }
    IEnumerator Up()
    {
        triggered = true;
        goingUp = true;
        while(image.color.a < 1)
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,1);
        goingUp = false;
    }
    void StopOtherDialogs()
    {
        var dialogs = FindObjectsOfType<DialogueManager>();
        foreach (var item in dialogs)
        {
            if(item.playing)
            {
                Destroy(item.gameObject);
            }
        }
    }
    void PlayDialogue()
    {
        if(dialogueToPlay != null)
        {
            dialogueToPlay.GetComponent<DialogueManager>().StartDialogue();
        }
    }
    public void Done()
    {
        if(!done)
        {
            done = true;
            image.color = new Color(colorDone.r,colorDone.g,colorDone.b,image.color.a);
            StartCoroutine(Down());
        }
    }

    IEnumerator Down()
    {
        while(goingUp)
        {
            yield return null;
        }
        while(image.color.a > 0)
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,0);
        Destroy(this.gameObject);
    }
}

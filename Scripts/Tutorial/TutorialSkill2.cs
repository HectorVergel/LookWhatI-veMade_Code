using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSkill2 : MonoBehaviour
{
    bool goingUp = false;
    public Image image;
    public float alphaSpeed;
    public Color colorDone;
    public bool triggered;
    TutorialCheck tutorial;
    private void Start() {
        tutorial = GetComponent<TutorialCheck>();
        image.color = new Color(image.color.r,image.color.g,image.color.b,0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !triggered && CheckIfOtherTutorialsRunning())
        {
            StartCoroutine(Up());
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
    
    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player" && !triggered && CheckIfOtherTutorialsRunning())
        {
            StartCoroutine(Up());
            Destroy(GetComponent<BoxCollider2D>());
        }
        
    }
    IEnumerator Up()
    {
        tutorial.actionRunning = true;
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
    bool CheckIfOtherTutorialsRunning()
    {
        if(FindObjectsOfType<TutorialCheck>() != null)
        {
            var tutorials = FindObjectsOfType<TutorialCheck>();
            foreach (var item in tutorials)
            {
                if(item.actionRunning)
                {
                    return false;
                }
            }
            return true;
        }
        else
        {
            return true;
        }
    }
    public void Done()
    {
        image.color = new Color(colorDone.r,colorDone.g,colorDone.b,image.color.a);
        StartCoroutine(Down());
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

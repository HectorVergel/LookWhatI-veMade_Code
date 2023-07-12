using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialConsumables : MonoBehaviour
{
    bool goingUp = false;
    public Image image1;
    public Image image2;
    public Image image3;
    public float alphaSpeed;
    public Color colorDone;
    public bool triggered1;
    public bool triggered2;
    public bool triggered3;
    public bool started;
    public GameObject gate;

    public void AskStart()
    {
        if(!started)
        {
            started =  true;
            StartCoroutine(WaitForOtherTutorials());
        }
    }
    private void Update() {
        ItemDrop[] items = FindObjectsOfType<ItemDrop>();
        if(items.Length < 1 && PlayerPrefs.GetString("CurrentConsumible",null).Length < 1)
        {
            Done2();
        }
    }

    public void StartTutorial()
    {
        triggered1 = true;
        triggered2 = false;
        triggered3 = false;
        StartCoroutine(Up(image1));
    }

    IEnumerator WaitForOtherTutorials()
    {
        while(!CheckIfOtherTutorialsRunning())
        {
            yield return null;
        }
        StartTutorial();
    }

    public void Done1()
    {
        if(triggered1)
        {
            image1.color = new Color(colorDone.r,colorDone.g,colorDone.b,image1.color.a);
            StartCoroutine(Down(image1));
        }
    }
    public void Done2()
    {
        if(triggered2)
        {
            image2.color = new Color(colorDone.r,colorDone.g,colorDone.b,image2.color.a);
            StartCoroutine(Down(image2));
        }
    }
    public void Done3()
    {
        if(triggered3)
        {
            image3.color = new Color(colorDone.r,colorDone.g,colorDone.b,image3.color.a);
            StartCoroutine(Down(image3));
        }
    }

    IEnumerator Up(Image image)
    {
        goingUp = true;
        while(image.color.a < 1)
        {
            image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(image.color.r,image.color.g,image.color.b,1);
        goingUp = false;
    }
    IEnumerator Down(Image image)
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
        triggered1 = false;
        triggered2 = false;
        triggered3 = false;
        if(image == image1)
        {
            triggered2 = true;
            StartCoroutine(Up(image2));
        }
        else if(image == image2)
        {
            triggered3 = true;
            StartCoroutine(Up(image3));
        }
        else
        {
            gate.SetActive(false);
            Destroy(this.gameObject);
        }
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
}

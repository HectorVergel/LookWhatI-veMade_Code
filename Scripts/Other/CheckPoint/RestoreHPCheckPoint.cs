using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RestoreHPCheckPoint : MonoBehaviour
{
    public bool goingUp;
    public float alphaSpeed;
    public Image image;

    private void Start() {
        goingUp = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract += RestoreHp;
            StopAllCoroutines();
            StartCoroutine(Up());
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract -= RestoreHp;
            StartCoroutine(Down());
        } 
    }

    IEnumerator Up()
    {
        goingUp = true;
        while(image.color.a < 1)
        {
            image.color = new Color(1,1,1,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,1);
        goingUp = false;

    }

    IEnumerator Down()
    {
        while(goingUp)
        {
            yield return null;
        }
        while(image.color.a > 0)
        {
            image.color = new Color(1,1,1,image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,0);
    }

    void RestoreHp()
    {
        HealthSystemPlayer.instance.SetMaximumHP();
    }
}

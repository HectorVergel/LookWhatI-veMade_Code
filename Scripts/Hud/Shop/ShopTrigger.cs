using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTrigger : MonoBehaviour
{
    public bool goingUp;
    public float alphaSpeed;
    public Image image;
    AudioSource audioSoruce;
    public AudioClip openShop;

    private void Start() {
        goingUp = false;
        audioSoruce = GetComponent<AudioSource>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract += OpenShop;
            StopAllCoroutines();
            StartCoroutine(Up());
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract -= OpenShop;
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

    void OpenShop()
    {
        InterfaceManager.instance.OpenShop();
        audioSoruce.PlayOneShot(openShop);
    }
}

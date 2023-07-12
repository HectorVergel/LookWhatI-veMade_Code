using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TheExamXP : MonoBehaviour
{
    public Sprite closed;
    public Sprite opened;
    public bool goingUp;
    public float alphaSpeed;
    public Image image;
    public GameObject dialogue;
    SpriteRenderer spriteRenderer;

    private void Start() {
        goingUp = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if(PlayerPrefs.GetInt("Ach5",0) == 1)
        {
            spriteRenderer.sprite = opened;
            dialogue.SetActive(true);
        }
        else
        {
            spriteRenderer.sprite = closed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && PlayerPrefs.GetInt("Ach5",0) == 0)
        {
            PlayerInputs.OnInteract += Interact;
            StopAllCoroutines();
            StartCoroutine(Up());
        }
    }
    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player" && PlayerPrefs.GetInt("Ach5",0) == 0)
        {
            PlayerInputs.OnInteract -= Interact;
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

    void Interact()
    {
        PlayerInputs.OnInteract -= Interact;
        if (GetComponent<AchievementPopUp>() != null)
        {
            GetComponent<AchievementPopUp>().Activate();
        }
        spriteRenderer.sprite = opened;
        StartCoroutine(Down());
        dialogue.SetActive(true);
    }
}

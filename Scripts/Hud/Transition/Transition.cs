using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition : MonoBehaviour
{
    public Image finalImage;

    public static Transition instance;
    public Sprite[] deadSprites;
    public Sprite[] transitionSprites;

    private void Awake()
    {
        instance = this;
    }

    public void ChooseDead()
    {
        int rnd = Random.Range(0, deadSprites.Length);

        finalImage.sprite = deadSprites[rnd];
    }

    public void ChooseTransition()
    {
        int rnd = Random.Range(0, transitionSprites.Length);

        finalImage.sprite = transitionSprites[rnd];
    }
}

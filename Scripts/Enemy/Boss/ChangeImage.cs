using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeImage : MonoBehaviour
{
    public bool dead;
    private Image image;
    public Sprite deadImage;

    private void Start()
    {
        image = GetComponent<Image>();
    }
    void Update()
    {
        if (dead && image.sprite != deadImage)
        {
            image.sprite = deadImage;
        }
    }
}

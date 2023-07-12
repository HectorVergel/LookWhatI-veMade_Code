using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSpriteCinematic : MonoBehaviour
{
    public Sprite[] sprites;

    public void ChangeSprite(int idx)
    {
        GetComponent<SpriteRenderer>().sprite = sprites[idx];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarPortions : MonoBehaviour
{
    public Sprite[] sprites = new Sprite[11];
    public string nameStat;
    Image image;
    private void OnEnable() {
        image = GetComponent<Image>();
        SetPortions();
    }

    public void SetPortions()
    {
        image.sprite = sprites[PlayerPrefs.GetInt(nameStat,0)];
    }
}

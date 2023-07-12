using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndFillFollow : MonoBehaviour
{
    public Image parent;
    RectTransform image;
    float lastFillamount;
    void Start()
    {
        image = GetComponent<RectTransform>();
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if(parent.fillAmount != lastFillamount)
        {
            SetPosition();
        }
    }

    void SetPosition()
    {
        image.anchorMax = new Vector2(parent.fillAmount,image.anchorMax.y);
        image.anchorMin = new Vector2(parent.fillAmount,image.anchorMin.y);
        image.anchoredPosition = new Vector2(0,0);
        lastFillamount = parent.fillAmount;
    }
}

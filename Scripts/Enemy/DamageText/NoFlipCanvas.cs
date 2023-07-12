using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoFlipCanvas : MonoBehaviour
{
    RectTransform rect;
    Quaternion rotation;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        rotation = rect.rotation;
    }
    void LateUpdate()
    {
        rect.rotation = rotation;
    }
}

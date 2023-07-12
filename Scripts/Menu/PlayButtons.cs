using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayButtons : MonoBehaviour,ISelectHandler,IPointerEnterHandler,IPointerExitHandler,IDeselectHandler
{
    public float initialWidth;
    public float initialHeight;
    public float finalWidth;
    public float finalHeight;
    public RectTransform[] textRect;
    public RectTransform imageRect;
    public float textBigScale;
    private void OnEnable() {
        ResetSize();
    }
    public void ResetSize()
    {
        //tamaño normal
        GetComponent<RectTransform>().sizeDelta = new Vector2(initialWidth,initialHeight);
        foreach (var item in textRect)
        {
            item.localScale = new Vector3(1,1,1);
        }
        imageRect.localScale = new Vector3(1,1,1);
    }

    void SetSize()
    {
        //tamaño grande
        GetComponent<RectTransform>().sizeDelta = new Vector2(finalWidth,finalHeight);
        foreach (var item in textRect)
        {
            item.localScale = new Vector3(textBigScale,textBigScale,textBigScale);
        }
        imageRect.localScale = new Vector3(textBigScale,textBigScale,textBigScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetSize();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlayButtons[] otherButtons = FindObjectsOfType<PlayButtons>();
        foreach (var item in otherButtons)
        {
            item.ResetSize();
        }
        SetSize();
    }

    public void OnSelect(BaseEventData eventData)
    {
        PlayButtons[] otherButtons = FindObjectsOfType<PlayButtons>();
        foreach (var item in otherButtons)
        {
            item.ResetSize();
        }
        SetSize();
    }
    public void OnUnselect(BaseEventData eventData)
    {
        PlayButtons[] otherButtons = FindObjectsOfType<PlayButtons>();
        foreach (var item in otherButtons)
        {
            item.ResetSize();
        }
        SetSize();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        ResetSize();
    }
}

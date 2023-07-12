using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldToUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Vector2 uiOffset;
    public Vector3 spawnPoint;
    RectTransform canvasRect;
    void Start()
    {
        // Get the rect transform
        this.rectTransform = GetComponent<RectTransform>();
        canvasRect = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        // Calculate the screen offset
        this.uiOffset = new Vector2((float)canvasRect.sizeDelta.x / 2f, (float)canvasRect.sizeDelta.y / 2f);
        MoveToPoint(spawnPoint);
        transform.SetAsFirstSibling();
    }

    
    public void MoveToPoint(Vector3 objectTransformPosition)
    {
        // Get the position on the canvas
        Vector2 ViewportPosition = Camera.main.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * canvasRect.sizeDelta.x, ViewportPosition.y * canvasRect.sizeDelta.y);

        // Set the position and remove the screen offset
        this.rectTransform.localPosition = proportionalPosition - uiOffset;
    }
}

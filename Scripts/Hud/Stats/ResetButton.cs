using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class ResetButton : MonoBehaviour,ISelectHandler,IPointerEnterHandler, IPointerExitHandler
{
    public GameObject description;
    public float scalePlus;
    public float initScale;
    void ISelectHandler.OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            var nextNumbers = FindObjectsOfType<ButonsSelected>();
            foreach (var item in nextNumbers)
            {
                item.DesactivateLabel();
            }
            description.SetActive(true);
            transform.localScale = new Vector3(initScale + scalePlus,0.8f + scalePlus,initScale + scalePlus);
        }
        else
        {
            StartCoroutine(Unselect());
        }
    }
    IEnumerator Unselect()
    {
        yield return new WaitForFixedUpdate();
        EventSystem.current.SetSelectedGameObject(null);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            item.DesactivateLabel();
        }
        description.SetActive(true);
        transform.localScale = new Vector3(initScale + scalePlus,0.8f + scalePlus,initScale + scalePlus);
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            item.DesactivateLabel();
        }
        DesactivateAll();
    }

    public void DesactivateAll()
    {
        description.SetActive(false);
        transform.localScale = new Vector3(initScale,0.8f,initScale);
    }
}

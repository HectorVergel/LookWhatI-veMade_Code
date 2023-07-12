using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class ButonsSelected : MonoBehaviour,ISelectHandler,IPointerEnterHandler, IPointerExitHandler
{
    public string flechita;
    public string stat;
    public GameObject nextNumber;
    public GameObject description;
    public float scalePlus;
    public float initScale;

    
    void ISelectHandler.OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            Check();
           
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
        Check();
       
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            item.DesactivateLabel();
        }
        ResetButton reset = FindObjectOfType<ResetButton>();
        reset.DesactivateAll();
    }

    void PutNumber() 
    {
        if(stat == "Health")
        {
            nextNumber.GetComponent<TextMeshProUGUI>().text = flechita + (StatsSystem.instance.currentHP + (StatsSystem.instance.maxHealth-StatsSystem.instance.initialHealth)/StatsSystem.instance.maxLevels).ToString();
        }
        else if(stat == "Strength")
        {
            if(Character.currentType == CharacterType.Warrior)
            {
                nextNumber.GetComponent<TextMeshProUGUI>().text = flechita +(StatsSystem.instance.currentStrength + (StatsSystem.instance.maxStrength-StatsSystem.instance.initialStrength)/StatsSystem.instance.maxLevels).ToString();
            }
            else
            {
                nextNumber.GetComponent<TextMeshProUGUI>().text = flechita + (1 + Mathf.RoundToInt((PlayerPrefs.GetInt("Strength",0) + 1) * 0.5f + 0.01f)).ToString();
            }
            
        }
        else if(stat == "Mana")
        {
            nextNumber.GetComponent<TextMeshProUGUI>().text = flechita +(StatsSystem.instance.currentMana + (StatsSystem.instance.maxMana-StatsSystem.instance.initialMana)/StatsSystem.instance.maxLevels).ToString();
        }
        else
        {
            float value = (StatsSystem.instance.currentLuck + (StatsSystem.instance.maxLuck-StatsSystem.instance.initialLuck)/StatsSystem.instance.maxLevels) *100;
            double result = ((int)( 10 * value )) / 10;
            nextNumber.GetComponent<TextMeshProUGUI>().text = flechita +((float)result).ToString() + "%";
        }
    }
    public void DesactivateLabel()
    {
        if(nextNumber != null)
        {
            nextNumber.SetActive(false);
        }
        if(description != null)
        {
            description.SetActive(false);
        }
        transform.localScale = new Vector3(initScale,initScale,initScale);
    }
    void ActivateLabel()
    {
        nextNumber.SetActive(true);
        PutNumber();
    }
    public void Check()
    {

        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            item.DesactivateLabel();
        }
        ResetButton reset = FindObjectOfType<ResetButton>();
        reset.DesactivateAll();

        if(PlayerPrefs.GetInt(stat,0) < StatsSystem.instance.maxLevels && PlayerPrefs.GetInt("Points",0) > 0)
        {
            ActivateLabel();
        }
        else
        {
            DesactivateLabel();
        }
        description.SetActive(true);
        transform.localScale = new Vector3(initScale + scalePlus, initScale + scalePlus, initScale + scalePlus);
    }
}

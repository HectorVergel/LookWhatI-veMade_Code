using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class ButonSelectedShop : MonoBehaviour,ISelectHandler,IPointerEnterHandler
{
    public string nombre;
    [TextArea(5,5)]
    public string description;
    public int cost;
    public PotionType type;
    public TextMeshProUGUI price;
    public Image imagen;
    public RectTransform child;
    string namePotion;
    private void Start() 
    {
        switch (type)
        {
            case PotionType.HP1:
            namePotion = Consumables.instance.potionHP1;
            break;

            case PotionType.HP2:
            namePotion = Consumables.instance.potionHP2;
            break;

            case PotionType.HP3:
            namePotion = Consumables.instance.potionHP3;
            break;

            case PotionType.MANA:
            namePotion = Consumables.instance.potionMana;
            break;

            case PotionType.TP:
            namePotion = Consumables.instance.potionTP;
            break;

            case PotionType.EXP:
            namePotion = Consumables.instance.potionXP;
            break;

            default:
            break;
        }

        price.text = cost.ToString();

    }
    private void OnEnable() {
        if(type == PotionType.HP1)
        {
            StartCoroutine(BugSelectFix());
        }
    }
    IEnumerator BugSelectFix()
    {
        yield return new WaitForFixedUpdate();
        MakeAll();
    }
    void ISelectHandler.OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            MakeAll();
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
        MakeAll();
    }
    void MakeAll()
    {
        SetCurrentConsumable();
        var butons = FindObjectsOfType<ButonSelectedShop>();
        foreach (var buton in butons)
        {
            buton.ResetSize();
        }
        SetSize();
    }
    public void Buy()
    {
        Shop.instance.BuyConsumable(namePotion,cost);
    }
    void SetCurrentConsumable()
    {
        Shop.instance.SetCurrentConsumable(nombre,description,imagen.sprite,cost);
    }
    public void ResetSize()
    {
        //tamaño normal
        GetComponent<RectTransform>().sizeDelta = new Vector2(25,30);
        child.localScale = new Vector3(1,1,1);
    }

    void SetSize()
    {
        //tamaño grande
        GetComponent<RectTransform>().sizeDelta += new Vector2(5,5);
        child.localScale = new Vector3(1.2f,1.2f,1.2f);
    }
}

public enum PotionType
{
    HP1,
    HP2,
    HP3,
    MANA,
    TP,
    EXP
}



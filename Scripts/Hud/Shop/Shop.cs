using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Shop : MonoBehaviour
{
    ConsumablesInventory cons;
    public static Shop instance;
    public TextMeshProUGUI title;
    public TextMeshProUGUI descript;
    public TextMeshProUGUI price;
    public Color buyColor;
    public Image photo;
    public GameObject firstButton;
    public GameObject priceTransformer;
    public float velocity;
    public float maxScale;
    public bool changingSize = false;
    public int numberOfXPConsumibles;
    public RectTransform soldOut;
    bool changingSize2 = false;
    float initScale;
    CoinsShop coinsShop;
    public AudioSource buttonsAudio;
    public AudioSource effectsAudio;
    public AudioClip noMoneySound;
    public AudioClip buySound;
    public AudioClip changeSelected;


    private void Awake() {
        instance = this;
    }

    private void Start() {
        cons = GetComponent<ConsumablesInventory>();
        coinsShop = GetComponent<CoinsShop>();
        initScale = priceTransformer.transform.localScale.x;
    }
    private void OnEnable() {
        PlayerInputs.OnBack += CloseShop;
        StartCoroutine(HighlightFirstButton(firstButton));
        Cursor.visible = true;
        Shake.instance.StopShake();
        InterfaceManager.instance.interfaceActive = true;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        soldOut.localScale = new Vector3(1,1,1);
        priceTransformer.transform.localScale = new Vector3(1,1,1);
        price.color = new Color(1,1,1,1);

    }
    private void OnDisable() {
        Cursor.visible = false;
        InterfaceManager.instance.interfaceActive = false;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        PlayerInputs.OnBack -= CloseShop; 
    }
    void CloseShop()
    {
        gameObject.SetActive(false);
    }
    void PlaySound(AudioClip sound)
    {
        effectsAudio.pitch = 1f + Random.Range(-0.1f,0.1f);
        effectsAudio.PlayOneShot(sound);
    }
    void PlaySoundNoPitch(AudioClip sound)
    {
        effectsAudio.pitch = 1f + Random.Range(-0.1f,0f);
        effectsAudio.PlayOneShot(sound);
    }
    
    IEnumerator HighlightFirstButton(GameObject button)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForFixedUpdate();
            EventSystem.current.SetSelectedGameObject(button);
        }
    }
    public void SetCurrentConsumable(string name, string description, Sprite foto, int cost)
    {
        buttonsAudio.pitch = 1f + Random.Range(-0.1f,0.1f);
        buttonsAudio.PlayOneShot(changeSelected);
        title.text = name;
        descript.text = description;
        photo.sprite = foto;
        price.text = cost.ToString();
    }

    public void BuyConsumable(string name, int amount)
    {
        if(Coins.instance.CheckIfHaveCoins(amount))
        {
            if(name == Consumables.instance.potionXP)
            {
                if(PlayerPrefs.GetInt("XPConsumible",numberOfXPConsumibles) > 0)
                {
                    Consumables.instance.GiveConsumable(name);
                    Coins.instance.RemoveCoins(amount);
                    cons.SetAll();
                    coinsShop.SetCoins();
                    coinsShop.RemoveCoins(amount);
                    StartCoroutine(ChangeSize());
                    PlayerPrefs.SetInt("XPConsumible",PlayerPrefs.GetInt("XPConsumible",numberOfXPConsumibles) - 1);
                    PlaySoundNoPitch(buySound);
                    if(PlayerPrefs.GetInt("XPConsumible",numberOfXPConsumibles) == 0 && GetComponent<AchievementPopUp>() != null)
                    {
                        GetComponent<AchievementPopUp>().Activate();
                    }
                }
                else if(!changingSize2)
                {
                    StartCoroutine(ChangeSizeSoldOut());
                    PlaySound(noMoneySound);
                }
            }
            else
            {
                Consumables.instance.GiveConsumable(name);
                Coins.instance.RemoveCoins(amount);
                cons.SetAll();
                coinsShop.SetCoins();
                coinsShop.RemoveCoins(amount);
                StartCoroutine(ChangeSize());
                PlaySoundNoPitch(buySound);
            }
        }
        else
        {
            PlaySound(noMoneySound);
            coinsShop.RedCoins();
        }
    }

    IEnumerator ChangeSize()
    {
        while(changingSize)
        {
            yield return null;
        }
        changingSize = true;
        price.color = buyColor;
        priceTransformer.transform.localScale = new Vector3(1,1,1);
        while(priceTransformer.transform.localScale.x < maxScale)
        {
            priceTransformer.transform.localScale = priceTransformer.transform.localScale + (new Vector3(velocity,velocity,velocity) * Time.deltaTime);
            yield return null;
        }
        priceTransformer.transform.localScale = new Vector3(maxScale,maxScale,maxScale);
        while(priceTransformer.transform.localScale.x > initScale)
        {
            priceTransformer.transform.localScale = priceTransformer.transform.localScale - (new Vector3(velocity,velocity,velocity) * Time.deltaTime);
            yield return null;
        }
        priceTransformer.transform.localScale = new Vector3(1,1,1);
        price.color = new Color(1,1,1,1);
        changingSize = false;
    }
    IEnumerator ChangeSizeSoldOut()
    {
        while(changingSize2)
        {
            yield return null;
        }
        changingSize2 = true;
        soldOut.localScale = new Vector3(1,1,1);
        while(soldOut.localScale.x < maxScale)
        {
            soldOut.localScale = soldOut.localScale + (new Vector3(velocity,velocity,velocity) * Time.deltaTime);
            yield return null;
        }
        soldOut.localScale = new Vector3(maxScale,maxScale,maxScale);
        while(soldOut.localScale.x > 1)
        {
            soldOut.localScale = soldOut.localScale - (new Vector3(velocity,velocity,velocity) * Time.deltaTime);
            yield return null;
        }
        soldOut.localScale = new Vector3(1,1,1);
        changingSize2 = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Consumables : MonoBehaviour
{
    public static Consumables instance;
    public Image consumableImage;
    public TextMeshProUGUI amount;
    public Sprite hp1;
    public Sprite hp2;
    public Sprite hp3;
    public Sprite mana;
    public Sprite tp;
    public Sprite exp;

    public int health1HP;
    public int health2HP;
    public int health3HP;
    public string potionHP1;
    public string potionHP2;
    public string potionHP3;
    public string potionMana;
    public string potionTP;
    public string potionXP;
    string currentConsumible;
    AudioSource audioSource;
    public AudioClip useSound;

    [SerializeField] Color potionHPColor;
    [SerializeField] Color potionManaColor;
    [SerializeField] Color potionXPColor;
    [SerializeField] Color potionTPColor;

   

    ParticleSystem consumableVFX;
    ParticleSystem consumableVFX1;

    private void Awake() {
        instance = this;
        
    }
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentConsumible = PlayerPrefs.GetString("CurrentConsumible",null);
        RefreshDisplay();
    }

    private void OnEnable() {
        PlayerInputs.OnUseConsumable += Use;
        PlayerInputs.OnLeft += DoLeft;
        PlayerInputs.OnRight += DoRight;
    }
    private void OnDisable() {
        PlayerInputs.OnUseConsumable -= Use;
        PlayerInputs.OnLeft -= DoLeft;
        PlayerInputs.OnRight -= DoRight;
    }

    public void GiveConsumable(string nameOfConsumable)
    {
        PlayerPrefs.SetInt(nameOfConsumable,PlayerPrefs.GetInt(nameOfConsumable,0) + 1);
        currentConsumible = nameOfConsumable;
        RefreshDisplay();
    }

    void PlaySound(AudioClip clip)
    {
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(clip);
    }
    public void Use()
    {
        var vfxController = FindObjectOfType<ConsumableParticles>();
        consumableVFX = vfxController.vfxCristals;
        switch (currentConsumible)
        {
            case var value when value == potionHP1:
                consumableVFX1 = vfxController.vfxRed;
                Health1();
                PlaySound(useSound);
            break;


            case var value when value == potionHP2:
                consumableVFX1 = vfxController.vfxRed;
                Health2();
                PlaySound(useSound);
            break;


            case var value when value == potionHP3:
                consumableVFX1 = vfxController.vfxRed;
                Health3();
                PlaySound(useSound);
            break;


            case var value when value == potionMana:
                consumableVFX1 = vfxController.vfxBlue;
                ManaFull();
                PlaySound(useSound);
            break;


            case var value when value == potionTP:
                consumableVFX1 = vfxController.vfxPurple;
                TP();
                PlaySound(useSound);
            break;


            case var value when value == potionXP:
                if(FindObjectOfType<TutorialConsumables>() != null)
                {
                    FindObjectOfType<TutorialConsumables>().Done2();
                }
                consumableVFX1 = vfxController.vfxYellow;
                LevelUp();
                PlaySound(useSound);
            break;


            default:
            break;
        }

       
        if (PlayerPrefs.GetInt(currentConsumible,0) <= 0)
        {
            DoRight();
        }
        else
        {
            RefreshDisplay();
        }
    }
    public void DoLeft()
    {
        if(FindObjectOfType<TutorialConsumables>() != null)
        {
            FindObjectOfType<TutorialConsumables>().Done1();
        }
        bool a1 = PlayerPrefs.GetInt(potionHP1,0) > 0;
        bool a2 = PlayerPrefs.GetInt(potionHP2,0) > 0;
        bool a3 = PlayerPrefs.GetInt(potionHP3,0) > 0;
        bool a4 = PlayerPrefs.GetInt(potionMana,0) > 0;
        bool a5 = PlayerPrefs.GetInt(potionTP,0) > 0;
        bool a6 = PlayerPrefs.GetInt(potionXP,0) > 0;
        if(a1 || a2 || a3 || a4 || a5 || a6)
        {
            if(PassLeft())
            {
                RefreshDisplay();
            }
        }
        else
        {
            currentConsumible = null;
            RefreshDisplay();
        }
    }
    public bool PassLeft()
    {
        switch (currentConsumible)
        {
            case var value when value == potionHP1:
                currentConsumible = potionXP;
            break;


            case var value when value == potionHP2:
                currentConsumible = potionHP1;
            break;


            case var value when value == potionHP3:
                currentConsumible = potionHP2;
            break;


            case var value when value == potionMana:
                currentConsumible = potionHP3;
            break;


            case var value when value == potionTP:
                currentConsumible = potionMana;
            break;


            case var value when value == potionXP:
                currentConsumible = potionTP;
            break;


            default:
            break;
        }

        if(PlayerPrefs.GetInt(currentConsumible,0) > 0)
        {
            return true;
        }
        else
        {
            return PassLeft();
        }
    }
    public bool PassRight()
    {
        switch (currentConsumible)
        {
            case var value when value == potionHP1:
                currentConsumible = potionHP2;
            break;


            case var value when value == potionHP2:
                currentConsumible = potionHP3;
            break;


            case var value when value == potionHP3:
                currentConsumible = potionMana;
            break;


            case var value when value == potionMana:
                currentConsumible = potionTP;
            break;


            case var value when value == potionTP:
                currentConsumible = potionXP;
            break;


            case var value when value == potionXP:
                currentConsumible = potionHP1;
            break;


            default:
            break;
        }

        if(PlayerPrefs.GetInt(currentConsumible,0) > 0)
        {
            return true;
        }
        else
        {
            return PassRight();
        }
    }
    public void DoRight()
    {
        if(FindObjectOfType<TutorialConsumables>() != null)
        {
            FindObjectOfType<TutorialConsumables>().Done1();
        }
        bool a1 = PlayerPrefs.GetInt(potionHP1,0) > 0;
        bool a2 = PlayerPrefs.GetInt(potionHP2,0) > 0;
        bool a3 = PlayerPrefs.GetInt(potionHP3,0) > 0;
        bool a4 = PlayerPrefs.GetInt(potionMana,0) > 0;
        bool a5 = PlayerPrefs.GetInt(potionTP,0) > 0;
        bool a6 = PlayerPrefs.GetInt(potionXP,0) > 0;
        if(a1 || a2 || a3 || a4 || a5 || a6)
        {
            if(PassRight())
            {
                RefreshDisplay();
            }
        }
        else
        {
            currentConsumible = null;
            RefreshDisplay();
        }
    }
    void Health1()
    {
        HealthSystemPlayer.instance.AddHPWithStat(health1HP);
        PlayerPrefs.SetInt(potionHP1,PlayerPrefs.GetInt(potionHP1,0) - 1);
        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionHPColor;

        consumableVFX.Play();
        consumableVFX1.Play();
        
        
    }

    void Health2()
    {
        HealthSystemPlayer.instance.AddHPWithStat(health2HP);
        PlayerPrefs.SetInt(potionHP2,PlayerPrefs.GetInt(potionHP2,0) - 1);

        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionHPColor;

      
        consumableVFX.Play();
        consumableVFX1.Play();
        
    }

    void Health3()
    {
        HealthSystemPlayer.instance.AddHPWithStat(health3HP);
        PlayerPrefs.SetInt(potionHP3,PlayerPrefs.GetInt(potionHP3,0) - 1);
        consumableVFX.Play();
        consumableVFX1.Play();
        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionHPColor;

        
    }

    void ManaFull()
    {
        Mana.instance.MaxMana();
        PlayerPrefs.SetInt(potionMana,PlayerPrefs.GetInt(potionMana,0) - 1);
        consumableVFX.Play();
        consumableVFX1.Play();
        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionManaColor;


    }

    void TP()
    {
        switch (PlayerPrefs.GetInt("ShopNumber",1))
        {
            case 1:
            PlayerPrefs.SetInt("CheckPoint",1);
            break;

            case 2:
            PlayerPrefs.SetInt("CheckPoint",5);
            break;

            case 3:
            PlayerPrefs.SetInt("CheckPoint",6);
            break;

            default:
            break;
        }
        LoadLevel.instance.RespawnDie();
        PlayerPrefs.SetInt(potionTP,PlayerPrefs.GetInt(potionTP,0) - 1);
        consumableVFX.Play();
        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionTPColor;
    }

    void LevelUp()
    {
        Experience.instance.Add1Level();
        PlayerPrefs.SetInt(potionXP,PlayerPrefs.GetInt(potionXP,0) - 1);
        consumableVFX.Play();
        consumableVFX1.Play();
        var mainParticle = consumableVFX.main;
        mainParticle.startColor = potionXPColor;

        
    }

    public void RefreshDisplay()
    {
        switch (currentConsumible)
        {
            case var value when value == potionHP1:
                consumableImage.sprite = hp1;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionHP1,0).ToString();
            break;


            case var value when value == potionHP2:
                consumableImage.sprite = hp2;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionHP2,0).ToString();
            break;


            case var value when value == potionHP3:
                consumableImage.sprite = hp3;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionHP3,0).ToString();
            break;


            case var value when value == potionMana:
                consumableImage.sprite = mana;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionMana,0).ToString();
            break;


            case var value when value == potionTP:
                consumableImage.sprite = tp;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionTP,0).ToString();
            break;


            case var value when value == potionXP:
                consumableImage.sprite = exp;
                consumableImage.color = new Color(1,1,1,1);
                amount.text = PlayerPrefs.GetInt(potionXP,0).ToString();
            break;


            default:
                consumableImage.sprite = null;
                consumableImage.color = new Color(1,1,1,0);
                amount.text = "";
            break;
        }
        PlayerPrefs.SetString("CurrentConsumible",currentConsumible);
    }

    public bool CheckHP1()
    {
        if(PlayerPrefs.GetInt(potionHP1,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckHP2()
    {
        if(PlayerPrefs.GetInt(potionHP2,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckHP3()
    {
        if(PlayerPrefs.GetInt(potionHP3,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckMana()
    {
        if(PlayerPrefs.GetInt(potionMana,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckTP()
    {
        if(PlayerPrefs.GetInt(potionTP,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public bool CheckXP()
    {
        if(PlayerPrefs.GetInt(potionXP,0) <= 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}

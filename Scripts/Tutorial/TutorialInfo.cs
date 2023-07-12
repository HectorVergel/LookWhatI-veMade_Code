using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInfo : MonoBehaviour
{
    public static TutorialInfo instance;
    bool normalAttack;
    bool fullCombo;
    bool ability1;
    bool ability2;
    bool airDash;
    bool roll;
    bool jump;
    bool airAttack;
    bool stats;

    public GameObject attackImage;
    public GameObject fullComboImage;
    public GameObject ability1Image;
    public GameObject ability2Image;
    public GameObject airdashImage;
    public GameObject rollImage;
    public GameObject jumpImage;
    public GameObject airattackImage;
    public GameObject statsImage;


    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        ResetTutorial();
    }
    // Update is called once per frame
    void Update()
    {
        CheckVariables();        
    }

    public void ResetTutorial()
    {
        stats = false;
        normalAttack = false;
        if(Character.currentType == CharacterType.Warrior)
        {
            fullCombo = false;
        }
        else
        {
            fullCombo = true;
        }
        ability1 = false;
        ability2 = false;
        airDash = false;
        roll = false;
        jump = false;
        airAttack = false;

        SetImages();
    }
    void CheckVariables()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            if(MeleCombat.instance.attackTutorial)
            {
                normalAttack = true;
                SetImages();
            }
            if(MeleCombat.instance.airAttackTutorial)
            {
                airAttack = true;
                SetImages();
            }
            if(MeleCombat.instance.fullComboTutorial)
            {
                fullCombo = true;
                SetImages();
            }
        }
        else
        {
            if(WizardCombat.instance.attackTutorial)
            {
                normalAttack = true;
                SetImages();
            }
            if(WizardCombat.instance.airAttackTutorial)
            {
                airAttack = true;
                SetImages();
            }
            if(WizardCombat.instance.fullComboTutorial)
            {
                fullCombo = true;
                SetImages();
            }
        }
        if(Ability1Manager.instance.doAbility)
        {
            ability1 = true;
            SetImages();
        }
        if(Ability2Manager.instance.doAbility)
        {
            ability2 = true;
            SetImages();
        }
        if(PlayerJump.instance.isJumping)
        {
            jump = true;
            SetImages();
        }
        if(PlayerRoll.instance.doingRoll)
        {
            roll = true;
            SetImages();
        }
        if(PlayerDash.instance.doingAirDash)
        {
            airDash = true;
            SetImages();
        }
    }

    public void SetStats()
    {
        stats = true;
        SetImages();
    }
    void SetImages()
    {
        if(stats)
        {
            statsImage.SetActive(false);
        }
        else
        {
            statsImage.SetActive(true);
        }
        if(normalAttack)
        {
            attackImage.SetActive(false);
        }
        else
        {
            attackImage.SetActive(true);
        }

        if(airAttack)
        {
            airattackImage.SetActive(false);
        }
        else
        {
            airattackImage.SetActive(true);
        }

        if(fullCombo)
        {
            fullComboImage.SetActive(false);
        }
        else
        {
            fullComboImage.SetActive(true);
        }

        if(ability1)
        {
            ability1Image.SetActive(false);
        }
        else
        {
            ability1Image.SetActive(true);
        }

        if(ability2)
        {
            ability2Image.SetActive(false);
        }
        else
        {
            ability2Image.SetActive(true);
        }

        if(jump)
        {
            jumpImage.SetActive(false);
        }
        else
        {
            jumpImage.SetActive(true);
        }

        if(roll)
        {
            rollImage.SetActive(false);
        }
        else
        {
            rollImage.SetActive(true);
        }

        if(airDash)
        {
            airdashImage.SetActive(false);
        }
        else
        {
            airdashImage.SetActive(true);
        }
    }
    
}

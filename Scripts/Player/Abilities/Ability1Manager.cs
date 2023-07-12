using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability1Manager : MonoBehaviour
{
    // Start is called before the first frame update
   public bool bloqued;
   public static Ability1Manager instance;
    public bool doAbility = false;
    public static Action  OnAbility1Warrior;
    public static Action  OnAbility1Wizard;
    public string skill1Name;
    

    private void Awake() 
    {
        instance = this;
    }

    
    private void OnEnable()
    {
        PlayerInputs.OnAbility1 += DoCharacterAbility1;
        bloqued = false;
    }
    private void OnDisable()
    {
        PlayerInputs.OnAbility1 -= DoCharacterAbility1;
    }

    private void Update()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            doAbility = Ability1Warrior.instance.doAbility1;
        }
        else
        {
            doAbility = Ability1Wizard.instance.doAbility1;
        }
    }
    public void DoCharacterAbility1()
    {
        if(!bloqued)
        {
            if(Character.currentType == CharacterType.Warrior && PlayerPrefs.GetInt(skill1Name,0) == 1)
            {
                OnAbility1Warrior?.Invoke();
            }
            else if(Character.currentType == CharacterType.Wizard && PlayerPrefs.GetInt(skill1Name,0) == 1)
            {
                OnAbility1Wizard?.Invoke();
            }
        }
    }


    
}




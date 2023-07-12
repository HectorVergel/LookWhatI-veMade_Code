using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Ability2Manager : MonoBehaviour
{
    public bool bloqued;
    public static Ability2Manager instance;
    public bool doAbility = false;
    public static Action  OnAbility2Warrior;
    public static Action  OnAbility2Wizard;
    public string skill2Name;

    private void Awake() 
    {
        instance = this;
    }
    private void OnEnable()
    {
        PlayerInputs.OnAbility2 += DoCharacterAbility2;
        bloqued = false;
    }
    private void OnDisable()
    {
        PlayerInputs.OnAbility2 -= DoCharacterAbility2;
    }

    private void Update()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            doAbility = Ability2Warrior.instance.doAbility2;
        }
        else
        {
            doAbility = Ability2Wizard.instance.doAbility;
        }
    }
    public void DoCharacterAbility2()
    {
        if(!bloqued)
        {
            if(Character.currentType == CharacterType.Warrior && PlayerPrefs.GetInt(skill2Name,0) == 1)
            {
                OnAbility2Warrior?.Invoke();
            }
            else if(Character.currentType == CharacterType.Wizard && PlayerPrefs.GetInt(skill2Name,0) == 1)
            {
                OnAbility2Wizard?.Invoke();
            }
        }
    }
}

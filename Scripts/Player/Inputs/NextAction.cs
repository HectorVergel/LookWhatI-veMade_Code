using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextAction : MonoBehaviour
{
    public float minimumHeight;
    public static NextAction instance;
    public bool nextDoAirAttack = false;
    public bool nextDoComboAttack = false;
    public bool nextDoJump = false;
    public bool nextDoAirDash = false;
    public bool nextDoAbility1 = false;
    public bool nextDoAbility2 = false;
    private void Start() 
    {
        instance = this;
    }
    public void SetNext()
    {
        ResetNextDo();
    }
    public void ResetNextDo()
    {
        nextDoAirAttack = false;
        nextDoComboAttack = false;
        nextDoJump = false;
        nextDoAirDash = false;
        nextDoAbility1 = false;
        nextDoAbility2 = false;
    }
}



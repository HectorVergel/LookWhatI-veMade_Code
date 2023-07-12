using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpUp : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            MeleCombat.instance.isAttacking = false;
        }
        else
        {
            WizardCombat.instance.isAttacking = false;
        }
        PlayerDash.instance.airDashed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerDash.instance.HaveSomethingAtTop())
        {
            animator.Play("goingDown");
        }
        if (!PlayerDash.instance.CanIAirDash())
        {
            PlayerDash.instance.airDashed = false;
        }
        if(Character.currentType == CharacterType.Warrior)
        {
            if (MeleCombat.instance.isAttacking && MeleCombat.instance.airCombo)
            {
                animator.Play("airAttack1");
            }
        }
        else
        {
            if (WizardCombat.instance.isAttacking)
            {
                animator.Play("airAttack1");
            }
        }
        if (PlayerDash.instance.airDashed && PlayerDash.instance.CanIAirDash())
        {
            animator.Play("airDash");
        }
        if (PlayerJump.instance.OnZenit())
        {
            animator.Play("jumpZenit");
        }
        if(PlayerJump.instance.CheckIfFalled())
        {
            animator.Play("goingDown");
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

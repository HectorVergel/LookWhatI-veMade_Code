using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAndRun : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerDash.instance.airDashed = false;
        PlayerRoll.instance.rolled = false;
        if(Character.currentType == CharacterType.Wizard)
        {
            Ability1Wizard.instance.doAbility1 = false;
            Ability2Wizard.instance.doAbility = false;
            WizardCombat.instance.isAttacking = false;
        }
        else
        {
            Ability1Warrior.instance.doAbility1 = false;
            Ability2Warrior.instance.doAbility2 = false;
            MeleCombat.instance.isAttacking = false;
        }
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            if(MeleCombat.instance.isAttacking)
            {
                animator.Play("attack1");
            }
        }
        else
        {
            if(WizardCombat.instance.isAttacking)
            {
                animator.Play("attack1");
            }
        }
        if(PlayerJump.instance.isJumping)
        {
            animator.Play("startJump");
        }
        if(!PlayerJump.instance.grounded)
        {
            animator.Play("goingDown");
        }
        if(PlayerRoll.instance.rolled && PlayerRoll.instance.canRoll) 
        {
            animator.Play("roll");
        }
        if(Ability1Manager.instance.doAbility)
        {
            animator.Play("ability1");
        }
        if(Ability2Manager.instance.doAbility)
        {
            animator.Play("ability2");
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

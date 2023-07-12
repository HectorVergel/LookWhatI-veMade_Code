using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundDash : StateMachineBehaviour
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
        PlayerRoll.instance.rolled = false;
        PlayerRoll.instance.canRoll = false;
        PlayerDash.instance.airDashed = false;
        PlayerDash.instance.canAirDash = false;
        PlayerRoll.instance.StartRoll();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PlayerRoll.instance.ResetTime();
        if(!PlayerRoll.instance.doingRoll)
        {
            if(Character.currentType == CharacterType.Warrior)
            {
                MeleCombat.instance.FlipInDirection();
            }
            else
            {
                WizardCombat.instance.FlipInDirection();
            }
            if(!PlayerJump.instance.grounded)
            {
                animator.Play("goingDown");
            }
            else
            {
                animator.Play("idle");
            }
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

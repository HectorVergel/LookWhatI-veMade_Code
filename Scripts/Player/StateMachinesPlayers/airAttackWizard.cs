using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class airAttackWizard : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        WizardCombat.instance.AirAttack();
        WizardCombat.instance.isAttacking = false;
        PlayerDash.instance.airDashed = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(PlayerJump.instance.grounded)
        {
            animator.Play("goingDown");
        }
        if(!PlayerDash.instance.CanIAirDash())
        {
            PlayerDash.instance.airDashed = false;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(!PlayerJump.instance.grounded)
        {
            if(PlayerJump.instance.GoingDown())
            {
                animator.Play("goingDown");
            }
            else
            {
                animator.Play("goingUp");
            }
        }
    }

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

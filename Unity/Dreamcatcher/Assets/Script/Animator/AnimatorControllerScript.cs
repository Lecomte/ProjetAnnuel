﻿using UnityEngine;
using System.Collections;

public class AnimatorControllerScript : StateMachineBehaviour {

    public bool startState = false;
    public bool needToStopActions = false;
    public bool needToWaitBeforeChange = true;
    private float startTime;
    public string name;
    public float damageCoeff;
    public float comboPoints;
    public float animTime = 2.0f;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat("DamageCoeff", damageCoeff);
        animator.SetFloat("ComboPoints", comboPoints);
        if (!startState || (animator.GetInteger("Button") == 1 && animator.GetInteger("ActionPoints") == 0))
        {
            animator.SetInteger("Button", 0);
        }
        animator.SetBool("stopActions", false);
        startTime = Time.timeSinceLevelLoad;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) 
    {
        if (needToStopActions)
        {
            animator.SetBool("Animating", false);
            needToStopActions = false;
            animator.SetBool("stopActions", true);
        }
        else if (!needToWaitBeforeChange || Time.timeSinceLevelLoad - startTime > animTime / 2)
        {
            if (Input.GetButtonDown("A_1"))
            {
                animator.SetInteger("Button", 1);
                Debug.Log("A");
            }
            if (Input.GetButtonDown("B_1"))
            {
                Debug.Log("B");
                animator.SetInteger("Button", 2);
            }
            if (Input.GetButtonDown("X_1"))
            {
                animator.SetInteger("Button", 3);
            }
            if (Input.GetButtonDown("Y_1"))
            {
                animator.SetInteger("Button", 4);
            }
            if (!needToWaitBeforeChange || Time.timeSinceLevelLoad - startTime > animTime)
            {
                animator.SetBool("Animating", false);
            }
        }
	}

    public void stopActions()
    {
        needToStopActions = true;
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        animator.SetBool("Animating", true);
	}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}

using UnityEngine;
using System.Collections;

public class DodgeScript : StateMachineBehaviour
{

    [SerializeField]
    private float dodgeSpeed = 100f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetInteger("ActionPoints", animator.GetInteger("ActionPoints")-1);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 direction = new Vector3(Input.GetAxis("L_XAxis_1"), 0, -Input.GetAxis("L_YAxis_1"));
        direction.Normalize();
		if(direction == Vector3.zero)
			direction = Vector3.forward;
        animator.gameObject.transform.parent.Translate(direction * Time.deltaTime * dodgeSpeed);
        Debug.Log(animator.gameObject.transform.parent);
        Debug.Log(direction * Time.deltaTime * dodgeSpeed);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}

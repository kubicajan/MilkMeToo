using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitScript : StateMachineBehaviour
{
    private float timer;
    private bool timerStarted;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 3f; // Set the timer to 3 seconds
        timerStarted = true;

        Debug.Log("Starting 3-second wait...");
    }

    // Called on each Update frame between OnStateEnter and OnStateExit
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (timerStarted)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timerStarted = false;
                Debug.Log("3 seconds have passed!");
                // You can trigger a transition or perform other actions here
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
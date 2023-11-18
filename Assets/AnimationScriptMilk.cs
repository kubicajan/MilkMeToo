using System.Text.RegularExpressions;
using Objects.Abstract.ActiveObjectClasses;
using UnityEngine;
using Utilities;

public class AnimationScriptMilk : StateMachineBehaviour
{
    private ActiveKokTreeObject activeKokTreeObject;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        string pattern = @"^(\w+)(?:\s(\d+))?$";;
        Debug.Log(animator.gameObject.name);
        Match decoded = Helpers.ParseRegex(animator.gameObject.name, pattern);
        string className = decoded.Groups[1].Value;
        int? number = int.TryParse(decoded.Groups[2].Value, out int result) ? result : null;
        activeKokTreeObject = Helpers.GetActiveKokTreeObject(className);
        activeKokTreeObject.PlayMilked(number);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

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
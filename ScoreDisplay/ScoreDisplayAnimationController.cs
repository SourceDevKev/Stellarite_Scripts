using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreDisplayAnimationController : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        NavigationController restartButtonController = animator.gameObject.GetComponent<NavigationController>();
        restartButtonController.OnRestart();
    }
}

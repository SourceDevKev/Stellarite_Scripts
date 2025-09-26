using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartButtonAnimationController : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RestartButtonController restartButtonController = animator.gameObject.GetComponent<RestartButtonController>();
        restartButtonController.RestartWrapper();
    }
}

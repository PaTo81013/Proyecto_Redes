using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class SaltarAnimatorBehaviour : StateMachineBehaviour
{
    public string boolToSetFalse = "Saltar";
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
       animator.SetBool(boolToSetFalse,false);
    }

    
}

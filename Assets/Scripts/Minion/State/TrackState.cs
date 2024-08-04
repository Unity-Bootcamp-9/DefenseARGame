using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;

public class TrackState : StateMachineBehaviour
{
    private Minion minion;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minion = animator.GetComponent<Minion>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minion.TargetDetection();
        minion.AttackDetection();
        minion.SetTarget(minion.target);
    }

}

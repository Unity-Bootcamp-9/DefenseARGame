using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SeekState : StateMachineBehaviour
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

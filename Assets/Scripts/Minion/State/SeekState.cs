using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SeekState : StateMachineBehaviour
{
    private MinionBehaviour minionBehaviour;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour = animator.GetComponent<MinionBehaviour>();
        minionBehaviour.isAttack = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour.TargetDetection();
        minionBehaviour.AttackDetection();
    }
}

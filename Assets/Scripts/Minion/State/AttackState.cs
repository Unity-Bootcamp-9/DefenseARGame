using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private MinionBehaviour minionBehaviour;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour = animator.GetComponent<MinionBehaviour>();
        minionBehaviour.SetIsAttacking(true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour.SetIsAttacking(false);
    }

}

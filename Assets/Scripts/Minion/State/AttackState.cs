using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackState : StateMachineBehaviour
{
    private MinionBehaviour minionBehaviour;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour = animator.GetComponent<MinionBehaviour>();
        minionBehaviour.isAttack = true;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(minionBehaviour.target.GetComponent<Entity>() != null)
        {
            if(minionBehaviour.target.GetComponent<Entity>().hp <= 0  )
            {
                animator.SetBool(MinionBehaviour.hashAttack, false);
            }
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour.isAttack = false;
    }
}

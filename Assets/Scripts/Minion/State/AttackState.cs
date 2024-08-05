using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackState : StateMachineBehaviour
{
    private Minion minion;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minion = animator.GetComponent<Minion>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minion.SetTarget(minion.transform);

        if (minion.target.GetComponent<Entity>() != null)
        {
            if(minion.target.GetComponent<Entity>().hp <= 0  )
            {
                animator.SetBool(Minion.hashAttack, false);
            }
        }
    }
}

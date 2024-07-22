using System.Collections;
using UnityEngine;

public class TurretAttackState : StateMachineBehaviour
{
    TurretBehavior turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehavior>();
        turretBehavior.Attacking();
        Debug.Log("TurretAttackState");
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.AttackCondition();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}

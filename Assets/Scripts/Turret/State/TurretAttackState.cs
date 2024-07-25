using System.Collections;
using UnityEngine;

public class TurretAttackState : StateMachineBehaviour
{
    TurretBehaviour turretBehavior;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehaviour>();
        turretBehavior.StartAttack();
    }
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.TargetDetection();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.StopAttack();
    }
}

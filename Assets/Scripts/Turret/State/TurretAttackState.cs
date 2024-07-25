using System.Collections;
using UnityEngine;

public class TurretAttackState : StateMachineBehaviour
{
    RevisedTurretBehaviour turretBehavior;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<RevisedTurretBehaviour>();
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

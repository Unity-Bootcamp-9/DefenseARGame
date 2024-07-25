using Unity.VisualScripting;
using UnityEngine;

public class TurretDetectionState : StateMachineBehaviour
{
    TurretBehavior turretBehavior;
    Projectile projectile;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehavior>();
        projectile = animator.GetComponentInChildren<Projectile>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.DetectMinion();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        projectile.target = turretBehavior.target;
    }
}
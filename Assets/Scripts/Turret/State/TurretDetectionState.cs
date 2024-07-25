using Unity.VisualScripting;
using UnityEngine;

public class TurretDetectionState : StateMachineBehaviour
{
    RevisedTurretBehaviour turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<RevisedTurretBehaviour>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.TargetDetection();
    }
}
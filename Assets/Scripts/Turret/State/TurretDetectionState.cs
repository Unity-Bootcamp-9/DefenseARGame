using Unity.VisualScripting;
using UnityEngine;

public class TurretDetectionState : StateMachineBehaviour
{
    TurretBehaviour turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehaviour>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior.TargetDetection();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TurretDeadState : StateMachineBehaviour
{
    RevisedTurretBehaviour turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<RevisedTurretBehaviour>();
        // 흙먼지나 터지는 이펙트 생성
    }
}

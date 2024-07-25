using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TurretDeadState : StateMachineBehaviour
{
    TurretBehaviour turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehaviour>();
        // 흙먼지나 터지는 이펙트 생성
    }
}

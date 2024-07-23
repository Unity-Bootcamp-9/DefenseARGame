using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretDeadState : StateMachineBehaviour
{
    TurretBehavior turretBehavior;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        turretBehavior = animator.GetComponent<TurretBehavior>();
        // 흙먼지나 터지는 이펙트 생성
        turretBehavior.ColliderOff();
    }
}

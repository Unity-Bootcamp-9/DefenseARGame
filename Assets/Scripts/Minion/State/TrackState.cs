using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class TrackState : StateMachineBehaviour
{
    private MinionBehaviour minionBehaviour;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour = animator.GetComponent<MinionBehaviour>();
        minionBehaviour.isAttack = false;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        minionBehaviour.TargetDetection();
        minionBehaviour.AttackDetection();
    }
    
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}

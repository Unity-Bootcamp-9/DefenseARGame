using Unity.VisualScripting;
using UnityEngine;

public class TurretDetectionState : TurretStateMachine
{
    public Animator _animator;
    public Turret _turret;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator.GetComponent<Animator>();
        _turret = animator.GetComponent<Turret>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _turret.DetectMinion();
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator.SetBool("attackStart", true);

    }
}
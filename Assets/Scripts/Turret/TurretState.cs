using UnityEngine;

public class TurretState : IState
{
    protected TurretStateMachine turretStateMachine;
    protected Turret turret;

    private string animBoolName;

    protected bool triggerCalled;

    public TurretState(TurretStateMachine turretStateMachine, Turret turret, string animBoolName)
    {
        this.turretStateMachine = turretStateMachine;
        this.turret = turret;
        this.animBoolName = animBoolName;
    }

    public virtual void Enter()
    {
        turret.anim.SetBool(animBoolName, true);
    }

    public virtual void Update()
    {

    }

    public virtual void Exit()
    {
        turret.anim.SetBool(animBoolName, false);
    }

    public virtual void AnimationFinishTrigger()
    {
        triggerCalled = true;
    }

}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : Entity
{
    Animator animator;
    public void Awake()
    {
        animator = GetComponent<Animator>();
        hp = 100;
        damage = 5;
    }

    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);
        if (hp <= 0)
        {
            animator.SetTrigger(TurretBehavior.hastisDead);
        }
    }
}

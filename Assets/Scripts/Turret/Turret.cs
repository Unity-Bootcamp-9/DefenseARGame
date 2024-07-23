using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Entity
{
    Animator animator;

    public override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        hp = 100;
        maxHP = hp;
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

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Turret : Entity
{
    Animator animator;
    public Image HPFilledImage;

    public void Awake()
    {
        animator = GetComponent<Animator>();
        hp = 100;
        damage = 5;
        HPFilledImage.fillAmount = 1;
    }

    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);
        HPFilledImage.fillAmount = hp * 0.01f;
        if (hp <= 0)
        {
            animator.SetTrigger(TurretBehavior.hastisDead);
        }
    }
}

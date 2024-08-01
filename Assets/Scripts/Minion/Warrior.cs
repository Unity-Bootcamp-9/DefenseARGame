using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Warrior : Minion
{
    public void Start()
    {
        isAttack = false;
        isStun = false;
        animator = GetComponent<Animator>();

        enemyLayerSet();
    }

    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        minionCollider = GetComponent<Collider>();
        hp = maxHP;
        hpBar.enabled = true;
        agent.enabled = true;
        minionCollider.enabled = true;
        HPFilledImage.fillAmount = (float)hp / (float)maxHP;
    }

    public void Update()
    {
        if (agent.enabled == true)
        {
            if (target == null)
            {
                DefaultTargetSet();
            }
            if (isAttack)
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                agent.SetDestination(target.position);
            }
            transform.LookAt(target);
        }
    }

    public void Attack()
    {
        if (target.GetComponent<Entity>() != null && target.GetComponent<Collider>().enabled == true)
        {
            target.GetComponent<Entity>().GetHit(damage);
        }
    }


}

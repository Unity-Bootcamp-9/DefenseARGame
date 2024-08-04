using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Warrior : Minion
{
    protected IObjectPool<Warrior> warriorObjectPool;
    public IObjectPool<Warrior> WarriorObjectPool { set => warriorObjectPool = value; }

    public void Start()
    {
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
            transform.LookAt(target);
        }
    }

    public void Attack()
    {
        if (target.GetComponent<Entity>() != null && target.GetComponent<Collider>().enabled == true)
        {
            target.GetComponent<Entity>().GetHit(damage);
            Managers.Sound.Play(Define.Sound.Speech, $"Trap 1_Sharp_Spike_{Random.Range(1, 4):D2}", 0.2f);
        }
    }


    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);
        if (hp <= 0)
        {
            hpBar.enabled = false;
            minionCollider.enabled = false;
            agent.enabled = false;
            Die();
            target = transform;
        }
    }
    public void Die()
    {
        animator.SetTrigger(hashDie);
        StartCoroutine(Deactivate(2f));
    }

    IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        warriorObjectPool.Release(this);
    }
}

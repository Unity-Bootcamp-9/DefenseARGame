using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Minion : Entity
{
    private IObjectPool<Minion> objectPool;
    public IObjectPool<Minion> ObjectPool { set => objectPool = value; }
    private MinionBehaviour minionBehaviour;    
    private Transform target;
    private NavMeshAgent agent;
    private Collider collider;
    [SerializeField]
    private Collider attackCollider;
    private Transform defaultTarget;



    public void Init(Transform mainTurretTransform)
    {
        defaultTarget = mainTurretTransform;
    }

    private void OnEnable()
    {
        hp = 100;
        maxHP = hp;
        agent = GetComponent<NavMeshAgent>();
        minionBehaviour = GetComponent<MinionBehaviour>();
        collider = GetComponent<Collider>();
        collider.enabled = true;
    }



    public void Update()
    {

        if(hp > 0)
            target = minionBehaviour.target;

        if(minionBehaviour.target == null)
        {
            target = defaultTarget;
        }

        if (minionBehaviour.isAttacking)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(target.position);
        }
        transform.LookAt(target);
    }

    public void Deactivate()
    {
        objectPool.Release(this);
    }

    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);

        if(hp <= 0)
        {
            collider.enabled = false;
            attackCollider.enabled = false;
            minionBehaviour.Die();
            target = transform;
        }
    }

}

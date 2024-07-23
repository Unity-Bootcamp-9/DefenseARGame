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
    public void Start()
    {
        hp = 100;
        agent = GetComponent<NavMeshAgent>();
        minionBehaviour = GetComponent<MinionBehaviour>();
    }

    public void Update()
    {
        target = minionBehaviour.target;
        if (!minionBehaviour.isAttacking)
        {
            agent.SetDestination(target.position);
        }
        else
        {
            agent.SetDestination(transform.position);
        }
        transform.LookAt(target);
    }

    public void Deactivate()
    {
        objectPool.Release(this);
    }

}

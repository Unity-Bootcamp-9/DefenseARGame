using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Minion : Entity
{
    private IObjectPool<Minion> objectPool;
    public IObjectPool<Minion> ObjectPool { set => objectPool = value; }
    

    public void Start()
    {
        hp = 100;
    }

    public void Deactivate()
    {
        objectPool.Release(this);
    }


}

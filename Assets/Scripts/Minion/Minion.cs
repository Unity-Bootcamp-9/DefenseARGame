using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Minion : MonoBehaviour
{
    private IObjectPool<Minion> objectPool;

    public IObjectPool<Minion> ObjectPool { set => objectPool = value; }


    [SerializeField] private float timeoutDelay = 3f;

    public void Deactivate()
    {
        objectPool.Release(this);
    }

}

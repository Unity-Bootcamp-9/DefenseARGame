using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class MinionSpawnManager : MonoBehaviour
{
    [SerializeField] private Minion minionPrefab;
    [SerializeField] private bool collectionCheck = false;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 50;
    
    private IObjectPool<Minion> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<Minion>(CreateMinion, OnTakeFromPool,
                            OnReturnedToPool,OnDestroyPoolObject,collectionCheck,
                            defaultCapacity,maxPoolSize);
    }

    private Minion CreateMinion()
    {
        Minion minionInstance = Instantiate(minionPrefab);
        minionInstance.ObjectPool = objectPool; 
        return minionInstance;
    }

    private void OnReturnedToPool(Minion minion)
    {
        minion.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(Minion minion)
    {
        minion.gameObject.SetActive(true);
    }

    private void OnDestroyPoolObject(Minion minion)
    {
        Destroy(minion.gameObject);
    }


}

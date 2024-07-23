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
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float waveCreateDelay = 30f;
    [SerializeField] private float minionCreateDelay = 1f;
    [SerializeField] private int minionsPperWave = 5;
    [SerializeField] private Transform enemyMainTurret;

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
        minion.Init(enemyMainTurret);
        minion.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void OnDestroyPoolObject(Minion minion)
    {
        Destroy(minion.gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaveSpawnRoutine(waveCreateDelay));
    }


    IEnumerator WaveSpawnRoutine(float _waveCreateDelay)
    {
        while (true)
        {
            StartCoroutine(MinionSpawnRoutine(minionCreateDelay));
            yield return new WaitForSeconds(_waveCreateDelay);
        }
    }

    IEnumerator MinionSpawnRoutine(float _minionCreateDelay)
    {
        for(int i = 0; i < minionsPperWave; ++i)
        {
            Minion minionObject = objectPool.Get();
            

            yield return new WaitForSeconds(_minionCreateDelay);
        }
    }






}

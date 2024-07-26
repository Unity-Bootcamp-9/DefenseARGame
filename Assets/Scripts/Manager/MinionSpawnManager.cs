using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class MinionSpawnManager : MonoBehaviour
{
    [SerializeField] private MinionBehaviour minionPrefab;
    [SerializeField] private bool collectionCheck = false;
    [SerializeField] private int defaultCapacity = 10;
    [SerializeField] private int maxPoolSize = 50;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float waveCreateDelay = 30f;
    [SerializeField] private float minionCreateDelay = 1f;
    [SerializeField] private int minionsPerWave = 5;
    [SerializeField] private Transform enemyMainTurret;
    [SerializeField] private HPBar hpBar;
    [SerializeField] private Subject subject;

    private int count = 0;
    private Coroutine waveSpawnRoutine;
    private Coroutine minionSpawnRoutine;
    private IObjectPool<MinionBehaviour> objectPool;

    private void Awake()
    {
        objectPool = new ObjectPool<MinionBehaviour>(CreateMinion, OnTakeFromPool,
                            OnReturnedToPool,OnDestroyPoolObject,collectionCheck,
                            defaultCapacity,maxPoolSize);
        subject.RedWin += StopSpawn;
        subject.BlueWin += StopSpawn;
    }

    public void StopSpawn()
    {
        if(waveSpawnRoutine != null)
            StopCoroutine(waveSpawnRoutine);
        if(minionSpawnRoutine != null)
            StopCoroutine(minionSpawnRoutine);
    }

    private MinionBehaviour CreateMinion()
    {
        MinionBehaviour minionInstance = Instantiate(minionPrefab);
        minionInstance.ObjectPool = objectPool;
        minionInstance.name = count.ToString();
        count++;
        return minionInstance;
    }

    private void OnReturnedToPool(MinionBehaviour minion)
    {
        minion.transform.position = spawnPoint.position;
        minion.gameObject.SetActive(false);
    }

    private void OnTakeFromPool(MinionBehaviour minion)
    {
        minion.gameObject.SetActive(true);
        minion.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void OnDestroyPoolObject(MinionBehaviour minion)
    {
        Destroy(minion.gameObject);
    }

    private void Start()
    {
        waveSpawnRoutine = StartCoroutine(WaveSpawnRoutine(waveCreateDelay));
    }


    IEnumerator WaveSpawnRoutine(float _waveCreateDelay)
    {
        while (true)
        {
            minionSpawnRoutine = StartCoroutine(MinionSpawnRoutine(minionCreateDelay));
            yield return new WaitForSeconds(_waveCreateDelay);
        }
    }

    IEnumerator MinionSpawnRoutine(float _minionCreateDelay)
    {
        for(int i = 0; i < minionsPerWave; ++i)
        {
            MinionBehaviour minionObject = objectPool.Get();
            minionObject.Init(enemyMainTurret, subject);

            yield return new WaitForSeconds(_minionCreateDelay);
        }
    }
}

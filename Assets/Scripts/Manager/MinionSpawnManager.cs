using CsvHelper.Configuration.Attributes;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;


public class MinionSpawnManager : MonoBehaviour
{
    [Header("[Spawn Option]")]
    [SerializeField] private Transform enemyMainTurret;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float startDelay;
    [SerializeField] private float waveCreateDelay;
    [SerializeField] private float minionCreateDelay;
    [SerializeField] private int warriorsPerWave;
    [SerializeField] private int archersPerWave;

    [Header("[ObjectPool Option]")]
    [SerializeField] private bool collectionCheck = false;
    [SerializeField] private int defaultCapacity;
    [SerializeField] private int maxPoolSize;

    [Header("[Subject]")]
    [SerializeField] private Subject subject;

    private Warrior warriorPrefab;
    private Archer archerPrefab;

    private Coroutine waveSpawnRoutine;
    private Coroutine minionSpawnRoutine;    

    private IObjectPool<Warrior> warriorObjectPool;
    private IObjectPool<Archer> archerObjectPool;

    public const int redLayer = 6;
    public const int blueLayer = 7;

    private string warriorName;
    private string archerName;

    private void Awake()
    {
        warriorObjectPool = new ObjectPool<Warrior>(CreateWarrior, OnTakeFromPool,
                            OnReturnedToPool, OnDestroyPoolObject, collectionCheck,
                            defaultCapacity, maxPoolSize);

        archerObjectPool = new ObjectPool<Archer>(CreateArcher, OnTakeFromPool,
                    OnReturnedToPool, OnDestroyPoolObject, collectionCheck,
                    defaultCapacity, maxPoolSize);

        if(gameObject.layer == redLayer)
        {
            warriorName = "Warrior_Red";
            archerName = "Archer_Red";
        }
        else if(gameObject.layer == blueLayer)
        {
            warriorName = "Warrior_Blue";
            archerName = "Archer_Blue";
        }

        warriorPrefab = Managers.Resource.Load<Warrior>($"Prefabs/Minion/{warriorName}");
        archerPrefab = Managers.Resource.Load<Archer>($"Prefabs/Minion/{archerName}");

        subject.RedWin += StopSpawn;
        subject.BlueWin += StopSpawn;
    }

    public void StopSpawn()
    {
        if (waveSpawnRoutine != null)
            StopCoroutine(waveSpawnRoutine);
        if (minionSpawnRoutine != null)
            StopCoroutine(minionSpawnRoutine);
    }

    private Warrior CreateWarrior()
    {
        Warrior minionInstance = Instantiate(warriorPrefab, spawnPoint, false);
        minionInstance.transform.parent = transform;
        minionInstance.WarriorObjectPool = warriorObjectPool;
        return minionInstance;
    }
    private Archer CreateArcher()
    {
        Archer minionInstance = Instantiate(archerPrefab, spawnPoint, false);
        minionInstance.transform.parent = transform;
        minionInstance.ArcherObjectPool = archerObjectPool;
        return minionInstance;
    }

    private void OnTakeFromPool(Minion minion)
    {
        minion.gameObject.SetActive(true);
        minion.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
    }

    private void OnReturnedToPool(Minion minion)
    {
        minion.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        minion.gameObject.SetActive(false);
    }


    private void OnDestroyPoolObject(Minion minion)
    {
        Destroy(minion.gameObject);
    }

    private void OnEnable()
    {
        waveSpawnRoutine = StartCoroutine(WaveSpawnRoutine(waveCreateDelay, startDelay));
    }


    IEnumerator WaveSpawnRoutine(float _waveCreateDelay, float _startDelay)
    {
        yield return new WaitForSeconds(_startDelay);

        while (true)
        {
            minionSpawnRoutine = StartCoroutine(MinionSpawnRoutine(minionCreateDelay));
            yield return new WaitForSeconds(_waveCreateDelay);
        }
    }

    IEnumerator MinionSpawnRoutine(float _minionCreateDelay)
    {

        for (int i = 0; i < warriorsPerWave; ++i)
        {
            Warrior warriorObject = warriorObjectPool.Get();
            warriorObject.Init(enemyMainTurret, subject);
            yield return new WaitForSeconds(_minionCreateDelay);
        }
        for (int i = 0; i < archersPerWave; ++i)
        {
            Archer archerObject = archerObjectPool.Get();
            archerObject.Init(enemyMainTurret, subject);
            yield return new WaitForSeconds(_minionCreateDelay);
        }
    }

}

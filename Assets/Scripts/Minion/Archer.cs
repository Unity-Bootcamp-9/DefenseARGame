using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using static GoalManager;

public class Archer : Minion
{
    protected IObjectPool<Archer> archerObjectPool;
    public IObjectPool<Archer> ArcherObjectPool { set => archerObjectPool = value; }

    [SerializeField] private GameObject arrrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootSpeed;
    protected Arrow arrow;

    protected override void Start()
    {

        isStun = false;
        animator = GetComponent<Animator>();
        arrow = arrrowPrefab.GetComponent<Arrow>();

        enemyLayerSet();
        arrrowPrefab.SetActive(false);
    }
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        minionCollider = GetComponent<Collider>();
        animator = GetComponent<Animator>();
        arrrowPrefab.SetActive(false);
        animator.SetBool(hashDie, false);
        hp = maxHP;
        hpBar.enabled = true;
        agent.enabled = true;
        minionCollider.enabled = true;
        HPFilledImage.fillAmount = (float)hp / (float)maxHP;
    }
    public void Update()
    {
        if(agent.enabled == true)
        {
            if (target == null)
            {
                DefaultTargetSet();
            }
            transform.LookAt(target);
            SetTarget();
        }
    }
    
    public void Shoot()
    {
        arrrowPrefab.transform.position = shootPoint.transform.position;
        arrow.Init(damage, target.gameObject, shootPoint);
        arrrowPrefab.SetActive(true);
        Managers.Sound.Play(Define.Sound.Speech, $"Shooting_Archer_Arrow_Bow_{Random.Range(1, 4):D2}", 0.2f);
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
        animator.SetBool(hashDie, true);
        StartCoroutine(Deactivate(2f));
    }

    IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        archerObjectPool.Release(this);
    }
}

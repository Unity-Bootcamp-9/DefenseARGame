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
    private bool isShoot;

    public void Start()
    {
        isStun = false;
        isShoot = false;
        animator = GetComponent<Animator>();
        enemyLayerSet();
        arrrowPrefab.SetActive(false);
    }
    private void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        minionCollider = GetComponent<Collider>();
        hp = maxHP;
        arrrowPrefab.SetActive(false);
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

            ArrowMove();
        }
    }

    public void ArrowMove()
    {
        if (isShoot)
        {
            Vector3 dir = ((target.position + new Vector3(0,2,0)) - arrrowPrefab.transform.position).normalized;
            
            arrrowPrefab.transform.position += dir * shootSpeed * Time.deltaTime;
            
            if (Vector3.Distance(arrrowPrefab.transform.position, target.position) < 2.5f)
            {
                if (target.GetComponent<Entity>() != null && target.GetComponent<Collider>().enabled == true)
                {
                    target.GetComponent<Entity>().GetHit(damage);
                }
                isShoot = false;
                arrrowPrefab.SetActive(false);
            }
        }
        else
        {
            arrrowPrefab.SetActive(false);
        }
    }
    
    public void Shoot()
    {
        arrrowPrefab.transform.position = shootPoint.transform.position;
        arrrowPrefab.SetActive(true);
        isShoot = true;

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
        animator.SetTrigger(hashDie);
        StartCoroutine(Deactivate(2f));
    }

    IEnumerator Deactivate(float delay)
    {
        yield return new WaitForSeconds(delay);
        archerObjectPool.Release(this);
    }
}

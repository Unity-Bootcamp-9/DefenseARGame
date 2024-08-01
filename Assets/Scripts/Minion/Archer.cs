using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.WSA;
using static GoalManager;

public class Archer : Minion
{
    [SerializeField] private GameObject arrrowPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootSpeed;

    
    private bool isShoot;

    public void Start()
    {
        isAttack = false;
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
            if (isAttack)
            {
                agent.SetDestination(transform.position);
            }
            else
            {
                agent.SetDestination(target.position);
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

        Managers.Sound.Play(Define.Sound.Speech, $"Shooting_Archer_Arrow_Bow_{Random.Range(1, 4):D2}, 0.2f");
    }



}

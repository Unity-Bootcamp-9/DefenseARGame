using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

public class TurretBehaviour : Entity
{
    public static readonly int hashAttackStart = Animator.StringToHash("AttackStart");
    public static readonly int hastisDead = Animator.StringToHash("IsDead");

    private List<Transform> enemyMinions = new List<Transform>();
    [SerializeField] private GameObject projectile;
    [SerializeField] private Canvas hpBar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private bool isMainTurret;
    [SerializeField] private GameSystemSubject subject;
    public Transform target { get; private set; }
    private Animator animator;
    private Collider turretCollier;
    private Rigidbody projectileRigid;

    private float detectionRange = 10f;
    public float moveSpeed = 7.0f;
    private bool isAttack = false;
    private bool isPlaying = true;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        turretCollier = GetComponent<Collider>();
        projectileRigid = projectile.GetComponent<Rigidbody>();
        projectile.transform.position = spawnPoint.transform.position;
        projectile.SetActive(false);
        enemyLayerSet();
        subject.RedWin += StopTurret;
        subject.BlueWin += StopTurret;
    }

    private void StopTurret()
    {
        isPlaying = false;
    }

    private void FixedUpdate()
    {
        if (isAttack && isPlaying)
        {
            Vector3 moveDir = target.transform.position - projectile.transform.position;
            projectileRigid.MovePosition(projectile.transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
         
            if (Vector3.Distance(projectile.transform.position, target.position) < 0.8f)
            {
                target.gameObject.GetComponent<Entity>().GetHit(damage);
                projectile.transform.position = spawnPoint.transform.position;
                projectile.SetActive(false);
            }
        }
    }

    public void TargetDetection()
    {
        if(isPlaying)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, 1 << enemyLayer);

            if (colliders.Length >= 1)
            {
                animator.SetBool(hashAttackStart , true);
                isAttack = true;
                target = TargetSelection(colliders, transform);
            }
            else
            {
                isAttack = false;
                animator.SetBool(hashAttackStart, false);
            }
        }
    }

    public Transform TargetSelection(Collider[] colliders, Transform thisTransform)
    {
        Transform turret = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Minion"))
            {
                enemyMinions.Add(collider.transform);
            }
        }

        enemyMinions = enemyMinions.OrderBy(enemyMinion => Vector3.Distance(enemyMinion.position, thisTransform.position)).ToList<Transform>();
        target = enemyMinions[0];
        enemyMinions.Clear();

        return target;
    }

    public void StartAttack()
    {
        
        StartCoroutine(Attack());
    }

    public void StopAttack()
    {
        StopCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        while (true)
        {
            projectile.transform.position = spawnPoint.transform.position;
            projectile.SetActive(true);
            yield return new WaitForSeconds(2f);
        }
    }


    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);
        if (hp <= 0)
        {
            hpBar.enabled = false;
            turretCollier.enabled = false;
            animator.SetTrigger(hastisDead);
            if (isMainTurret && gameObject.layer == redLayer)
            {
                subject.Victory();
            }
            else if (isMainTurret && gameObject.layer == blueLayer)
            {
                subject.Defead();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

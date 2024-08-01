using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

public class Minion : Entity
{
    public static readonly int hashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int hashDetected = Animator.StringToHash("Detected");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDie = Animator.StringToHash("Die");

    [SerializeField] protected Canvas hpBar;
    [SerializeField] protected Transform defaultTarget;
    [SerializeField] protected float detectionRange;
    [SerializeField] protected float attackRange;
    protected List<Transform> enemyMinions = new List<Transform>(100);
    protected Collider minionCollider;
    protected Animator animator;
    public Transform target { get; protected set; }
    protected NavMeshAgent agent;
    public bool isStun;
    public bool isAttack { get; set; }

    protected IObjectPool<Minion> objectPool;
    public IObjectPool<Minion> ObjectPool { set => objectPool = value; }

    public void Init(Transform mainTurretTransform, Subject subject)
    {
        defaultTarget = mainTurretTransform;
        DefaultTargetSet();
        subject.RedWin += StopMinion;
        subject.BlueWin += StopMinion;
    }

    public void StopMinion()
    {
        agent.enabled = false;
        animator.enabled = false;
        this.enabled = false;
    }

    public void DefaultTargetSet()
    {
        target = defaultTarget;
    }

    public void TargetDetection()
    {

        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, 1 << enemyLayer);

        if (colliders.Length >= 1)
        {
            animator.SetTrigger(hashDetected);
            animator.SetBool(hashInPursuit, true);
            target = TargetSelection(colliders, transform);
        }
        else
        {
            animator.SetBool(hashInPursuit, false);
            DefaultTargetSet();
        }
    }

    public Transform TargetSelection(Collider[] colliders, Transform thisTransform)
    {
        Transform turret = null;
        Transform target = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Minion"))
            {
                enemyMinions.Add(collider.transform);
            }
            else if (collider.CompareTag("Turret") || collider.CompareTag("MainTurret"))
            {
                turret = collider.transform;
            }
        }
        if (enemyMinions.Count > 0)
        {
            enemyMinions = enemyMinions.OrderBy(enemyMinion => Vector3.Distance(enemyMinion.position, thisTransform.position)).ToList<Transform>();
            target = enemyMinions[0];
        }
        else
        {
            target = turret;
        }
        enemyMinions.Clear();

        return target;
    }

    public void AttackDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange, 1 << enemyLayer);
        Collider attackTarget = colliders.FirstOrDefault(collider => collider.transform == target);
        if (attackTarget != null)
        {
            animator.SetBool(hashAttack, true);
            isAttack = true;
        }
        else
        {
            isAttack = false;
        }
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
        objectPool.Release(this);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}

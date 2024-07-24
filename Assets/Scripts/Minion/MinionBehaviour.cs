using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class MinionBehaviour : Entity
{
    private IObjectPool<MinionBehaviour> objectPool;
    public IObjectPool<MinionBehaviour> ObjectPool { set => objectPool = value; }

    private Animator animator;
    private List<Transform> enemyMinions = new List<Transform>();
    public Transform target { get; private set; }
    private NavMeshAgent agent;
    private Collider minionCollider;
    [SerializeField]
    private Collider attackCollider;
    private Transform defaultTarget;

    public bool isAttacking { get;  set; }
    private int enemyLayer;
    private float detectionRange = 7f;
    private float attackRange = 1f;

    public static readonly int hashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int hashDetected = Animator.StringToHash("Detected");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDie = Animator.StringToHash("Die");
    private void Start()
    {
        isAttacking = false;
        animator = GetComponent<Animator>();
        if (gameObject.layer == 6)
        {
            enemyLayer = 7;
        }
        else if(gameObject.layer == 7)
        {
            enemyLayer = 6;
        }
    }

    public void Init(Transform mainTurretTransform)
    {
        defaultTarget = mainTurretTransform;
    }

    private void OnEnable()
    {
        hp = 100;
        maxHP = hp;
        agent = GetComponent<NavMeshAgent>();
        minionCollider = GetComponent<Collider>();
        minionCollider.enabled = true;
        attackCollider.enabled = false;
    }
    public void Update()
    {
        if (target == null)
        {
            target = defaultTarget;
        }

        if (isAttacking)
        {
            agent.SetDestination(transform.position);
        }
        else
        {
            agent.SetDestination(target.position);
        }
        transform.LookAt(target);
    }

    public void Deactivate()
    {
        objectPool.Release(this);
    }

    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);

        if (hp <= 0)
        {
            minionCollider.enabled = false;
            attackCollider.enabled = false;
            Die();
            target = transform;
        }
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
            else if (collider.CompareTag("Turret"))
            {
                turret = collider.transform;
            }
        }
        if(enemyMinions.Count > 0)
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
        Collider attackTarget= colliders.FirstOrDefault(collider => collider.transform == target);
        if (attackTarget != null)
        {
            animator.SetTrigger(hashAttack);
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
        Deactivate();
    }

    public void AttackRangeCollierTurnOn()
    {
        attackCollider.enabled = true; 
    }

    public void AttackRangeCollierTurnOff()
    {
        attackCollider.enabled = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}

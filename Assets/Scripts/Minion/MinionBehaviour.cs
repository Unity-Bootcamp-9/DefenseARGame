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
    public static readonly int hashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int hashDetected = Animator.StringToHash("Detected");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDie = Animator.StringToHash("Die");

    [SerializeField] private Canvas hpBar;
    [SerializeField] private Transform defaultTarget;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float attackRange = 1.5f;
    private List<Transform> enemyMinions = new List<Transform>(100);
    private Collider minionCollider;
    private Animator animator;
    public Transform target { get; private set; }
    private NavMeshAgent agent;
    public bool isSturn;

    public bool isAttack { get;  set; }

    private IObjectPool<MinionBehaviour> objectPool;
    public IObjectPool<MinionBehaviour> ObjectPool { set => objectPool = value; }
    
    private void Start()
    {
        isAttack = false;
        isSturn = false;
        animator = GetComponent<Animator>();
        enemyLayerSet();
    }

    public void Init(Transform mainTurretTransform , Subject subject)
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

    private void OnEnable()
    {
        hp = maxHP;
        agent = GetComponent<NavMeshAgent>();
        minionCollider = GetComponent<Collider>();
        hpBar.enabled = true;
        minionCollider.enabled = true;
        HPFilledImage.fillAmount = (float)hp / (float)maxHP;
    }
    public void Update()
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
            agent.SetDestination(transform.position);
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


    public void Attack()
    {
        if(target.GetComponent<Entity>() != null)
        {
            target.GetComponent<Entity>().GetHit(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}

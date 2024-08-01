using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretBehaviour : Entity
{
    public static readonly int hashAttackStart = Animator.StringToHash("AttackStart");
    public static readonly int hastisDead = Animator.StringToHash("IsDead");

    private List<Transform> enemyMinions = new List<Transform>(100);
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject destroyedTurret;
    [SerializeField] private Canvas hpBar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Subject subject;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float detectionRange;

    public Transform target { get; private set; }
    private Animator animator;
    private Collider turretCollier;
    private Rigidbody projectileRigid;

    public bool isDead { get; private set; }
    private bool isAttack = false;
    private IEnumerator attackCoroutine;

    protected override void Awake()
    {
        isDead = false;
        destroyedTurret.SetActive(false);
        animator = GetComponentInParent<Animator>();    
        turretCollier = GetComponent<Collider>();
        projectileRigid = projectile.GetComponent<Rigidbody>();
        projectile.transform.position = spawnPoint.transform.position;
        projectile.SetActive(false);
        enemyLayerSet();
        hp = maxHP;
    }

    private void FixedUpdate()
    {
        if (isAttack)
        {
            Vector3 moveDir = (target.transform.position - projectile.transform.position).normalized;
            projectileRigid.velocity = moveDir * projectileSpeed;
         
            if (Vector3.Distance(projectile.transform.position, target.position) < 0.8f)
            {
                target.gameObject.GetComponent<Entity>().GetHit(damage);
                projectile.SetActive(false);
                projectile.transform.position = spawnPoint.transform.position;
            }
        }
        else
        {
            projectile.SetActive(false);
        }
    }
    
    public void TargetDetection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRange, 1 << enemyLayer);


        if (colliders.Length >= 1)
        {
            target = TargetSelection(colliders, transform);
            animator.SetBool(hashAttackStart , true);
            isAttack = true;
        }
        else
        {
            isAttack = false;
            animator.SetBool(hashAttackStart, false);
        }
    }

    public Transform TargetSelection(Collider[] colliders, Transform thisTransform)
    {

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Minion"))
            {
                enemyMinions.Add(collider.transform);
            }
        }

        enemyMinions = enemyMinions.OrderBy(enemyMinion => Vector3.Distance(enemyMinion.position, thisTransform.position)).ToList<Transform>();

        if (enemyMinions.Count > 0)
        {
            target = enemyMinions[0];
        }
        enemyMinions.Clear();

        return target;
    }

    public void StartAttack()
    {
        attackCoroutine = Attack();
        StartCoroutine(attackCoroutine);
    }

    public void StopAttack()
    {
        StopCoroutine(attackCoroutine);
    }

    IEnumerator Attack()
    {
        while (true)
        {
            projectile.SetActive(true);
            projectile.transform.position = spawnPoint.transform.position;
            yield return new WaitForSeconds(2f);
        }
    }

    public override void GetHit(int _damage)
    {
        base.GetHit(_damage);
        if (hp <= 0)
        {
            if(gameObject.CompareTag("MainTurret"))
            {

                subject.SetResult(gameObject.layer);
            }
            projectile.SetActive(false);
            isDead = true;
            hpBar.enabled = false;
            turretCollier.enabled = false;
            animator.SetTrigger(hastisDead);
            destroyedTurret.SetActive(true);
            gameObject.SetActive(false);

            Managers.Sound.Play(Define.Sound.Effect, "Sus4_Rock_Large_Debris_01_Mono");
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

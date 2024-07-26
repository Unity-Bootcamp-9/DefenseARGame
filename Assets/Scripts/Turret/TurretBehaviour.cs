using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretBehaviour : Entity
{
    public static readonly int hashAttackStart = Animator.StringToHash("AttackStart");
    public static readonly int hastisDead = Animator.StringToHash("IsDead");

    private List<Transform> enemyMinions = new List<Transform>();
    [SerializeField] private GameObject projectile;
    [SerializeField] private GameObject destroyedTurret;
    [SerializeField] private Canvas hpBar;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Subject subject;
    public Transform target { get; private set; }
    private Animator animator;
    private Collider turretCollier;
    private Rigidbody projectileRigid;

    public bool isDead { get; private set; }
    private float detectionRange = 10f;
    public float moveSpeed = 7.0f;
    private bool isAttack = false;



    private void Awake()
    {
        isDead = false;
        destroyedTurret.SetActive(false);
        animator = GetComponentInParent<Animator>();    
        turretCollier = GetComponent<Collider>();
        projectileRigid = projectile.GetComponent<Rigidbody>();
        projectile.transform.position = spawnPoint.transform.position;
        projectile.SetActive(false);
        enemyLayerSet();
        


    }


    private void FixedUpdate()
    {
        if (isAttack)
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
            if(gameObject.CompareTag("MainTurret"))
            {
                subject.gameEnd = true;
                
            }
            projectile.SetActive(false);
            isDead = true;
            hpBar.enabled = false;
            turretCollier.enabled = false;
            animator.SetTrigger(hastisDead);
            destroyedTurret.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}

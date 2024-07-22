using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MinionBehaviour : MonoBehaviour
{
    public static readonly int hashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int hashDetected = Animator.StringToHash("Detected");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDie = Animator.StringToHash("Die");

    private float detectionRange = 4f;
    private float attackRange = 1f;
    private Animator animator;
    private int enemyLayer;
    [SerializeField]
    private Transform defaultTarger;
    private List<Transform> enemyMinions = new List<Transform>();
    public Transform target { get; private set; }

    private void Start()
    {
        animator = GetComponent<Animator>();
        target = defaultTarger;
        if (gameObject.layer == 6)
        {
            enemyLayer = 7;
        }
        else if(gameObject.layer == 7)
        {
            enemyLayer = 6;
        }
    }

    private void Update()
    {
        if (target.gameObject.name != null)
        {
            Debug.Log($"target : {target.gameObject.name}");
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
            target = defaultTarger;
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



    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}

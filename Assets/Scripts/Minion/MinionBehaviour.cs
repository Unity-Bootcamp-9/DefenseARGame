using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ShaderGraph.Internal.KeywordDependentCollection;
using static UnityEngine.GraphicsBuffer;

public class MinionBehaviour : MonoBehaviour
{
    public static readonly int hashInPursuit = Animator.StringToHash("InPursuit");
    public static readonly int hashDetected = Animator.StringToHash("Detected");
    public static readonly int hashAttack = Animator.StringToHash("Attack");
    public static readonly int hashDie = Animator.StringToHash("Die");

    private float detectionRange = 4.0f;
    private Animator animator;
    private bool camp;
    private int enemyLayer;
    private Transform target;
    [SerializeField]
    private Transform defaultTarger;
    private List<Transform> enemyMinions = new List<Transform>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        target = defaultTarger;
        if (gameObject.layer == 6)
        {
            camp = true;
            enemyLayer = 7;
        }
        else if(gameObject.layer == 7)
        {
            camp = false;
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
        float distance;
        float minDistance;

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
            target = enemyMinions[0];
            minDistance = Vector3.Distance(enemyMinions[0].position, thisTransform.position);
            for (int i = 1; i < enemyMinions.Count; ++i)
            {
                distance = Vector3.Distance(enemyMinions[i].position, thisTransform.position);
                if (minDistance > distance)
                {
                    minDistance = distance;
                    target = enemyMinions[i];
                }
            }
        }
        else
        {
            target = turret;
        }

        enemyMinions.Clear();
        
        
        return target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange) ;
    }

}

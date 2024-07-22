using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public static readonly int hashAttackStart = Animator.StringToHash("attackStart");
    public static readonly int hastisDead = Animator.StringToHash("isDead");
    
    private float detectionRange = 15.0f;
    private Animator anim;
    public Transform target;
    private Transform defaultTarget;

    public bool isAttack;
    private int enemyMinionLayer;
    public string enemyLayerName;
    private int layerMask;

    public Transform AttackTr;
    public GameObject projectile;
    public List<GameObject> projectiles;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        isAttack = false;
        defaultTarget = null;
        enemyMinionLayer = LayerMask.NameToLayer(enemyLayerName);
        layerMask = (1 << enemyMinionLayer);
        SetProjectileObject(projectile, 5, "projectile");
    }

    // 오브젝트를 받아 생성. (생성한 원본 오브젝트, 생성할 갯수, 생성할 객체의 이름)
    public void SetProjectileObject(GameObject _Obj, int _Count, string _Name)
    {
        for (int i = 0; i < _Count; i++)
        {
            GameObject obj = Instantiate(_Obj, AttackTr, false);
            //obj.SetActive(false);
            projectiles.Add(obj);
        }
    }

    public void DetectMinion()
    {
        Collider?[] colls = Physics.OverlapSphere(gameObject.transform.position, detectionRange, layerMask);

        if (colls.Length == 0)
        {
            anim.SetBool(hashAttackStart, false);
            target = null;
            ReferenceEquals(target, null);
        }
        else
        {
            colls = SortCollidersByDistance(colls, gameObject.transform);

            foreach (Collider coll in colls)
            {
                if (coll.gameObject.CompareTag("Minion"))
                {
                    anim.SetBool(hashAttackStart, true);

                    Debug.Log("Targetting Minion");
                    target = coll.gameObject.transform;
                }
            }
        }
    }

    Collider[] SortCollidersByDistance(Collider[] colliders, Transform reference)
    {
        return colliders.OrderBy(col => Vector3.Distance(reference.position, col.transform.position)).ToArray();
    }

    IEnumerator AttackCo()
    {
        while (AttackCondition())
        {
            for (int i = 0; i < 5; ++i)
            {
                if (projectiles[i].active == true) continue;
                else
                {
                    projectiles[i].SetActive(true);
                }
                yield return new WaitForSeconds(2f);
            }
        }
    }

    public void Attacking()
    {
        StartCoroutine(AttackCo());
    }

    public bool AttackCondition()
    {
        if(target == null)
        {
            anim.SetBool(hashAttackStart, false);
            return false;
        }
        else if (!target.gameObject.active)
        {
            anim.SetBool(hashAttackStart, false);
            return false;
        }
        else if (Vector3.Distance(target.position, transform.position) > detectionRange)
        {
            anim.SetBool(hashAttackStart, false);
            return false;
        }
        else
        {
            anim.SetBool(hashAttackStart, true);
            return true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }

}

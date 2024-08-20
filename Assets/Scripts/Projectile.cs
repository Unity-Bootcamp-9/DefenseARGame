using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private GameObject target;
    private Transform spawnPoint;
    private Rigidbody projectileRigid;
    private Vector3 curMoveDir;
    private Vector3 preMoveDir;
    private int damage;
    private float projectileSpeed = 21f;

    private void Start()
    {
        projectileRigid = GetComponent<Rigidbody>();    
    }

    public void Init(int _damgae, GameObject _target, Transform _spawnPoint)
    {
        damage = _damgae;
        target = _target;
        spawnPoint = _spawnPoint;
    }

    private void FixedUpdate()
    {
        if(target != null)
        {
            curMoveDir = (target.transform.position - transform.position).normalized;
            preMoveDir = curMoveDir;
            projectileRigid.velocity = curMoveDir * projectileSpeed;
        }
        else
        {
            projectileRigid.velocity = preMoveDir * projectileSpeed;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == target)
        {
            other.gameObject.GetComponent<Entity>().GetHit(damage);
            gameObject.SetActive(false);
            gameObject.transform.position = spawnPoint.position;
        }
        else if (other.CompareTag("Ground"))
        {
            gameObject.SetActive(false);
            gameObject.transform.position = spawnPoint.position;
        }
    }
}

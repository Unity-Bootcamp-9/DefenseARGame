using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private GameObject target;
    private Transform spawnPoint;
    private Rigidbody projectileRigid;
    private int damage;
    private float projectileSpeed = 31f;
    private Vector3 MoveDir;

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
        MoveDir = (target.transform.position - transform.position).normalized;
        MoveDir.y = 0;
        projectileRigid.velocity = MoveDir * projectileSpeed;
    }
    private void Update()
    {
        if(target.GetComponent<Entity>() != null)
        {
            if (target.GetComponent<Entity>().hp <= 0)
            {
                gameObject.SetActive(false);
                gameObject.transform.position = spawnPoint.position;
            }
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
    }

}

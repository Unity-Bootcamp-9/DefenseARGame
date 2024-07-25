using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    TurretBehavior turretBehavior;
    public Transform target;
    Rigidbody projectileRigid;
    public float moveSpeed = 7.0f;
    public int damage = 5;
    Vector3 spawnPoint;

    private void Start()
    {
        turretBehavior = transform.root.GetComponent<TurretBehavior>();
        projectileRigid = GetComponent<Rigidbody>();
        spawnPoint = new Vector3(0, 0, 0);
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        transform.position = spawnPoint;

        //Debug.Log($"OnEnable : {target.name}");
    }

    private void Update()
    {
        //projectileRigid.velocity = transform.forward * velocity;
        //var targetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
        //projectileRigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

//        target = turretBehavior.target;

        Debug.Log($"Update : {target.name}");
        Vector3 moveDir = target.transform.position - transform.position;

        projectileRigid.MovePosition(transform.position + moveDir * moveSpeed * Time.fixedDeltaTime);
        if (Vector3.Distance(gameObject.transform.position, target.position) < 0.8f)
        {
            target.gameObject.GetComponent<Entity>().GetHit(damage);
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        transform.position = spawnPoint;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]TurretBehavior turretBehavior;
    Transform target;
    public Rigidbody projectileRigid;
    public float turn;
    public float velocity;
    public int damage;
    Vector3 spawnPoint;

    private void Start()
    {
        turretBehavior = transform.root.GetComponent<TurretBehavior>();
        projectileRigid = GetComponent<Rigidbody>();
        turn = 20.0f;
        velocity = 7.0f;
        damage = 5;
        spawnPoint = new Vector3(0, 14, 0);

        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        if (turretBehavior != null && turretBehavior.target != null)
        {
            target = turretBehavior.target;
        }
    }

    private void Update()
    {
        projectileRigid.velocity = transform.forward * velocity;
        var targetRotation = Quaternion.LookRotation(target.position + new Vector3(0, 0.8f) - transform.position);
        projectileRigid.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

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

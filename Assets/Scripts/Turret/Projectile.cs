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

    private void Start()
    {
        turretBehavior = transform.root.GetComponent<TurretBehavior>();
        projectileRigid = GetComponent<Rigidbody>();
        turn = 20.0f;
        velocity = 7.0f;

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
            // 공격
            Debug.Log("Attack!!!!");
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        transform.position = Vector3.zero;
    }

}

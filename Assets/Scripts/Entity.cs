using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int hp;
    protected int damage;
    protected Entity target;

    private Transform targetTransform;

    private void Awake()
    {
        targetTransform = GetComponent<MinionBehaviour>().target;
        target = targetTransform.GetComponent<Minion>();
    }


    public virtual void GetHit(int _damage)
    {
        hp -= _damage;
        Debug.Log($"hp : {hp}");
    }

    public virtual void Attack()
    {
        target.GetHit(damage);
    }

}
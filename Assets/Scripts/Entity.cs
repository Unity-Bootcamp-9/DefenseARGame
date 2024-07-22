using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int hp;
    protected int damage;
    protected Entity target;

    public virtual void GetHit(int _damage)
    {
        hp -= _damage;
        Debug.Log($"hp : {hp}");
    }

    protected virtual void Attack()
    {
        target.GetHit(damage);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int hp;
    protected int damage;
    protected Entity target;

    protected void GetHit(int _damage)
    {
        hp -= _damage;
    }

    protected void Attack()
    {
        target.GetHit(damage);
    }
}
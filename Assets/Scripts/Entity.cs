using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected int hp;
    protected int damage;

    public virtual void GetHit(int _damage)
    {
        hp -= _damage;
        Debug.Log($"hp : {hp}");
    }


   
}
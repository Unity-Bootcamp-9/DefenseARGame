using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionWeapon : MonoBehaviour
{
    private int enemyLayer;
    private int damage;
    
    private void Start()
    {
        if (gameObject.layer == 6)
        {
            enemyLayer = 7;
        }
        else if (gameObject.layer == 7)
        {
            enemyLayer = 6;
        }
        damage = 10;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌함");
        if (other.gameObject.layer == enemyLayer)
        {
            other.gameObject.GetComponent<Entity>().GetHit(damage);
        }
    }

}

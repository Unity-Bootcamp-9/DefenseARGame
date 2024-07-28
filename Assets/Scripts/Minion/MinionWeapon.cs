using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionWeapon : MonoBehaviour
{
    
    public int damage { get; private set; }
    private int enemyLayer;

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
        damage = 20;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == enemyLayer)
        {
            if (other.gameObject.GetComponent<Entity>() != null)
            {
                other.gameObject.GetComponent<Entity>().GetHit(damage);
            }
        }
    }

}

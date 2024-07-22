using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Turret : Entity
{
    public void Awake()
    {
        hp = 100;
        damage = 5;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("GetHurt");
        }
    }
}
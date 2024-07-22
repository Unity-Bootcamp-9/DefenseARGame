using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionWeapon : MonoBehaviour
{
    [SerializeField]
    private Transform minionPrefab;
    private Minion minion;
    private int enemyLayer;

    private void Start()
    {
        minion = minionPrefab.GetComponent<Minion>();
        if (gameObject.layer == 6)
        {
            enemyLayer = 7;
        }
        else if (gameObject.layer == 7)
        {
            enemyLayer = 6;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("충돌함");
        if (other.gameObject.layer == enemyLayer)
        {
            minion.Attack();
            Debug.Log("공격함");
        }
    }

}

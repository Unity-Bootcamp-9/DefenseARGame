using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] protected Image HPFilledImage;
    [SerializeField] protected int hp;
    [SerializeField] protected int maxHP;
    [SerializeField] protected int damage;
    protected int enemyLayer;

    public const int redLayer = 6;
    public const int blueLayer = 7;

    public void Awake()
    {
        HPFilledImage.fillAmount = 1;
    }

    public void enemyLayerSet()
    {
        if (gameObject.layer == redLayer)
        {
            enemyLayer = blueLayer;
        }
        else if (gameObject.layer == blueLayer)
        {
            enemyLayer = redLayer;
        }
    }

    public virtual void GetHit(int _damage)
    {
        hp -= _damage;
        HPFilledImage.fillAmount = (float)hp / (float)maxHP;
    }


   
}
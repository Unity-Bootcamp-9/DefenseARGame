using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField] protected int damage;
    [SerializeField] protected int maxHP;
    [SerializeField] protected Image HPFilledImage;
    public int hp { get; protected set; }
    protected int enemyLayer;

    public const int redLayer = 6;
    public const int blueLayer = 7;

    protected virtual void Awake()
    {
        HPFilledImage.fillAmount = 1;
    }

    protected void enemyLayerSet()
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
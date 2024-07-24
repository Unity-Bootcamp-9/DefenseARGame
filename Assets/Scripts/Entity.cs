using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [SerializeField]protected int hp;
    [SerializeField]protected int maxHP;
    protected int damage;
    public Image HPFilledImage;

    public void Awake()
    {
        HPFilledImage.fillAmount = 1;
    }

    public virtual void GetHit(int _damage)
    {
        hp -= _damage;
        HPFilledImage.fillAmount = (float)hp / (float)maxHP;
    }


   
}
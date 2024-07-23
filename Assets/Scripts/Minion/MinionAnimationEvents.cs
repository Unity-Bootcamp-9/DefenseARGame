using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionAnimationEvents : MonoBehaviour
{
    [SerializeField]
    private GameObject attackRange;

    void Start()
    {
        attackRange.SetActive(false);
    }

    public void AttackRangeCollierTurnOn()
    {
        attackRange.SetActive(true);
    }

    public void AttackRangeCollierTurnOff()
    {
        attackRange.SetActive(false);
    }

}

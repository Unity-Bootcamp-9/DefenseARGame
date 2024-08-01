using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class IceSkill : Skill
{
    private const float duration = 0.26f;

    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "IceExplosion";
        SkillName_KR = "아이스 익스플로젼";
        Description = "";
        RequireMana = 4;
        Damage = 50;
        Radius = 15f;
        base.Init();
    }

    protected override void Activate()
    {
        GameObject effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset,
                draggingObject.transform.rotation
            );

        StartCoroutine(Attack(effect.transform.position, duration));
    }
    IEnumerator Attack(Vector3 position, float time)
    {
        for( int i = 0; i < 5; ++i )
        {
            TakeDamage(position, (Damage/2) / 5);
            yield return new WaitForSeconds(time);
        }
        TakeDamage(position, Damage / 2);

    }

    private void TakeDamage(Vector3 position, int damage)
    {
        int targetAmount = Physics.OverlapSphereNonAlloc(position, Radius / 2, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<Minion>().GetHit(damage);
        }
    }

}

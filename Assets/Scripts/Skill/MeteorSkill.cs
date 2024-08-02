using System.Collections;
using UnityEngine;

public class MeteorSkill : Skill
{
    private const float duration = 1.3f;

    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "Meteor";
        SkillName_KR = "메테오";
        Description = "범위 내에 있는 적 미니언에게 피해량만큼의 피해를 입힙니다.";
        RequireMana = 4;
        Damage = 15;
        Radius = 9f;
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
        yield return new WaitForSeconds(time);
        
        int targetAmount = Physics.OverlapSphereNonAlloc(position, Radius / 2, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<Minion>().GetHit(Damage);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class IceSkill : Skill
{
    private const float duration = 0.26f;

    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "IceExplosion";
        SkillName_KR = "아이스 익스플로젼";
        Description = "범위 내에 있는 적 미니언에게 1.3초간 피해량 만큼의 도트 데미지를 준 후 폭발하여 피해량의 5배 만큼의 피해와 둔화효과를 줍니다.";
        RequireMana = 4;
        Damage = 2;
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
        base.Activate();

        Managers.Sound.Play(Define.Sound.Effect, "Nunchaku Attack_Skill_Bass_02");
    }
    IEnumerator Attack(Vector3 position, float time)
    {
        for( int i = 0; i < 5; ++i )
        {
            TakeDamage(position, Damage);
            yield return new WaitForSeconds(time);
        }
        
        Managers.Sound.Play(Define.Sound.Effect, "Gaint Sword_Skill_Knight_Hit_02");
        TakeDamage(position, Damage * 5);
        TakeSlow(position);
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

    IEnumerator SpeedSlowCo(Collider targetColl)
    {
        targetColl.gameObject.GetComponent<NavMeshAgent>().speed = 0.6f;
        yield return new WaitForSeconds(1f);
        targetColl.gameObject.GetComponent<NavMeshAgent>().speed = 3f;        
    }

    private void TakeSlow(Vector3 position)
    {
        int targetAmount = Physics.OverlapSphereNonAlloc(position, Radius / 2, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            StartCoroutine(SpeedSlowCo(targets[j]));
        }
    }

}

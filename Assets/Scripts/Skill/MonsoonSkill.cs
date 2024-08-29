using System.Collections;
using UnityEngine;

public class MonsoonSkill : Skill
{
    private readonly Collider[] targets = new Collider[100];

    public override void Init()
    {
        SkillName = "Monsoon";
        SkillName_KR = "순풍";
        Description = "범위 내에 있는 적 미니언을 스킬 시전 위치로 당겨옵니다.";
        RequireMana = 3;
        Damage = 0;
        Radius = 10f;
        base.Init();
    }

    protected override void Activate()
    {
        GameObject effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset,
                Quaternion.Euler(-90, 0, 0)
            );

        StartCoroutine(Attack(effect.transform.position));
        base.Activate();

        Vibration.VibrateAndroid(500);
        Managers.Sound.Play(Define.Sound.Effect, "Fire Ball Spell_Skill_Sorcerer_Medium_02");
    }

    IEnumerator Attack(Vector3 position)
    {
        yield return null;

        int targetAmount = Physics.OverlapSphereNonAlloc(position, 3f, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (targets[j].CompareTag("Minion"))
            {
                StartCoroutine(MoveObjects(targets[j], position));
            }
        }
    }
    IEnumerator MoveObjects(Collider coll, Vector3 eventPositon)
    {
        float time = 0;
        Minion isStunCheck = coll.gameObject.GetComponent<Minion>();
        if (!isStunCheck.isStun) 
        {
            isStunCheck.isStun = true;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                Vector3 direction = (eventPositon - coll.gameObject.transform.position).normalized; // 반대 방향
                coll.gameObject.transform.position += direction * 16f * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            isStunCheck.isStun = false;
        }
    }
}

using System.Collections;
using UnityEngine;

public class MonsoonSkill : Skill
{
    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "Monsoon";
        SkillName_KR = "순풍";
        Description = "";
        RequireMana = 3;
        Damage = 0;
        Radius = 3f;
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

        StartCoroutine(Attack(effect.transform.position));
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
        MinionBehaviour isStunCheck = coll.gameObject.GetComponent<MinionBehaviour>();
        if (!isStunCheck.isStun) 
        {
            isStunCheck.isStun = true;
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                Vector3 direction = (coll.gameObject.transform.position - eventPositon).normalized; // 반대 방향
                coll.gameObject.transform.position += direction * 16f * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            isStunCheck.isStun = false;
        }
    }
}

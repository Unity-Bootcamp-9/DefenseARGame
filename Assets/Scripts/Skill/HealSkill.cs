using System.Collections;
using UnityEngine;

public class HealSkill : Skill
{
    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "Heal";
        SkillName_KR = "회복";
        Description = "범위 내에 있는 아군 미니언에게 회복량만큼 HP를 회복합니다.";
        RequireMana = 7;
        Damage = -30;
        Radius = 8f;
        base.Init();
    }

    protected override void Activate()
    {
        GameObject effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset + Vector3.up,
                draggingObject.transform.rotation
            );
        effect.transform.Rotate(Vector3.right * -90f);
        StartCoroutine(Heal(effect.transform.position));
        base.Activate();
    }

    IEnumerator Heal(Vector3 position)
    {
        Managers.Sound.Play(Define.Sound.Effect, "Heal");
        Vibration.VibratePop();

        int targetAmount = Physics.OverlapSphereNonAlloc(position, Radius, targets, 1 << 7);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<Minion>().GetHit(Damage);
        }
        yield break;
    }
}
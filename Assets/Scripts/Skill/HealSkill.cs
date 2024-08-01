using System.Collections;
using UnityEngine;

public class HealSkill : Skill
{
    private const float duration = 2f;

    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        SkillName = "Heal";
        SkillName_KR = "회복";
        Description = "";
        RequireMana = 7;
        Damage = 30;
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

        StartCoroutine(Heal(effect.transform.position, duration));
    }

    IEnumerator Heal(Vector3 position, float time)
    {
        yield return new WaitForSeconds(time);
        
        int targetAmount = Physics.OverlapSphereNonAlloc(position, Radius / 2, targets, 1 << 7);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<Minion>().GetHit(-Damage);
        }
    }
}
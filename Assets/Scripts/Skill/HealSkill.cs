using System.Collections;
using UnityEngine;

public class HealSkill : Skill
{
    private const float duration = 2f;

    private readonly Collider[] targets = new Collider[10];

    public override void Init()
    {
        skillName = "Heal";
        skillName_KR = "회복";
        description = "";
        requireMana = 7;
        damage = 30;
        radius = 9f;
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
        
        int targetAmount = Physics.OverlapSphereNonAlloc(position, radius / 2, targets, 1 << 7);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<MinionBehaviour>().GetHit(-damage);
        }
    }
}
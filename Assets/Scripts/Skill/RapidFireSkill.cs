using System.Collections;
using UnityEngine;

public class RapidFireSkill : Skill
{
    [Header("스킬 상세")]
    public GameObject effectToSpawn;
    [Range(1f, 5f), Tooltip("스킬이 피해를 가하는 지속 시간")] public float duration = 3f;
    [Range(0.1f, 5f), Tooltip("첫 공격까지의 대기 시간")] public float attackDelay = 1f;
    [Range(0.1f, 5f), Tooltip("다음 공격까지의 대기 시간")] public float attackRepeatRate = 0.5f;

    protected override void Activate()
    {
        ParticleSystem effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset,
                draggingObject.transform.rotation
            ).GetComponent<ParticleSystem>();

        effect.transform.localScale = Vector3.one * (Circle.Radius / 9);

        Destroy(effect.gameObject, effect.main.duration);

        StartCoroutine(RepeatShooting(effect.transform.position, attackDelay, attackRepeatRate));
    }

    IEnumerator RepeatShooting(Vector3 position, float time, float repeatRate)
    {
        yield return new WaitForSeconds(time);

        for (float i = 0; i <= duration - time; i += repeatRate)
        {
            Collider[] targets = new Collider[10];
            int targetAmount = Physics.OverlapSphereNonAlloc(position, radius / 2, targets, 1 << 6);

            for (int j = 0; j < targetAmount; j++)
            {
                if (!targets[j].CompareTag("Minion")) continue;
                targets[j].GetComponent<MinionBehaviour>().GetHit(damage);
            }

            yield return new WaitForSeconds(repeatRate);
        }
    }
}
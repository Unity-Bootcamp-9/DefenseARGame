using System.Collections;
using UnityEngine;

public class RapidFireSkill : Skill
{
    [Header("스킬 상세")]
    public GameObject effectToSpawn;
    [Range(0.1f, 5f), Tooltip("첫 공격까지의 대기 시간")] public float attackDelay = 1f;
    [Range(0.1f, 5f), Tooltip("다음 공격까지의 대기 시간")] public float attackRepeatRate = 1f;

    protected override void Activate()
    {
        ParticleSystem effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset,
                draggingObject.transform.rotation
            ).GetComponent<ParticleSystem>();

        Destroy(effect.gameObject, effect.main.duration);

        StartCoroutine(RepeatShooting(effect.transform.position, attackDelay, attackRepeatRate));
    }

    IEnumerator RepeatShooting(Vector3 position, float time, float repeatRate)
    {
        yield return new WaitForSeconds(time);

        float tick = time;

        do
        {
            tick += repeatRate;

            Collider[] targets = new Collider[10];
            int count = Physics.OverlapSphereNonAlloc(position, radius / 2, targets, 1 << 6);

            for (int i = 0; i < count; i++)
            {
                if (!targets[i].CompareTag("Minion")) continue;
                targets[i].GetComponent<MinionBehaviour>().GetHit((int)(damage / duration));
            }

            yield return new WaitForSeconds(repeatRate);
        } while (tick < duration);
    }
}
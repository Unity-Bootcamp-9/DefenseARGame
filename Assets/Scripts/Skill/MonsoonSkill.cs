using System.Collections;
using UnityEngine;

public class MonsoonSkill : Skill
{
    //public float radius = 1.38f;  // 반경
    public float pushForce = 100f; // 밀어내는 힘
    //public float duration = 3f;   // 지속 시간

    private const float duration = 1.3f;
    private readonly Collider[] targets = new Collider[10];

    private void Start()
    {
        skillName = "Monsoon";
        requireMana = 2;
        damage = 10;
        radius = 3f;
        effectToSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Skill/Monsoon");
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
        yield return null;

        int targetAmount = Physics.OverlapSphereNonAlloc(position, 3f, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (targets[j].CompareTag("Minion"))
            {
                Debug.Log(targets[j].gameObject.transform.name);
                StartCoroutine(MoveObjects(targets[j]));
            }
        }
    }
    IEnumerator MoveObjects(Collider coll)
    {
        float time = 0;
        while (time < 3)
        {
            time += Time.deltaTime;
            Vector3 direction = (coll.gameObject.transform.position - effectToSpawn.gameObject.transform.position).normalized; // 반대 방향
            coll.gameObject.transform.position += direction * 10f * Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }
    }
}
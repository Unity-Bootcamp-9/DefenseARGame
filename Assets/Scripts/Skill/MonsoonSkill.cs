using System.Collections;
using UnityEngine;
using static GoalManager;

public class MonsoonSkill : Skill
{
    private readonly Collider[] targets = new Collider[10];

    private void Start()
    {
        skillName = "Monsoon";
        requireMana = 4;
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
                //Debug.Log(effectToSpawn.gameObject.transform.position);
                StartCoroutine(MoveObjects(targets[j], position));
            }
        }
    }
    IEnumerator MoveObjects(Collider coll, Vector3 eventPositon)
    {
        float time = 0;
        MinionBehaviour isSturnCheck = coll.gameObject.GetComponent<MinionBehaviour>();
        if (!isSturnCheck.isSturn) 
        {
            isSturnCheck.isSturn = true;
            Debug.Log(isSturnCheck.isSturn);
            while (time < 0.5f)
            {
                time += Time.deltaTime;
                Vector3 direction = (coll.gameObject.transform.position - eventPositon).normalized; // 반대 방향
                coll.gameObject.transform.position += direction * 16f * Time.deltaTime;
                yield return null; // 다음 프레임까지 대기
            }
            isSturnCheck.isSturn = false;
        }
    }
}

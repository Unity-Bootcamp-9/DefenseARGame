using System.Collections;
using UnityEngine;

public class MeteorSkill : Skill
{
    private const float duration = 1.3f;

    private readonly Collider[] targets = new Collider[10];

    private void Start()
    {
        skillName = "Meteor";
        requireMana = 4;
        damage = 50;
        effectToSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Skill/Meteor");
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
        yield return new WaitForSeconds(time);
        
        int targetAmount = Physics.OverlapSphereNonAlloc(position, radius / 2, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (!targets[j].CompareTag("Minion")) continue;
            targets[j].GetComponent<MinionBehaviour>().GetHit(damage);
        }
    }
}
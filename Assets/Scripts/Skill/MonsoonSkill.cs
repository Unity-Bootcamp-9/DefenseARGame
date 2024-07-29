using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

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
        //StartCoroutine(ActivateUltimate());
    }

    IEnumerator Attack(Vector3 position, float time)
    {
        yield return null;

        int targetAmount = Physics.OverlapSphereNonAlloc(position, 3f, targets, 1 << 6);

        for (int j = 0; j < targetAmount; j++)
        {
            if (targets[j].CompareTag("Minion"))
            {
                //targets[j].transform.position = Vector3.zero;
                Rigidbody rb = targets[j].GetComponent<Rigidbody>();

                Debug.Log(rb.gameObject.transform.name);

                // 오브젝트의 forward 방향의 반대 방향
                Vector3 pushDirection = -targets[j].transform.forward;

                // 반대 방향으로 밀어내기
                rb.AddForce(pushDirection * 100f * Time.deltaTime, ForceMode.Impulse);

            }
        }
    }

    private IEnumerator ActivateUltimate()
    {
        float startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            // 반경 내의 모든 Collider를 감지
            Collider[] colliders = Physics.OverlapSphere(transform.position, radius, 1 << 6);

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Minion"))
                {
                    Rigidbody rb = col.GetComponent<Rigidbody>();
                    // 오브젝트에서 잔나의 위치를 뺀 벡터
                    Vector3 direction = col.transform.position - transform.position;

                    // 반대 방향으로 밀어내기
                    rb.AddForce(direction.normalized * pushForce * Time.deltaTime, ForceMode.Impulse);
                }
            }

            yield return null; // 다음 프레임까지 대기
        }
    }
}
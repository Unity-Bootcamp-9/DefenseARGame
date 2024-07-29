using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MonsoonSkill : Skill
{
    private const float duration = 1.3f;
    private SphereCollider monsoonCollider;

    private void Start()
    {
        skillName = "Monsoon";
        requireMana = 2;
        damage = 10;
        effectToSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Skill/Monsoon");
        monsoonCollider = effectToSpawn.GetComponent<SphereCollider>();
    }

    protected override void Activate()
    {
        GameObject effect = Instantiate
            (
                effectToSpawn,
                draggingObject.transform.position - offset,
                draggingObject.transform.rotation
            );

        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.38f, 1 << 6);
        TargetSelection(colliders, transform);
    }

    IEnumerator colliderBigging()
    {
        float detectionRange = 0;

        while (detectionRange < 1.38f)    //monsoonCollider.radius < 1.38
        {
            yield return new WaitForSeconds(0.2f);
            detectionRange += 0.1f;
        }
        detectionRange = 0;
    }

    public void TargetSelection(Collider[] colliders, Transform thisTransform)
    {

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Minion"))
            {
                Debug.Log("TargetSelection");
                Vector3 vTargetPos = collider.gameObject.transform.position;
                Vector3 vPos = transform.position;
                Vector3 vDist = vTargetPos - vPos;
                Vector3 vDir = vDist.normalized;
                float fDist = vDist.magnitude;
                //vTargetPos += -vDir * 3f * 0.1f;    // 이걸 또 코루틴?
                MonsoonStart(vTargetPos, vDir);

            }
        }
/*
        enemyMinions = enemyMinions.OrderBy(enemyMinion => Vector3.Distance(enemyMinion.position, thisTransform.position)).ToList<Transform>();

        if (enemyMinions.Count > 0)
        {
            target = enemyMinions[0];
        }
        enemyMinions.Clear();
*/
    }


    IEnumerator MonsoonCo(Vector3 vTargetPos, Vector3 vDir)
    {
        while (true)
        {
            Debug.Log("hello");
            vTargetPos += -vDir * 3f * 0.1f;    // 이걸 또 코루틴?
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void MonsoonStart(Vector3 vTargetPos, Vector3 vDir)
    {
        Debug.Log("MonsoonStart");

        StartCoroutine(MonsoonCo(vTargetPos, vDir));
    }

/*    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1.38f, 1 << 6);

        TargetSelection(colliders, transform);
    }
*/}
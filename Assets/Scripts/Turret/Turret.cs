using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] public float turretHP;
    public Transform? targetTr;
    public bool isAttack = false;
    public int enemyMinion;
    public string enemyLayerName;
    public int layerMask;

    #region States
    public TurretStateMachine stateMachine { get; private set; }
    public TurretDetectionState detectionState { get; private set; }
    #endregion

    #region Animator
    public Animator anim { get; private set; }
    #endregion

    public void Awake()
    {
        anim = GetComponent<Animator>();
        stateMachine = new TurretStateMachine();
        turretHP = 100;
        anim.SetFloat("currentHP", turretHP);
        enemyMinion = LayerMask.NameToLayer(enemyLayerName);
        layerMask = (1 << enemyMinion);
    }

    public void DetectMinion()
    {
        Collider?[] colls = Physics.OverlapSphere(gameObject.transform.position, 15f, layerMask);

        if(colls.Length == 0)
        {
            anim.SetBool("attackStart", false);
            targetTr = null;
            ReferenceEquals(targetTr, null);
        }
        else
        {
            foreach (Collider coll in colls)
            {
                if(coll.name == "mixamorig:Hips")
                {
                    anim.SetBool("attackStart", true);

                    isAttack = true;
                    Debug.Log("mixamorig:Hips");
                    targetTr = coll.gameObject.transform;
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Weapon"))
        {
            Debug.Log("GetHurt");
        }
    }
}
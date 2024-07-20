using System.Collections;
using UnityEngine;

public class TurretAnimationTrigger : MonoBehaviour
{
    private Turret turret => GetComponentInParent<Turret>();
    private void AnimationTrigger()
    {
        turret.AnimationTrigger();
    }
}
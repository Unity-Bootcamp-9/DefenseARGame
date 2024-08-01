using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(SphereCollider), typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    enum Childs
    {
        Projectile, Hit, Flash, Trail
    }

    private readonly Dictionary<Childs, ParticleSystem> _parts = new Dictionary<Childs, ParticleSystem>();

    private void Awake()
    {
        if (transform.childCount == 0) return;

        Transform visual = transform.GetChild(0);

        if (Enum.GetValues(typeof(Childs)).Length != visual.childCount)
        {
            Debug.LogError("Not equal");
            return;
        }

        for (int i = 0; i < visual.childCount; i++)
        {
            _parts.Add((Childs)i, visual.GetChild(i).GetComponent<ParticleSystem>());
        }
    }

    private void OnEnable()
    {
        _parts[Childs.Flash].transform.parent = null;
        _parts[Childs.Flash].Play();
        Managers.Sound.Play(Define.Sound.Effect, "Fire_Magic_Whoosh_Fast-007");
    }

    private void OnDisable()
    {
        _parts[Childs.Hit].transform.parent = null;
        _parts[Childs.Hit].transform.position = transform.position;
        _parts[Childs.Hit].Play();
        _parts[Childs.Trail].Stop();
        Managers.Sound.Play(Define.Sound.Effect, "Fire_Punch_Combat-006");
    }
}

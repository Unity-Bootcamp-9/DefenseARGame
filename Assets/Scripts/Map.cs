using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Map : MonoBehaviour
{

    [SerializeField] private NavMeshSurface map;

    private void Awake()
    {
        map.BuildNavMesh();
    }


}

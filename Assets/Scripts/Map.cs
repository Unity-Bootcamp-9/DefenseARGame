using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Map : MonoBehaviour
{

    NavMeshSurface map;

    private void Awake()
    {
        map = GetComponent<NavMeshSurface>();
        map.BuildNavMesh();
    }


}

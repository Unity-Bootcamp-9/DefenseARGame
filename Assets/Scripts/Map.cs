using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class Map : MonoBehaviour
{

    //[SerializeField]
    private NavMeshSurface map;

    private void Start()
    {
        map = GetComponent<NavMeshSurface>();
        map.RemoveData();
        map.BuildNavMesh();

        Managers.Resource.Load<Material>("Materials/M_Plane").color = Color.clear;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;

public class NavMeshGenerator : MonoBehaviour
{

    public NavMeshSurface surface;

    // Start is called before the first frame update
    void Start()
    {
        surface.BuildNavMesh();
    }
}

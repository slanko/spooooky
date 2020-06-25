using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCmats : MonoBehaviour
{

    public Material[] possibleMaterials;
    SkinnedMeshRenderer myMesh;
    void Start()
    {
        myMesh = GetComponent<SkinnedMeshRenderer>();
        myMesh.material = possibleMaterials[Random.Range(0, possibleMaterials.Length)];
    }
}
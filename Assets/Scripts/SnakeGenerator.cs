using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SnakeGenerator : MonoBehaviour
{
    private MeshFilter filter;
    private SkinnedMeshRenderer rend;
    private RopeMesh ropeMesh;

    void Start()
    {
        var children = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            var child = transform.GetChild(i);
            children.Add(child);
        }
        ropeMesh = new RopeMesh(transform, children, 36, 1);

        filter = GetComponent<MeshFilter>();
        filter.mesh = ropeMesh.mesh;
        filter.mesh.boneWeights = ropeMesh.weights.ToArray();
        filter.mesh.bindposes = ropeMesh.bindPoses.ToArray();


        rend = GetComponent<SkinnedMeshRenderer>();
        rend.sharedMesh = filter.mesh;
        rend.material.mainTextureScale = new Vector2(transform.childCount, 1);
        rend.bones = ropeMesh.bones.ToArray();
    }

    public void AddRopePoint(Transform child)
    {
        ropeMesh.AddRopePoint(transform, child);
        filter.mesh = ropeMesh.mesh;
        filter.mesh.boneWeights = ropeMesh.weights.ToArray();
        filter.mesh.bindposes = ropeMesh.bindPoses.ToArray();


        rend.sharedMesh = filter.mesh;
        rend.material.mainTextureScale = new Vector2(transform.childCount, 1);
        rend.bones = ropeMesh.bones.ToArray();
    }
}

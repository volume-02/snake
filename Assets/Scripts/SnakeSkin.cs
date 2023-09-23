using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SnakeSkin : MonoBehaviour
{
    public int edgeCount = 36;
    public float radius = 1;

    private int prevCount = 0;
    private MeshFilter filter;
    private SkinnedMeshRenderer rend;

    private Mesh mesh;

    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector2> uv = new List<Vector2>();
    private List<int> triangles = new List<int>();
    private List<BoneWeight> weights = new List<BoneWeight>();
    private List<Matrix4x4> bindPoses = new List<Matrix4x4>();
    private List<Transform> bones = new List<Transform>();

    void Start()
    {
        filter = GetComponent<MeshFilter>();
        rend = GetComponent<SkinnedMeshRenderer>();
        mesh = new Mesh();

        Calculate();
    }

    void AddPoint(Transform transform, int index)
    {
        var point = transform.gameObject.AddComponent<RopePoint>();
        point.index = index;
        point.Calculate();

        vertices.AddRange(point.circleVerticies);

        if (point.index > 0)
        {
            int prevPointIndex = point.index - 1;
            int pointOffset = point.index * edgeCount;
            int prevPointOffset = prevPointIndex * edgeCount;

            for (int i = 0; i < edgeCount; i++)
            {
                int vertIndex = i;
                int nextVertIndex = (i + 1) % edgeCount;


                int a = prevPointOffset + vertIndex;
                int b = prevPointOffset + nextVertIndex;
                int c = pointOffset + vertIndex;
                int d = pointOffset + nextVertIndex;

                triangles.Add(c);
                triangles.Add(b);
                triangles.Add(a);

                triangles.Add(d);
                triangles.Add(b);
                triangles.Add(c);
            }
        }

        weights.AddRange(point.weights);
        bindPoses.Add(point.bindPose);
        bones.Add(transform);
    }

    void CalculateMesh()
    {
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();

        filter.mesh = mesh;
        filter.mesh.boneWeights = weights.ToArray();
        filter.mesh.bindposes = bindPoses.ToArray();

        rend.sharedMesh = filter.mesh;
        rend.material.mainTextureScale = new Vector2(0.3f * transform.childCount, 1);
        rend.bones = bones.ToArray();
    }

    void CalculateUV()
    {
        uv = new List<Vector2>();
        for (int pointIndex = 0; pointIndex < transform.childCount; pointIndex++)
        {
            var child = transform.GetChild(pointIndex);
            var point = child.gameObject.GetComponent<RopePoint>();
            point.calculateUV();
            uv.AddRange(point.circleUV);
        }

    }

    void Calculate()
    {
        for (int pointIndex = 0; pointIndex < transform.childCount; pointIndex++)
        {
            var child = transform.GetChild(pointIndex);
            if (child.gameObject.GetComponent<RopePoint>() == null)
            {
                AddPoint(child, pointIndex);
            }
        }
        CalculateUV();
        CalculateMesh();
        prevCount = transform.childCount;
        if (transform.childCount > 3)
        {
            var pieces = (transform.childCount - 2) / 2;
            var splint = 1f / pieces;
            float size = 0;
            int i = 1;
            for (; i < 1 + pieces; i++)
            {
                var child = transform.GetChild(i);
                size += splint;
                child.localScale = new Vector3(1.5f+size, 1.5f + size, 1.5f + size);

            }
            if((transform.childCount - 2) % 2 > 0)
            {
                i++;
                var child = transform.GetChild(i);
                child.localScale = new Vector3(1.5f + size, 1.5f + size, 1.5f + size);
            }
            for (; i < transform.childCount - 1; i++)
            {
                var child = transform.GetChild(i);
                child.localScale = new Vector3(1.5f + size, 1.5f + size, 1.5f + size);
                size -= splint;
            }
        }
    }

    private void FixedUpdate()
    {
        if (prevCount != transform.childCount)
        {
            Calculate();
        }
    }
}

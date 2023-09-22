using System.Collections.Generic;
using UnityEngine;

public class RopeMesh
{
    private int edgeCount { get; set; }
    private float radius { get; set; }
    public Mesh mesh { get; set; }
    public List<BoneWeight> weights { get; set; } = new List<BoneWeight>();
    public List<Matrix4x4> bindPoses { get; set; } = new List<Matrix4x4>();
    public List<Transform> bones { get; set; } = new List<Transform>();
    public List<RopePoint> points { get; set; } = new List<RopePoint>();

    public RopeMesh(Transform parent, List<Transform> children, int edgeCount, float radius)
    {
        this.edgeCount = edgeCount;
        this.radius = radius;
        for (int i = 0; i < children.Count; i++)
        {
            var child = children[i];
            var point = new RopePoint(parent, child, i, children.Count, edgeCount, radius);
            points.Add(point);
        }

        var vertices = new List<Vector3>();
        var uv = new List<Vector2>();

        foreach (var point in points)
        {
            vertices.AddRange(point.circleVerticies);
            uv.AddRange(point.circleUV);
            weights.AddRange(point.weights);
            bindPoses.Add(point.bindPose);
            bones.Add(point.transform);
        }


        var triangles = new List<int>();

        for (int pointIndex = 1; pointIndex < points.Count; pointIndex++)
        {
            int prevPointIndex = pointIndex - 1;
            int pointOffset = pointIndex * edgeCount;
            int prevPointOffset = prevPointIndex * edgeCount;

            for (int i = 0; i < 36; i++)
            {
                int vertIndex = i;
                int nextVertIndex = (i + 1) % edgeCount;

                int a = prevPointOffset + vertIndex;
                int b = prevPointOffset + nextVertIndex;
                int c = pointOffset + vertIndex;
                int d = pointOffset + nextVertIndex;

                triangles.Add(a);
                triangles.Add(b);
                triangles.Add(c);

                triangles.Add(c);
                triangles.Add(b);
                triangles.Add(d);
            }
        }

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.uv = uv.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();
    }

    public void AddRopePoint(Transform parent, Transform child)
    {
        var point = new RopePoint(parent, child, points.Count, points.Count + 1, edgeCount, radius);

        var vertices = new List<Vector3>();

        vertices.AddRange(mesh.vertices);
        vertices.AddRange(point.circleVerticies);

        weights.AddRange(point.weights);
        bindPoses.Add(point.bindPose);
        bones.Add(point.transform);

        var triangles = new List<int>();
        triangles.AddRange(mesh.triangles);

        var pointIndex = points.Count;
        int prevPointIndex = pointIndex - 1;
        int pointOffset = pointIndex * edgeCount;
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

        mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.RecalculateNormals();


        points.Add(point);

        var uv = new List<Vector2>();
        foreach (var p in points)
        {
            p.RecalculateUV(points.Count);
            uv.AddRange(p.circleUV);
        }
        mesh.uv = uv.ToArray();
    }
}

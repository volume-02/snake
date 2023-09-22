using System.Collections.Generic;
using UnityEngine;

public class RopePoint
{
    private int edgeCount { get; set; }
    public int index { get; set; }
    public Transform transform { get; set; }
    public List<Vector3> circleVerticies { get; set; } = new List<Vector3>();
    public List<Vector2> circleUV { get; set; } = new List<Vector2>();
    public List<BoneWeight> weights { get; set; } = new List<BoneWeight>();
    public Matrix4x4 bindPose { get; set; }

    public RopePoint(Transform parent, Transform transform, int index, int pointCount, int edgeCount, float radius)
    {
        this.edgeCount = edgeCount;
        this.transform = transform;
        this.index = index;

        for (int i = 0; i < edgeCount; i++)
        {
            float angle = i * 360f / edgeCount;
            var x = transform.position.x + radius * Mathf.Cos(Mathf.Deg2Rad * angle);
            var y = transform.position.y + radius * Mathf.Sin(Mathf.Deg2Rad * angle);

            circleVerticies.Add(new Vector3(x, y, transform.position.z));
            circleUV.Add(new Vector2(index * (1.0f / pointCount), i * (1.0f / edgeCount)));

            var w = new BoneWeight();
            w.boneIndex0 = index;
            w.weight0 = 1;
            weights.Add(w);
        }

        bindPose = transform.worldToLocalMatrix * parent.localToWorldMatrix;
    }

    public void RecalculateUV(int pointCount)
    {
        for (int i = 0; i < edgeCount; i++)
        {
            circleUV[i] = new Vector2(index * (1.0f / pointCount), i * (1.0f / edgeCount));
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class RopePoint : MonoBehaviour
{
    private SnakeSkin parent { get; set; }

    public int index { get; set; }
    public List<Vector3> circleVerticies { get; set; } = new List<Vector3>();
    public List<Vector2> circleUV { get; set; } = new List<Vector2>();
    public List<BoneWeight> weights { get; set; } = new List<BoneWeight>();
    public Matrix4x4 bindPose { get; set; }

    public void Calculate()
    {
        parent = transform.parent.GetComponent<SnakeSkin>();
        for (int i = 0; i < parent.edgeCount; i++)
        {
            float angle = i * 360f / parent.edgeCount;
            var x = transform.position.x + parent.radius * Mathf.Cos(Mathf.Deg2Rad * (angle-90));
            var y = transform.position.y + 0.8f*parent.radius * Mathf.Sin(Mathf.Deg2Rad * (angle - 90));

            circleVerticies.Add(new Vector3(x, y, transform.position.z));
            var w = new BoneWeight();
            w.boneIndex0 = index;
            w.weight0 = 1;
            weights.Add(w);
        }

        calculateUV();


        var rott = transform.rotation;
        transform.rotation = Quaternion.identity;
        bindPose = transform.worldToLocalMatrix * parent.transform.localToWorldMatrix;
        transform.rotation = rott;
    }

    public void calculateUV()
    {
        circleUV = new List<Vector2>();
        for (int i = 0; i < parent.edgeCount; i++)
        {
            circleUV.Add(new Vector2(index * (1.0f / parent.transform.childCount), i * (1.0f / parent.edgeCount)));
        }
    }
}

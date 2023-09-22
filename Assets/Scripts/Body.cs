using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject next;
    public Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        var rend = GetComponent<MeshRenderer>();
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        var offsetValue = GetComponent<MeshRenderer>().bounds.size.x;
        var diff = next.transform.position - transform.position;
        var diggLength = diff.magnitude;
        transform.LookAt(next.transform);
        transform.Translate(Vector3.forward * (diggLength - offsetValue));

        direction = transform.forward;
    }
}

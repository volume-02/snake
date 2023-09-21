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
    }

    // Update is called once per frame
    void Update()
    {
        var offsetValue = GetComponent<MeshRenderer>().bounds.size.x / 2;
        var diff = next.transform.position - transform.position;
        var trans = diff - diff.normalized * offsetValue;
        transform.Translate(trans);

        direction = diff.normalized;
    }
}

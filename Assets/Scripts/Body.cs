using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Body : MonoBehaviour
{
    public GameObject next;
    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        CalculateDirection();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(direction * Time.deltaTime * 5);

        if (direction == Vector3.zero ||
            direction == Vector3.forward && transform.position.z >= next.transform.position.z ||
            direction == Vector3.back && transform.position.z <= next.transform.position.z ||
            direction == Vector3.right && transform.position.x >= next.transform.position.x ||
            direction == Vector3.left && transform.position.x <= next.transform.position.x

            )
        {
            CalculateDirection();
        }
    }

    private void CalculateDirection()
    {
        float horizontalInput = next.transform.position.x - transform.position.x ;
        float verticalInput = next.transform.position.z - transform.position.z;



        Debug.Log($"horiz: {horizontalInput}, vert: {verticalInput}");

        horizontalInput = Mathf.Abs(horizontalInput) < 0.1 ? 0 : horizontalInput;
        verticalInput = Mathf.Abs(verticalInput) < 0.1 ? 0 : verticalInput;

        if (verticalInput > 0)
        {
            direction = Vector3.forward;
        } else if (verticalInput < 0)
        {
            direction = Vector3.back;
        }
        else if (horizontalInput > 0)
        {
            direction = Vector3.right;
        }
        else if (horizontalInput < 0)
        {
            direction = Vector3.left;
        }   
    }
}

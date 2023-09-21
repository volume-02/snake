using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Vector3 headDirection = Vector3.forward;
    public GameObject BodyPrefab;
    private GameObject tail;

    private Vector3 tailDirection;
    // Start is called before the first frame update
    void Start()
    {
        tail = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        MoveHead();
        tailDirection = tail?.GetComponent<Body>()?.direction ?? headDirection;

    }

    private void MoveHead()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");


        if (verticalInput > 0 && headDirection != Vector3.back)
        {
            headDirection = Vector3.forward;
        }
        if (verticalInput < 0 && headDirection != Vector3.forward)
        {
            headDirection = Vector3.back;
        }

        if (horizontalInput > 0 && headDirection != Vector3.left)
        {
            headDirection = Vector3.right;
        }
        if (horizontalInput < 0 && headDirection != Vector3.right)
        {
            headDirection = Vector3.left;
        }

        transform.Translate(headDirection * speed * Time.deltaTime);
    }

    private Vector3 CalculateOffset()
    {
        var offsetValue = GetComponent<MeshRenderer>().bounds.size.x;

        return tail.transform.position - offsetValue * tailDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            var newBody = Instantiate(BodyPrefab, CalculateOffset(), Quaternion.identity);
            var bodyScript = newBody.GetComponent<Body>();
            bodyScript.next = tail;
            tail = newBody;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    MeshRenderer rend;
    public float speed = 5;
    public GameObject BodyPrefab;
    public GameObject tailObject;

    float prevSpeed;
    Vector3 headDirection = Vector3.forward;
    GameObject tail;

    int bodyCount = 1;

    Vector3 tailDirection;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<MeshRenderer>();
        tail = gameObject;
        rend.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        MoveHead();
        tailDirection = tail?.GetComponent<Body>()?.direction ?? headDirection;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            prevSpeed = speed;
            speed *= 2;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            speed = prevSpeed;
        }

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

        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(headDirection), 180*Time.deltaTime);
        transform.Translate(headDirection * speed * Time.deltaTime, Space.World);
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
            newBody.transform.parent = transform.parent;
            bodyScript.id = bodyCount;
            bodyCount++;
            tail = newBody;
            tailObject.transform.parent = tail.transform;
            tailObject.transform.localPosition = Vector3.zero;
            tailObject.transform.localRotation = Quaternion.identity;

            speed += 0.2f;
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Body") &&
            other.GetComponent<Body>().id != 1 &&
            other.GetComponent<Body>().id != 2
            )
        {
            speed = 0;
        }
    }

}

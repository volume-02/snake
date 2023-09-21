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
    private Vector3 lastTailPos;
    // Start is called before the first frame update
    void Start()
    {
        tail = gameObject;
        lastTailPos = tail.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveHead();
        var currTailPos = tail.transform.position;
        CalculateTailDirection(currTailPos, lastTailPos);
        lastTailPos = tail.transform.position;

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

    private void CalculateTailDirection(Vector3 currPos, Vector3 lastPos)
    {
        var zDirection = Mathf.Abs(lastPos.z) < currPos.z ? Vector3.forward : Vector3.back;
        var xDirection = Mathf.Abs(lastPos.x) < currPos.x ? Vector3.right : Vector3.left;

        if (lastPos.z == currPos.z)
        {
            tailDirection = xDirection;
        }
        else if (lastPos.x == currPos.x)
        {
            tailDirection = zDirection;
        }
    }

    private Vector3 CalculateOffset()
    {
        var offsetValue = GetComponent<MeshRenderer>().bounds.size.x;
       
        if (tailDirection == Vector3.forward)
        {
            Debug.Log("forward");
            return tail.transform.position - new Vector3(0, 0, offsetValue);

        }
        else if (tailDirection == Vector3.back)
        {

            Debug.Log("back");
            return tail.transform.position - new Vector3(0, 0, -offsetValue);
        }
        else if (tailDirection == Vector3.right)
        {

            Debug.Log("right");
            return tail.transform.position - new Vector3(offsetValue, 0, 0);
        }
        else if (tailDirection == Vector3.left)
        {

            Debug.Log("left");
            return tail.transform.position - new Vector3(-offsetValue, 0, 0);
        } 
        return Vector3.zero;
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

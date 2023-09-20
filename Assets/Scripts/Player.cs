using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    private Vector3 direction = Vector3.forward;
    public GameObject BodyPrefab;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        CalculateDirection();

    }

    private void CalculateDirection()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        if (verticalInput > 0)
        {
            direction = Vector3.forward;
        }
        if (verticalInput < 0)
        {
            direction = Vector3.back;
        }

        if (horizontalInput > 0)
        {
            direction = Vector3.right;
        }
        if (horizontalInput < 0)
        {
            direction = Vector3.left;
        }

        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);
        //Instantiate(BodyPrefab);
    }
        
}

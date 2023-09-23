using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyPowerup());

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeSelf)
        {
            StopCoroutine(DestroyPowerup());
        }
    }

    IEnumerator DestroyPowerup()
    {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }
}

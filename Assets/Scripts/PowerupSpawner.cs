using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;
    private GameObject powerup;

    int bound = 18;

    void Start()
    {
        powerup = Instantiate(powerupPrefab, GetSpawnPoint(), Quaternion.identity);
        powerup.transform.parent = transform;
        StartCoroutine(SpawnPowerup());
    }

    IEnumerator SpawnPowerup()
    {
        while (true)
        {
            while (powerup != null)
            {
                yield return null;
            }
                yield return new WaitForSeconds(3);
                powerup = Instantiate(powerupPrefab, GetSpawnPoint(), Quaternion.identity);
        }
    }

    private Vector3 GetSpawnPoint()
    {
        return new Vector3(Random.Range(-bound, bound), 1, Random.Range(-bound, bound));
    }
}

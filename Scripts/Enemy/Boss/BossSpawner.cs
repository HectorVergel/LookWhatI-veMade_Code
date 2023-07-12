using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public GameObject boss;
    public float timeToSpawn;
    public void Spawn()
    {
        StartCoroutine(SpawnBoss());
    }

    IEnumerator SpawnBoss()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(timeToSpawn);
        Instantiate(boss,spawnPoint.position,Quaternion.identity);
    }
    
}

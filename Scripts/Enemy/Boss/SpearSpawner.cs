 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearSpawner : MonoBehaviour
{
    public GameObject spear;
    public float maxCooldown;
    public float minCooldown;
    public float maxDistanceToPlayer;
    public float frontPercent;
    public float minPosX;
    public float maxPosX;
    public float spawnPosY;
    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position,transform.position + new Vector3(maxDistanceToPlayer,0,0));
    }
    public void ActivateSpears()
    {
        StartCoroutine(SpawnSpear());
    }

    public void DesactivateSpears()
    {
        StopAllCoroutines();
        SpearController[] spears = FindObjectsOfType<SpearController>();
        foreach (var spear in spears)
        {
            Destroy(spear.gameObject);
        }
    }

    IEnumerator SpawnSpear()
    {
        float time = 0;
        float cooldown = Random.Range(minCooldown,maxCooldown);
        while(time < cooldown)
        {
            time+=Time.deltaTime;
            yield return null;
        }
        GameObject player = FindObjectOfType<HealthSystemPlayer>().gameObject;
        float spawnPosX;
        float rnd = Random.Range(1,100);
        if(rnd < frontPercent)
        {
            spawnPosX = player.GetComponent<BoxCollider2D>().bounds.center.x + Random.Range(0,maxDistanceToPlayer * player.transform.right.x);
        }
        else
        {
            spawnPosX = player.GetComponent<BoxCollider2D>().bounds.center.x - Random.Range(0,maxDistanceToPlayer * player.transform.right.x);
        }
        if(spawnPosX > maxPosX)
        {
            spawnPosX = maxPosX;
        }
        if(spawnPosX < minPosX)
        {
            spawnPosX = minPosX;
        }
        var s = Instantiate(spear, new Vector3(spawnPosX,spawnPosY,0),Quaternion.identity);
        StartCoroutine(SpawnSpear());
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackCage : MonoBehaviour
{
    public GameObject cagePrefab;
    public Transform cageSpawnPoint;
    public float timeToInstantiate;
    public static BossAttackCage instance;
    AudioSource myAudio;
    public AudioClip attackSound;
    private void Awake() {
        instance = this;
        myAudio = GetComponent<AudioSource>();
    }
    public void StartAttack()
    {
        StartCoroutine(WaitToInstantiate());
    }
    IEnumerator WaitToInstantiate()
    {
        yield return new WaitForSeconds(timeToInstantiate);
        myAudio.PlayOneShot(attackSound);
        var go = Instantiate(cagePrefab,cageSpawnPoint.position,Quaternion.identity);
        go.GetComponent<Cage>().direction = (int)transform.right.x;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
    public int rockNumber;
    public AudioSource audioSource;
    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
    public ParticleSystem vfx3;
    public ParticleSystem vfx4;
    public ParticleSystem vfx5;
    public AudioClip sound;
    public int minGold;
    public int maxGold;
    public GameObject goldDropped;
    public float offsetGoldSpawn;
    private void Start() {
        vfx1.Stop();
        vfx2.Stop();
        vfx3.Stop();
        vfx4.Stop();
        vfx5.Stop();
    }
    private void OnEnable() 
    {
        if(PlayerPrefs.GetInt("Rock"+rockNumber.ToString(),0) == 1)
        {
            Destroy(audioSource.gameObject);
        }
    }
    public void DestroyObject()
    {
        int coinsGiven = Random.Range(minGold,maxGold);


        Collider2D box = GetComponent<Collider2D>();
        Vector3 goldSpawnPos = new Vector3(box.bounds.center.x,box.bounds.center.y,0);
        var a = Instantiate(goldDropped,goldSpawnPos,Quaternion.identity);
        a.GetComponent<CoinSpawner>().gold = coinsGiven;
        a.GetComponent<CoinSpawner>().ID = Random.Range(-100000, 100000);

        PlayerPrefs.SetInt("Rock"+rockNumber.ToString(),1);
        audioSource.PlayOneShot(sound);
        vfx1.Play();
        vfx2.Play();
        vfx3.Play();
        vfx4.Play();
        vfx5.Play();
        Destroy(gameObject);
    }
}

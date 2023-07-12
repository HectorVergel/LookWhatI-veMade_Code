using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineBox : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] ParticleSystem vfx1;
    [SerializeField] ParticleSystem vfx2;
    [SerializeField] ParticleSystem vfx3;
    [SerializeField] ParticleSystem vfx4;
    public AudioClip sound;
    public AudioSource audioSource;
    public int minGold;
    public int maxGold;
    public GameObject goldDropped;
    public float offsetGoldSpawn;
    private void Start()
    {
        vfx1.Stop();
        vfx2.Stop();
        vfx3.Stop();
        vfx4.Stop();
    }
    public void DestroyObject()
    {
        int coinsGiven = Random.Range(minGold,maxGold);
        Collider2D box = GetComponent<Collider2D>();
        Vector3 goldSpawnPos = new Vector3(box.bounds.center.x,box.bounds.center.y,0);
        var a = Instantiate(goldDropped,goldSpawnPos,Quaternion.identity);
        a.GetComponent<CoinSpawner>().gold = coinsGiven;
        a.GetComponent<CoinSpawner>().ID = Random.Range(-100000, 100000);

        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
        Destroy(gameObject);
        vfx1.Play();
        vfx2.Play();
        vfx3.Play();
        vfx4.Play();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spear : MonoBehaviour
{
    public int damageOnFall;
    public int hitsToDestroy;
    public float velocityOnFall;
    public GameObject colliderSpear;
    Rigidbody2D rb;
    public bool falling;
    Animator myAnim;
    SpriteRenderer spriteRenderer;
    public Sprite breakLvl1;
    public Sprite breakLvl2;
    public GameObject vfx;
    public Transform vfxStartPoint;
    AudioSource audioSource;
    public AudioClip spearHit;
    public AudioClip spearFall;
    public GameObject audioDiePrefab;
    public int minGold;
    public int maxGold;
    public GameObject goldDropped;
    public float offsetGoldSpawn;
    bool dead;

    private void Start() {
       
        falling = true;
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0,-velocityOnFall * Time.fixedDeltaTime);
        myAnim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
   
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && falling)
        {
            other.GetComponent<HealthSystemPlayer>().TakeDamage(damageOnFall);
        }
        if(other.tag == "SpearTrigger" && falling)
        {
            PlaySound(spearFall);
            Shake.instance.StartCoroutine(Shake.instance.shake());
            colliderSpear.SetActive(true);
            falling = false;
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.velocity = new Vector2(0,0);
            rb.gravityScale = 0;
            GetComponent<BoxCollider2D>().isTrigger = false;
        }
    }

    public void TakeDamage()
    {
        if(!falling)
        {

            myAnim.Play("hit");
            hitsToDestroy--;
            if(hitsToDestroy <= 0)
            {
                OnDie();
            }
            else if(!dead)
            {
                HitVisuals();
            }
        }
    }
    void PlaySound(AudioClip sound)
    {
        audioSource.pitch = 1 + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }

    void HitVisuals()
    {
        PlaySound(spearHit);
        if(hitsToDestroy == 2)
        {
            spriteRenderer.sprite = breakLvl1;
        }
        if(hitsToDestroy == 1)
        {
            spriteRenderer.sprite = breakLvl2;
        }
        //cambiar sprite a uno mas roto??
    }
    public void OnDie()
    {
        //activar particulas de la barra partida en trozos + sonido
        dead = true;
        int coinsGiven = Random.Range(minGold,maxGold);


        Collider2D box = GetComponent<Collider2D>();
        Vector3 goldSpawnPos = new Vector3(box.bounds.center.x,box.bounds.center.y,0);
        var a = Instantiate(goldDropped,goldSpawnPos,Quaternion.identity);
        a.GetComponent<CoinSpawner>().gold = coinsGiven;
        a.GetComponent<CoinSpawner>().ID = Random.Range(-100000, 100000);
        Instantiate(vfx, vfxStartPoint.position, Quaternion.identity);
        Instantiate(audioDiePrefab,transform.position,Quaternion.identity);
        Destroy(this.gameObject);  
    }
}

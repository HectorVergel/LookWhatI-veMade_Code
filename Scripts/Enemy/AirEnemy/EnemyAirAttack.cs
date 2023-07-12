using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAirAttack : MonoBehaviour, IHaveHealth
{
    public GameObject goldDropped;
    public float offsetGoldSpawn;
    public float manaPercentOnDie;
    public int damage;
    public float bigRadius;
    public float velocity;
    public int gold;
    public int exp;
    public bool dead = false;
    AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip soundDie;
    BoxCollider2D player;
    public Collider2D myCollider;
    public LayerMask whatIsPlayer;
    public float attackRange;
    private void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(myCollider.bounds.center, attackRange);
    }
    private void Update() {
        if(player != null)
        {
            CheckIfCollidingWithPlayer();
        }
        else
        {
            if(FindObjectOfType<HealthSystemPlayer>() != null)
            {
                player = FindObjectOfType<HealthSystemPlayer>().GetComponent<BoxCollider2D>();
            }
        }
    }
    void CheckIfCollidingWithPlayer()
    {
        if(!dead)
        {
            Collider2D[] playerCollider = Physics2D.OverlapCircleAll(myCollider.bounds.center,attackRange,whatIsPlayer);
            foreach (var item in playerCollider)
            {   
                if(item!= null)
                {
                    item.gameObject.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
                    dead = true;
                    StartCoroutine(Die());
                }    
            }
        }
    }
    public void TakeDamage(float dmg)
    {
        if(!dead)
        {
            dead = true;
            StartCoroutine(Die());
            Collider2D box = GetComponent<Collider2D>();
            Vector3 goldSpawnPos = new Vector3(box.bounds.center.x,box.bounds.center.y,0);
            var a = Instantiate(goldDropped,goldSpawnPos,Quaternion.identity);
            a.GetComponent<CoinSpawner>().gold = gold;
            a.GetComponent<CoinSpawner>().ID = Random.Range(-100000, 100000);

            FindObjectOfType<HealthSystemPlayer>().GetComponent<Mana>().AddMana(manaPercentOnDie);
            Experience.instance.AddXP(exp);
            if(CheckIfCrit())
            {
                GetComponent<PrintDamage>().ShowText(dmg*2,true);
            }
            else
            {
                GetComponent<PrintDamage>().ShowText(dmg,false);
            }
        }
    }
    bool CheckIfCrit()
    {
        float rnd = Random.Range(1,100);
        if(rnd < StatsSystem.instance.currentLuck*100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator Die()
    {
        audioSource.loop = false;
        audioSource.Stop();
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(soundDie);
        audioSource.PlayOneShot(hitSound);
        GetComponent<AIPath>().canMove = false;
        GetComponent<EnemyAirParticles>().trailParticles.GetComponent<ParticleSystem>().Stop();
        var mainParticle = GetComponent<EnemyAirParticles>().mainParticles.GetComponent<ParticleSystem>().shape;
        while(mainParticle.radius < bigRadius)
        {
            mainParticle.radius = mainParticle.radius + velocity *Time.deltaTime;
            yield return null;
        }
        while(mainParticle.radius > 0.001f)
        {
            mainParticle.radius = mainParticle.radius - velocity *Time.deltaTime;
            yield return null;
        }
        Destroy(this.gameObject);
    }
}

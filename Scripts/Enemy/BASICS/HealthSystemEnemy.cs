using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystemEnemy : MonoBehaviour, IHaveHealth
{
    public GameObject goldDropped;
    public float manaPercentOnDie;

    public float MAX_HEALTH;

    public float currentHealth;

    [SerializeField] float timeHitted;
    [SerializeField] int expDrop;
    [SerializeField] int goldDrop;
    public bool hitted;
    public bool fatEnemy;
    float fade = 1f;
    [SerializeField] AudioClip hit;
    [SerializeField] AudioClip die;
    [SerializeField] ParticleSystem hitVFX;
    [SerializeField] ParticleSystem hitVFX2;
    [SerializeField] GameObject dieEffect;
    Material material;

    AudioSource audioSource;
    EnemyDetection detection;
    Animator myAnim;
    EnemyHealthBar healthBar;
    Rigidbody2D rb;

    public bool isAlive = true;

    private void Start()
    {
      
        hitVFX.Stop();
        hitVFX2.Stop();
        healthBar = GetComponent<EnemyHealthBar>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = MAX_HEALTH;
        hitted = false;
        isAlive = true;
        detection = GetComponent<EnemyDetection>();
        myAnim = GetComponent<Animator>();
        material = GetComponent<SpriteRenderer>().material;
        rb = GetComponent<Rigidbody2D>();
    }
    public void TakeDamage(float amountOfDamge)
    {
        if (isAlive)
        {
            if(GetComponent<ShieldEnemyController>() != null)
            {
                if(GetComponent<ShieldEnemyController>().canBeHitted)
                {
                    hitVFX.Play();
                    hitVFX2.Play();
                    if (!fatEnemy)
                    {
                        hitted = true;
                        if(GetComponent<ShieldEnemyController>()._currentState != (SMCInterface)GetComponent<DashState>())
                        {
                            myAnim.Play("Hitted");
                        }
                        StopAllCoroutines();
                        StartCoroutine(ResetHitted());
                    }
                    if (CheckIfCrit())
                    {
                        if(Character.currentType == CharacterType.Warrior)
                        {
                            currentHealth -= amountOfDamge * 2;
                            GetComponent<PrintDamage>().ShowText(amountOfDamge * 2, true);
                        }
                        else
                        {
                            currentHealth -= amountOfDamge * 4;
                            GetComponent<PrintDamage>().ShowText(amountOfDamge * 4, true);
                        }
                    }
                    else
                    {
                        currentHealth -= amountOfDamge;
                        GetComponent<PrintDamage>().ShowText(amountOfDamge, false);
                    }
                    if (currentHealth <= 0)
                    {
                        OnDie();
                        healthBar.DestroyHealthBar();
                        PlaySound(die);
                    }
                    else
                    {
                        healthBar.PopUp();
                        PlaySound(hit);
                    }
                }
                else
                {
                    GetComponent<ShieldEnemyController>().ShieldHit();
                }
                
            }
            else
            {
                hitVFX.Play();
                hitVFX2.Play();
                if (!fatEnemy)
                {
                    hitted = true;
                    myAnim.Play("Hitted");
                    StopAllCoroutines();
                    StartCoroutine(ResetHitted());
                }
                if (CheckIfCrit())
                {
                    if(Character.currentType == CharacterType.Warrior)
                    {
                        currentHealth -= amountOfDamge * 2;
                        GetComponent<PrintDamage>().ShowText(amountOfDamge * 2, true);
                    }
                    else
                    {
                        currentHealth -= amountOfDamge * 4;
                        GetComponent<PrintDamage>().ShowText(amountOfDamge * 4, true);
                    }
                }
                else
                {
                    currentHealth -= amountOfDamge;
                    GetComponent<PrintDamage>().ShowText(amountOfDamge, false);
                }
                if (currentHealth <= 0)
                {
                    OnDie();
                    healthBar.DestroyHealthBar();
                    PlaySound(die);
                }
                else
                {
                    healthBar.PopUp();
                    PlaySound(hit);
                }
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
    public void PlaySound(AudioClip sound)
    {
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }

    public void OnDie()
    {
        isAlive = false;
        
        Experience.instance.AddXP(expDrop);


        Collider2D box = GetComponent<Collider2D>();
        Vector3 goldSpawnPos = new Vector3(box.bounds.center.x,box.bounds.center.y,0);

        var a = Instantiate(goldDropped,goldSpawnPos,Quaternion.identity);
        a.GetComponent<CoinSpawner>().gold = goldDrop;
        a.GetComponent<CoinSpawner>().ID = Random.Range(-100000, 100000);



        FindObjectOfType<HealthSystemPlayer>().GetComponent<Mana>().AddMana(manaPercentOnDie);
        Instantiate(dieEffect, transform.position, Quaternion.identity);

        rb.velocity = Vector2.zero;
        hitted = true;
        StartCoroutine(DieEffect());
        if(GetComponent<WaveEnd>() != null)
        {
            GetComponent<WaveEnd>().EndWaveLevel();
        }
        Destroy(gameObject, 0.8f);
    }

    IEnumerator ResetHitted()
    {
        yield return new WaitForSeconds(timeHitted);
        hitted = false;
    }

    IEnumerator DieEffect()
    {   
       while(fade >= 0)
        {
            fade -= Time.deltaTime;

            if (fade <= 0f)
            {
                fade = 0f;

            }

            material.SetFloat("_Fade", fade);
            yield return null;
        }
    }
}

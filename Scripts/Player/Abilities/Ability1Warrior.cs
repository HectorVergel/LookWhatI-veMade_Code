using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability1Warrior : MonoBehaviour
{
    Image image;
    [SerializeField] float cooldown;
    float timer;
    public float ManaCost;
    [SerializeField] private float knockback;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] LayerMask WhatIsEnemy;
    public static Ability1Warrior instance;

    [SerializeField] AudioClip soundAbility;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] float timeToPlay;

    [SerializeField] ParticleSystem abilityVFX;

    public AudioSource audioSource;
    HealthSystemPlayer health;
    public float yOffset;

    

    public bool doAbility1;

    public float timeToDoAbility;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        timer = cooldown;
        abilityVFX.Stop();
        health = GetComponent<HealthSystemPlayer>();
    }


    private void OnEnable()
    {
        Ability1Manager.OnAbility1Warrior += DoAbility;
    }

    private void OnDisable()
    {
        Ability1Manager.OnAbility1Warrior -= DoAbility;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(boxCollider2d.bounds.center,boxCollider2d.bounds.center + new Vector3(range,0,0));
        Gizmos.DrawLine(boxCollider2d.bounds.center + new Vector3(0,boxCollider2d.bounds.extents.y),boxCollider2d.bounds.center + new Vector3(0,boxCollider2d.bounds.extents.y + yOffset/2));
    }

    private void Update() 
    {
       
        if(image != null)
        {
            CooldownTimer();
        }
        else if(GameObject.FindGameObjectWithTag("Ability1UI") != null)
        {
            image = GameObject.FindGameObjectWithTag("Ability1UI").GetComponent<Image>();
            image.fillAmount = timer/cooldown;
        }
    }
    void CooldownTimer()
    {
        if(timer < cooldown)
        {
            timer += Time.deltaTime;
        }
        if(Mana.instance.HaveManaTo(ManaCost) && PlayerJump.instance.grounded)
        {
            image.color = new Color (1,1,1,1);
        }
        else
        {
            image.color = new Color (1,1,1,0);
        }
        image.fillAmount = timer/cooldown;
    }

    public void CheckIfInRange()
    {
        PlaySound();
        timer = 0;
        StartCoroutine(StartAbility());
        StartCoroutine(PlayVFX());
        Tutorial();
    }

    private void DoAbility()
    {
        if(Mana.instance.HaveManaTo(ManaCost) && PlayerJump.instance.grounded)
        {
            doAbility1 = true;
        }
        else
        {
            audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(noManaSound);
        }
        
    }
    void Tutorial()
    {
        if(FindObjectOfType<TutorialSkill1>() !=null)
        {
            if(FindObjectOfType<TutorialSkill1>().triggered)
            {
                FindObjectOfType<TutorialSkill1>().Done();
            }
        }
    }
    
    public void Hitted()
    {
        StopAllCoroutines();
        doAbility1 = false;
        audioSource.Stop();


    }
    private void Cast(Collider2D enemy)
    {
        if (enemy.GetComponent<IHaveHealth>() != null)
        {
            enemy.GetComponent<IHaveHealth>().TakeDamage(damage);
            if(enemy.GetComponent<KnockBack>() != null)
            {
                BoxCollider2D entityCollider = enemy.GetComponent<BoxCollider2D>();
                enemy.GetComponent<KnockBack>().CallKnockBackLateral(entityCollider.bounds.center.x > boxCollider2d.bounds.center.x,knockback);
            }
        }
        if (enemy.GetComponent<MineBox>() != null)
        {
            enemy.GetComponent<MineBox>().DestroyObject();
        }
        if(enemy.GetComponent<IEnemyProjectile>() != null)
        {
            RejectProjectile(enemy);
        }
    }

    void RejectProjectile(Collider2D bullet)
    {
        Vector2 bulletVel = bullet.GetComponent<Rigidbody2D>().velocity;
        bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(-bulletVel.x * 4,0);

        if(bullet.GetComponent<turretBullet>() != null)
        {
            bullet.GetComponent<turretBullet>().defelected = true;
        }
        if(bullet.GetComponent<enemyBullet>() != null)
        {
            bullet.GetComponent<enemyBullet>().defelected = true;
        }
        if(bullet.GetComponent<Cage>() != null)
        {
            bullet.GetComponent<Cage>().defelected = true;
        }
    }

    IEnumerator StartAbility()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        RaycastHit2D[] colliders = Physics2D.BoxCastAll(boxCollider2d.bounds.center,boxCollider2d.bounds.size + new Vector3(0,yOffset,0),0,transform.right,range, WhatIsEnemy);
        foreach (var enemy in colliders)
        {
            Cast(enemy.collider);
        }
        
    }

    public void PlaySound()
    {
        StartCoroutine(StartSound());
    }

    IEnumerator StartSound()
    {
        yield return new WaitForSeconds(timeToPlay);
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(soundAbility);
        
    }

    IEnumerator PlayVFX()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        abilityVFX.Play();
    }


   

}

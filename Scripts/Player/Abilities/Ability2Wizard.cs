using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability2Wizard : MonoBehaviour
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
    public static Ability2Wizard instance;

    public bool doAbility;

    public float timeToDoAbility;

    [SerializeField] AudioClip soundAbility;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] float timeToPlay;

    [SerializeField] ParticleSystem abilityVFX;
    [SerializeField] ParticleSystem abilityVFX2;
    [SerializeField] ParticleSystem abilityVFX3;

    public AudioSource audioSource;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timer = cooldown;
        boxCollider2d = GetComponent<BoxCollider2D>();
        abilityVFX2.Stop();
    }

    private void OnEnable()
    {
        Ability2Manager.OnAbility2Wizard += DoAbility;
    }

    private void OnDisable()
    {
        Ability2Manager.OnAbility2Wizard -= DoAbility;
    }

    private void Update() 
    {
        if(image != null)
        {
            CooldownTimer();
        }
        else if(GameObject.FindGameObjectWithTag("Ability2UI") != null)
        {
            image = GameObject.FindGameObjectWithTag("Ability2UI").GetComponent<Image>();
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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range);

        Gizmos.DrawRay(boxCollider2d.bounds.center, transform.right* range);

        Gizmos.DrawRay(boxCollider2d.bounds.center, -transform.right * range);


    }

    public void CheckIfInRange()
    {
        PlaySound();
        timer = 0;
        StartCoroutine(Shake());
        Tutorial();
        StartCoroutine(StartAbility());
    }
    void Tutorial()
    {
        if(FindObjectOfType<TutorialSkill2>() !=null)
        {
            if(FindObjectOfType<TutorialSkill2>().triggered)
            {
                FindObjectOfType<TutorialSkill2>().Done();
            }
        }
        if(FindObjectOfType<TutorialBreakRock>() !=null)
        {
            if(FindObjectOfType<TutorialBreakRock>().triggered)
            {
                FindObjectOfType<TutorialBreakRock>().Done();
            }
        }
    }

    private void DoAbility()
    {
        if(Mana.instance.HaveManaTo(ManaCost) && timer >= cooldown && PlayerJump.instance.grounded)
        {
            doAbility = true;
        }
        else
        {
            audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(noManaSound);
        }
    }
    public void Hitted()
    {
        StopAllCoroutines();
        doAbility = false;
        audioSource.Stop();
    }
    private void Cast(Collider2D enemy)
    {
        if(enemy.GetComponentInParent<Spear>() != null)
        {
            enemy.GetComponentInParent<Spear>().OnDie();
        }
        if(enemy.GetComponent<IHaveHealth>() != null)
        {
            enemy.GetComponent<IHaveHealth>().TakeDamage(damage);
            if(enemy.GetComponent<KnockBack>() != null)
            {
                BoxCollider2D entityCollider = enemy.GetComponent<BoxCollider2D>();
                enemy.GetComponent<KnockBack>().CallKnockBackLateral(entityCollider.bounds.center.x > boxCollider2d.bounds.center.x, knockback);
            }
        }
        if(enemy.GetComponent<DestroyableObject>() != null)
        {
            enemy.GetComponent<DestroyableObject>().DestroyObject();
        }
        if (enemy.GetComponent<MineBox>() != null)
        {
            enemy.GetComponent<MineBox>().DestroyObject();
        }


    }

    IEnumerator StartAbility()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        Collider2D[] colliders = Physics2D.OverlapCircleAll((Vector2)boxCollider2d.bounds.center, range, WhatIsEnemy);
        foreach (var enemy in colliders)
        {
            Cast(enemy);
        }
        
    }
    IEnumerator Shake()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        global::Shake.instance.StartCoroutine(global::Shake.instance.shake());
        abilityVFX.Play();
        abilityVFX2.Play();
        

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
}

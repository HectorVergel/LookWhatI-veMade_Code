using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability2Warrior : MonoBehaviour
{
    Image image;
    [SerializeField] float cooldown;
    float timer;
    public float ManaCost;
    [SerializeField] private float knockback;
    [SerializeField] private float FOV;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] LayerMask WhatIsEnemy;
    public static Ability2Warrior instance;

    public float timeToDoAbility;

    public bool doAbility2;

    public GameObject abilityVFX;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    [SerializeField] AudioClip soundAbility;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] float timeToPlay;
    [SerializeField] float timeToPlayVFX;

    public AudioSource audioSource;
    HealthSystemPlayer health;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        boxCollider2d = GetComponent<BoxCollider2D>();
        timer = cooldown;
        FillLists();
        DisableParticles();
        health = GetComponent<HealthSystemPlayer>();
    }
    private void OnEnable()
    {
        Ability2Manager.OnAbility2Warrior += DoAbility;
    }

    private void OnDisable()
    {
        Ability2Manager.OnAbility2Warrior -= DoAbility;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range);


        Gizmos.color = Color.red;

        var direction = Quaternion.AngleAxis(FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction * range);

        var direction2 = Quaternion.AngleAxis(-FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction2 * range);


    }

    private void Update() 
    {
      
        if (image != null)
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

    public void CheckIfInRange()
    {
        PlaySound();
        Tutorial();
        StartCoroutine(Shake());
        StartCoroutine(StartParticles());
        timer = 0;
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

    public bool IsInAngle(Collider2D enemy, float FOV)
    {
        float angle = GetAngleToPlayer(enemy);

        return FOV >= 2 * angle;


    }

    private float GetAngleToPlayer(Collider2D enemy)
    {

        Vector2 v1 = transform.right;

        Vector2 v2 = enemy.bounds.center - boxCollider2d.bounds.center;

        return Vector2.Angle(v1, v2);
    }

    private void DoAbility()
    {
        if(Mana.instance.HaveManaTo(ManaCost) && PlayerJump.instance.grounded)
        {
            doAbility2 = true;
        }
        else
        {
            audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(noManaSound);
        }
    }

    IEnumerator StartAbility()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, range, WhatIsEnemy);
        
        foreach (var enemy in colliders)
        {
            if (IsInAngle(enemy, FOV))
            {
                Cast(enemy);
            }
        }
       
    }
    public void Hitted()
    {
        StopAllCoroutines();
        doAbility2 = false;
        audioSource.Stop();
    }
    private void Cast(Collider2D enemy)
    {
        if(enemy.GetComponentInParent<Spear>() != null)
        {
            enemy.GetComponentInParent<Spear>().OnDie();
        }
        if (enemy.GetComponent<IHaveHealth>() != null)
        {
            if(enemy.GetComponent<KnockBack>() != null)
            {
                enemy.GetComponent<IHaveHealth>().TakeDamage(damage);
                enemy.GetComponent<KnockBack>().CallKnockBackTop(knockback);
            }
        }
        if (enemy.GetComponent<DestroyableObject>() != null)
        {
            
            enemy.GetComponent<DestroyableObject>().DestroyObject();
        }
        if (enemy.GetComponent<MineBox>() != null)
        {
            enemy.GetComponent<MineBox>().DestroyObject();
        }

    }


    void FillLists()
    {
        for (int i = 0; i < abilityVFX.transform.childCount; i++)
        {
            var ps = abilityVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if (ps != null)
            {
                particles.Add(ps);
            }
        }


    }

    void EnableParticles()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();

        }
    }

    void DisableParticles()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

    IEnumerator Shake()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        global::Shake.instance.StartCoroutine(global::Shake.instance.shake());
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

    IEnumerator StartParticles()
    {
        yield return new WaitForSeconds(timeToPlayVFX);
        EnableParticles();
    }
}

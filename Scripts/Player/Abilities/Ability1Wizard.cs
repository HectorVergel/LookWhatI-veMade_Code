using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability1Wizard : MonoBehaviour
{
    Image image;
    [SerializeField] float cooldown;
    float timercooldown;
    public float manaCost;
    public static Ability1Wizard instance;

    public bool doAbility1;

    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float laserDistance;

    public Transform firePoint;

    public float timeToDoAbility;

    public GameObject startVFX;
    public GameObject finalVFX;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] LayerMask WhatIsEnemy;

    [SerializeField] float offset;

    float timer;
    [SerializeField] float timeParticles;

    [SerializeField] AudioClip soundAbility;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] float timeToPlay;

    public AudioSource audioSource;

    bool startCount;

    public int damage;


    private void OnEnable()
    {
        Ability1Manager.OnAbility1Wizard += DoAbility;
    }

    private void OnDisable()
    {
        Ability1Manager.OnAbility1Wizard -= DoAbility;
    }
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        timercooldown = cooldown;
        FillLists();
        DisableLaser();
    }

    // Update is called once per frame
    void Update()
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
        if (startCount)
        {
            TimeToDisable();
        }
       
    }
    void CooldownTimer()
    {
        if(timercooldown < cooldown)
        {
            timercooldown += Time.deltaTime;
        }
        if(Mana.instance.HaveManaTo(manaCost) && PlayerJump.instance.grounded)
        {
            image.color = new Color (1,1,1,1);
        }
        else
        {
            image.color = new Color (1,1,1,0);
        }
        image.fillAmount = timercooldown/cooldown;
    }

    private void DoAbility()
    {
        if (Mana.instance.HaveManaTo(manaCost) && PlayerJump.instance.grounded)
        {
            doAbility1 = true;
        }
        else
        {
            audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(noManaSound);
        }
    }

    public void Laser()
    {
        PlaySound();
        StartCoroutine(StartAbility());
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
    public void ActivateLaser()
    {
        Tutorial();
        timercooldown = 0;
        EnableLaser();
        Vector2 laserDirection;

        if (transform.localRotation.y == 1)
        {
             laserDirection = new Vector2(-laserDistance, firePoint.position.y);
        }
        else
        {
             laserDirection = new Vector2(laserDistance, firePoint.position.y);
        }
        
        lineRenderer.SetPosition(0, (Vector2)firePoint.position);
        startVFX.transform.position = firePoint.position;
        lineRenderer.SetPosition(1, new Vector3(laserDirection.x + firePoint.position.x,laserDirection.y, 0));

        RaycastHit2D hit = Physics2D.Linecast(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), WhatIsGround);

        RaycastHit2D[] enemyHit = Physics2D.LinecastAll(lineRenderer.GetPosition(0), lineRenderer.GetPosition(1), WhatIsEnemy);

        foreach (var enemy in enemyHit)
        {
            if (enemy.collider.GetComponent<IHaveHealth>() != null)
            {
                enemy.collider.GetComponent<IHaveHealth>().TakeDamage(damage);
            }
            if (enemy.collider.GetComponent<MineBox>() != null)
            {
                enemy.collider.GetComponent<MineBox>().DestroyObject();
            }
        }

        if (hit)
        {
            if(transform.localRotation.y == 1)
            {
                lineRenderer.SetPosition(1, hit.point + new Vector2(offset,0) );
            }
            else
            {
                lineRenderer.SetPosition(1, hit.point - new Vector2(offset, 0));
            }
            
        }

        finalVFX.transform.position = lineRenderer.GetPosition(1);
    }

    public void Hitted()
    {
        StopAllCoroutines();
        doAbility1 = false;
        timer = 0;
        startCount = false;
        audioSource.Stop();
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
        lineRenderer.enabled = false;
    }

    private void EnableLaser()
    {
        lineRenderer.enabled = true;
        startCount = true;
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    public void DisableLaser()
    {
        

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
       
        lineRenderer.enabled = false;
    }


    void FillLists()
    {
        for (int i = 0; i < startVFX.transform.childCount; i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if (ps != null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < finalVFX.transform.childCount; i++)
        {
            var ps = finalVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }

    IEnumerator StartAbility()
    {
        yield return new WaitForSeconds(timeToDoAbility);
        ActivateLaser();
    }

    void TimeToDisable()
    {
        if(timer < timeParticles)
        {
            timer += Time.deltaTime;
        }
        else
        {
            startCount = false;
            DisableLaser();
            timer = 0;
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

}

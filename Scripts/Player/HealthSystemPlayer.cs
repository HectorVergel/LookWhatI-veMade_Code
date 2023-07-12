using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
 using UnityEngine.SceneManagement;

public class HealthSystemPlayer : MonoBehaviour
{
    Image image;
    Image follow;
    bool following = false;

    [SerializeField]
    public float currentHealth;



    Animator anim;

    public bool invulnerable = false;

    public static HealthSystemPlayer instance;

    public bool hitted;
    public bool isAlive;
    private float timer;
    [SerializeField] private float invulnerableCooldown;
    private BoxCollider2D playerCollider;
    public Rigidbody2D rb;

    [SerializeField] AudioClip hitSound;

    [SerializeField] float timeToPlay;
    [SerializeField] float timeStopEffect;
    [SerializeField] float knockForce;


  
    [SerializeField] ParticleSystem vfx2;
    public AudioSource effects;
    public float followVelocity;


    private void Awake()
    {
        instance = this;

    }

    void Start()
    {
        isAlive = true;
        image = GameObject.FindWithTag("HealthBar").GetComponent<Image>();
        follow = GameObject.FindWithTag("FollowHealth").GetComponent<Image>();
        currentHealth = PlayerPrefs.GetInt("CurrentHealth",(int)StatsSystem.instance.currentHP);
        StartCoroutine(SetHp());
        anim = GetComponent<Animator>();
        playerCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        image.fillAmount = currentHealth/StatsSystem.instance.currentHP;
        follow.fillAmount = image.fillAmount;
 
        vfx2.Stop();
    }

    IEnumerator SetHp()
    {
        yield return new WaitForFixedUpdate();
        currentHealth = PlayerPrefs.GetInt("CurrentHealth",(int)StatsSystem.instance.currentHP);
        image.fillAmount = currentHealth/StatsSystem.instance.currentHP;
    }

    void Update()
    {
        TimerToVulnerable();
        if(PlayerJump.instance.grounded)
        {
            hitted = false;
        }
    }

    public void SetMaximumHP()
    {
        currentHealth = StatsSystem.instance.currentHP;
        PlayerPrefs.SetInt("CurrentHealth",(int)currentHealth);
        image.fillAmount = currentHealth/StatsSystem.instance.currentHP;
    }
    public void OnDie()
    {
        isAlive = false;
        LoadLevel.instance.DeleteEnemies();
        LoadLevel.instance.Respawn();
        PlayerPrefs.SetInt("Died",1);
    }
    

    public void AddHPWithStat(float hp)
    {
        currentHealth+=hp;
        PlayerPrefs.SetInt("CurrentHealth",(int)currentHealth);
        SetMaxHP();

    }
    public void SetMaxHP()
    {
        if(currentHealth > StatsSystem.instance.currentHP)
        {
            currentHealth = StatsSystem.instance.currentHP;
            PlayerPrefs.SetInt("CurrentHealth",(int)currentHealth);
        }
        image.fillAmount = currentHealth/StatsSystem.instance.currentHP;
    }

    public void TakeDamage(int amountOfDamge)
    {
        if (!invulnerable && PlayerPrefs.GetInt("Zen",0) == 0)
        {
            hitted = true;
            FindObjectOfType<HitFadeEffect>().StartFadeIn();
            vfx2.Play();
            anim.Play("hit");
            CancelAttack();
            follow.fillAmount = image.fillAmount;
            currentHealth -= amountOfDamge;
            PlayerPrefs.SetInt("CurrentHealth", (int)currentHealth);
            effects.PlayOneShot(hitSound);
            image.fillAmount = currentHealth / StatsSystem.instance.currentHP;
            if(!following)
            {
                StartCoroutine(FollowHealth());
            }
            MakeInvulnerable();
            if (currentHealth <= 0)
            {
                OnDie();
            }
        }
        
    }

    void CancelAttack()
    {
        if(Character.currentType == CharacterType.Warrior)
        {
            Ability1Warrior.instance.Hitted();
            Ability2Warrior.instance.Hitted();
            MeleCombat.instance.Hitted();
        }
        else if(Character.currentType == CharacterType.Wizard)
        {
            Ability1Wizard.instance.Hitted();
            Ability2Wizard.instance.Hitted();
            WizardCombat.instance.Hitted();
        }
    }

    IEnumerator FollowHealth()
    {
        following = true;
        while(follow.fillAmount > image.fillAmount)
        {
            follow.fillAmount -= followVelocity * Time.deltaTime;
            yield return null;
        }
        following = false;
    }


    void KnockBack()
    {
        Vector2 direction;
        if(transform.localRotation.y == 0 && hitted)
        {
            direction = new Vector2(-1, 1);
            rb.AddForce(direction * knockForce, ForceMode2D.Impulse);
        }
        else if(transform.localRotation.y == 1 && hitted)
        {
            direction = new Vector2(1, 1);
            rb.AddForce(direction * knockForce, ForceMode2D.Impulse);
        }
    }
    

    
    

    void TimerToVulnerable()
    {
        if (invulnerable)
        {
            timer += Time.deltaTime;

            if (timer >= invulnerableCooldown)
            {
                MakeVulnerable();
                timer = 0;
            }
        }
    }


   

    void MakeInvulnerable()
    {
        invulnerable = true;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
        gameObject.layer = 7;
    }

    void MakeVulnerable()
    {
        invulnerable = false;
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        gameObject.layer = 9;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        if (collision.gameObject.tag == "Ground")
        {
            hitted = false;
           
        }
    }



}

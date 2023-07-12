using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    public bool airAttackTutorial = false;
    public bool attackTutorial = false;
    public bool fullComboTutorial = false;
    [SerializeField] float forceOfImpulse;
    public float minimumHeightToAirAttack;
    public static PlayerCombat instance;
    [SerializeField] AudioClip sound1;
    [SerializeField] AudioClip sound2;
    [SerializeField] AudioClip sound3;
    [SerializeField] AudioClip sound4;
    [SerializeField] AudioClip sound5;
    [SerializeField] AudioClip soundWizardAttack;
    [SerializeField] AudioClip soundAirMelee1;
    [SerializeField] AudioClip soundAirMelee2;
    Animator myAnim;
    public AudioSource audioAttacks;
    public bool isAttacking = false;
    public bool combing = false;

    public float timeToPlayMeleeAtt;
    public float timeToPlayWizardAtt;

    Rigidbody2D rb;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] LayerMask WhatIsEnemy;

    [SerializeField] GameObject magicBall1;

    [SerializeField] GameObject magicAirBall1;

    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointAir;

    public Collider2D boxCollider2d;
    
    [SerializeField]
    private float FOV_long;

    [SerializeField]
    private float FOV_short;


    [SerializeField]
    private float range_long;

     [SerializeField]
    private float range_short;

    private float currentDamage;

    public float timeToDoAttackMelee;

    public float timeToDoAttackWizard;

    public float timeToDoAttackWizardAir;

    [SerializeField] ParticleSystem attackMeleeVFX;

    [SerializeField] private float multiplier1;
    [SerializeField] private float multiplier2;
    [SerializeField] private float multiplier3;
    [SerializeField] private float multiplier4;
    [SerializeField] private float airMultiplier1;
    [SerializeField] private float airMultiplier2;
    // Start is called before the first frame update
    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        currentDamage = StatsSystem.instance.currentStrength;
        fullComboTutorial = false;
        airAttackTutorial = false;
        attackTutorial = false;
        attackMeleeVFX.Stop();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_long);

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_short);
        Gizmos.color = Color.red;

        var direction = Quaternion.AngleAxis(FOV_long / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction * range_long);

        var direction2 = Quaternion.AngleAxis(-FOV_long / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction2 * range_long);

        var direction3 = Quaternion.AngleAxis(FOV_short / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction3 * range_long);

        var direction4 = Quaternion.AngleAxis(-FOV_short / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction4 * range_long);
    }

    private void OnEnable()
    {
        PlayerInputs.OnAttack += DoAttack;
    }

    private void OnDisable()
    {
        PlayerInputs.OnAttack -= DoAttack;
        
    }


    public void DoAttack()
    {
        isAttacking = true;
    }

    public void SetComboAttack(int numAttack)
    {

        if(Character.currentType == CharacterType.Warrior)
        {
            MeleeAttack(numAttack);
        }
        else
        {
            WizardAttack(numAttack);
        }
        
    }

    public void SetComboAirAttack(int numAttack)
    {

        if(Character.currentType == CharacterType.Warrior)
        {
            MeleeAirAttack(numAttack);
        }
        else
        {
            WizardAirAttack(numAttack);
        }
        
    }

    public void PlayAirAttackSound(int num)
    {
        switch (num)
        {
            case 1:
                audioAttacks.PlayOneShot(soundAirMelee1);
                break;
            case 2:
                audioAttacks.PlayOneShot(soundAirMelee2);
                break;
        }
    }
    public void PlaySounds(int num)
    {
        switch (num)
        {
            case 1:
            audioAttacks.PlayOneShot(sound1);
            break;
            case 2:
            audioAttacks.PlayOneShot(sound2);
            break;
            case 3:
            audioAttacks.PlayOneShot(sound4);
            break;
            case 4:
            audioAttacks.clip = sound5;
            audioAttacks.PlayDelayed(timeToPlayMeleeAtt);
            break;
            default:
            break;
        }
    }

    public bool HaveHeightToAirAttack()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center,boxCollider2d.bounds.size,0f,Vector2.down,minimumHeightToAirAttack, WhatIsGround);
        return raycastHit2d.collider == null;
    }

    public bool HaveHeightToAttackWhenFall()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center,boxCollider2d.bounds.size,0f,Vector2.down,NextAction.instance.minimumHeight, WhatIsGround);
        return raycastHit2d.collider != null;
    }

     public void IsInRange(float range, float FOV, int attackInitAngle)
    {
      
         Collider2D [] colliders =  Physics2D.OverlapCircleAll(boxCollider2d.bounds.center,range, WhatIsEnemy);
        
        foreach(var enemy in colliders)
        {
            if(IsInAngle(enemy, FOV, attackInitAngle))
            {
                enemy.GetComponent<HealthSystemEnemy>().TakeDamage(currentDamage);
            }
        }
    }

    public bool IsInAngle(Collider2D enemy,float FOV, int attackInitAngle)
    {
        float angle = GetAngleToPlayer(enemy,attackInitAngle);
        
       return FOV >= 2 * angle;
        
        
    }

     private float GetAngleToPlayer(Collider2D enemy, int attackInitAngle)
    {
        
        Vector2 v1 = transform.right;
        
        Vector2 v2 = enemy.bounds.center - boxCollider2d.bounds.center;
          
        return Vector2.Angle(v1,v2);
    }

   

    void MeleeAttack(int numAttack)
    {
        currentDamage = StatsSystem.instance.currentStrength;
        switch(numAttack)
        {
            case 1:
                IsInRange(range_short,FOV_short,1);
                attackTutorial = true;
                break;

            case 2:
                currentDamage *= multiplier1;
                currentDamage = Mathf.Round(currentDamage);
                IsInRange(range_short, FOV_short, 1);
                
                break;

            case 3:
                currentDamage *= multiplier2;
                currentDamage = Mathf.Round(currentDamage);
                IsInRange(range_short, FOV_short, 1);
                
                break;

            case 4:
                currentDamage *= multiplier3;
                currentDamage = Mathf.Round(currentDamage);
                StartCoroutine(DoLastAttack());
                
                break;
            default:
            break;
        }
    }

    void MeleeAirAttack(int numAttack)
    {
        airAttackTutorial = true;
        currentDamage = StatsSystem.instance.currentStrength;
        switch(numAttack)
        {
            case 1:
                currentDamage *= airMultiplier1;
                currentDamage = Mathf.Round(currentDamage);
                IsInRange(range_long, FOV_long,2);
                break;

            case 2:
                currentDamage *= airMultiplier2;
                currentDamage = Mathf.Round(currentDamage);
                IsInRange(range_long, FOV_long,2);
                break;

            default:
            break;
        }
    }


    void WizardAirAttack(int numAttack)
    {
        airAttackTutorial = true;
        StartCoroutine(DoWizardAttackAir());
    }

    void WizardAttack(int numAttack)
    {
        StartCoroutine(DoWizardAttack());
        attackTutorial = true;

    }

    void CastMagicBall(int typeOfBall)
    {
        Instantiate(magicBall1,firePoint.position,Quaternion.identity);
        audioAttacks.PlayDelayed(timeToPlayWizardAtt);
    }

    void CastMagicAirBall(int typeOfBall)
    {
        Instantiate(magicAirBall1,firePointAir.position,Quaternion.identity);
        audioAttacks.PlayDelayed(timeToPlayWizardAtt);
    }
    
    IEnumerator DoLastAttack()
    {
        yield return new WaitForSeconds(timeToDoAttackMelee);
        IsInRange(range_long, FOV_long, 1);
        attackMeleeVFX.Play();
    }

    IEnumerator DoWizardAttack()
    {
        yield return new WaitForSeconds(timeToDoAttackWizard);
        CastMagicBall(1);
    }

    IEnumerator DoWizardAttackAir()
    {
        yield return new WaitForSeconds(timeToDoAttackWizardAir);
        CastMagicAirBall(1);
    }
}

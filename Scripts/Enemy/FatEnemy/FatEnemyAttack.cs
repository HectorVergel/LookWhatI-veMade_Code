using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatEnemyAttack : MonoBehaviour
{
    public float heavyAttackTime;
    public float comboAttackTime1;
    public float comboAttackTime2;
    public float comboAttackTime3;
    public float attackHeavyStoppedTime;
    public float attackComboStoppedTime;
    public bool attacking;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] int damageHeavyAttack;
    [SerializeField] int damageComboAttack;
    [SerializeField] float FOV_Combo1;
    [SerializeField] float FOV_Combo2;
    [SerializeField] LayerMask WhatIsPlayer;
    [SerializeField] private float range_heavyAttack;
    [SerializeField] private float rangeStartAttack;
    [SerializeField] private float range_comboAttack1;
    [SerializeField] private float range_comboAttack2;
    [SerializeField] private float heavyDamageTime;
    [SerializeField] private float cooldown;
    float timeToAttack;
    Rigidbody2D rb;
    HealthSystemEnemy healthEnemy;
    EnemyPatrollingBasic patrol;
    EnemyDetection detection;
    Animator myAnimator;
    AudioSource audioSource;
    public AudioClip soundAttackHeavy;
    public AudioClip soundAttackCombo;


    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthEnemy = GetComponent<HealthSystemEnemy>();
        detection = GetComponent<EnemyDetection>();
        patrol = GetComponent<EnemyPatrollingBasic>();
        attacking = false;
        myAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();  
    }

    private void Update()
    {
        if (!healthEnemy.isAlive)
        {
            StopAllCoroutines();
        }
        
        if(timeToAttack < cooldown)
        {
            timeToAttack += Time.deltaTime;
        }
        IsInRangeToAttack();

    }

    private void OnDrawGizmos()
    {
        //HEAVY ATTACK GIZMOS
        Gizmos.color = Color.black;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, rangeStartAttack);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_heavyAttack);


        //COMBO ATTACK GIZMOS

        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_comboAttack1);

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_comboAttack1);

        var direction3 = Quaternion.AngleAxis(FOV_Combo1 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction3 * range_comboAttack1);

        var direction4 = Quaternion.AngleAxis(-FOV_Combo1 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction4 * range_comboAttack1);


        //2


        Gizmos.color = Color.blue;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_comboAttack2);


        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_comboAttack2);

        var direction5 = Quaternion.AngleAxis(FOV_Combo2 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction5 * range_comboAttack2);

        var direction6 = Quaternion.AngleAxis(-FOV_Combo2 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction6 * range_comboAttack2);
    }

    public void ChooseAttack()
    {
        switch (Random.Range(0,2))
        {
            case 0:
                StartCoroutine(AttackHeavy());
                break;
            case 1:
                StartCoroutine(AttackCombo1());
                break;
            default:
                break;

        }
    }

    
    public bool CheckIfCanAttack()
    {
        if (rb.velocity.y != 0 || patrol.attacking || !detection.visionArea || timeToAttack<cooldown || !healthEnemy.isAlive)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void DoAttack()
    {
        patrol.attacking = true;
        ChooseAttack();
        
    }

    IEnumerator AttackHeavy()
    {
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(soundAttackHeavy);
        float time = 0;
        myAnimator.SetBool("Attacking", true);
        myAnimator.Play("Attack1Fat");
        StartCoroutine(AttackState(attackHeavyStoppedTime));
        yield return new WaitForSeconds(heavyAttackTime);
        float time2 = 0;
        while (time <= heavyDamageTime)
        {
            time += Time.deltaTime;
            if (time2 <= heavyAttackTime)
            {
                time2 += Time.deltaTime;
            }
            else
            {
                CheckIfPlayerInsideHeavyAttack();
                time2 = 0;
            }
            
            yield return null;
        }
        
    }

    IEnumerator AttackCombo1()
    {
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(soundAttackCombo);
        myAnimator.SetBool("Attacking", true);
        myAnimator.Play("Attack2Fat");
        StartCoroutine(AttackState(attackComboStoppedTime));
        yield return new WaitForSeconds(comboAttackTime1);
        CheckIfPlayerInsideComboAttack(FOV_Combo1,range_comboAttack1);
        StartCoroutine(AttackCombo2());
    }

    IEnumerator AttackCombo2()
    {
        yield return new WaitForSeconds(comboAttackTime2);
        CheckIfPlayerInsideComboAttack(FOV_Combo1, range_comboAttack1);
        StartCoroutine(AttackCombo3());
    }

    IEnumerator AttackCombo3()
    {
        yield return new WaitForSeconds(comboAttackTime3);
        CheckIfPlayerInsideComboAttack(FOV_Combo2, range_comboAttack2);
    }

    IEnumerator AttackState(float time)
    {
        yield return new WaitForSeconds(time);
        patrol.attacking = false;
        myAnimator.SetBool("Attacking", false);
        timeToAttack = 0;
    }

   

    public void IsInRangeToAttack()
    {
        Collider2D[] collide = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, rangeStartAttack, WhatIsPlayer);
        foreach (var player in collide)
        {
            if (player != null)
            {
                if (CheckIfCanAttack())
                {
                    DoAttack();
                }

            }
        }
    }

   

    void CheckIfPlayerInsideHeavyAttack()
    {
        Collider2D[] collide = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, range_heavyAttack, WhatIsPlayer);
        foreach (var player in collide)
        {
            if (player != null)
            {
                HealthSystemPlayer.instance.TakeDamage(damageHeavyAttack);
            }
        }
       
    }

    void CheckIfPlayerInsideComboAttack(float FOV, float range)
    {
        Collider2D[] collide = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, range, WhatIsPlayer);
        foreach (var player in collide)
        {
            if (player != null)
            {
                if (IsInAngle(player, FOV))
                {
                    HealthSystemPlayer.instance.TakeDamage(damageComboAttack);
                }
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackSmall : MonoBehaviour
{

    public float attackDelay;
    public float attackStoppedTime;
    public bool attacking;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] int damage;
    [SerializeField] float FOV;
    [SerializeField] LayerMask WhatIsPlayer;
    [SerializeField] private float range_attack;
    [SerializeField] private float rangeStartAttack;
    Rigidbody2D rb;
    HealthSystemEnemy healthEnemy;
    EnemyDetection detection;
    EnemyPatrollingBasic patrol;
    Animator myAnim;
    [SerializeField] private float cooldown;
    float timeToAttack;
    AudioSource audioSource;
    public AudioClip attackSound;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        detection = GetComponent<EnemyDetection>();
        healthEnemy = GetComponent<HealthSystemEnemy>();
        patrol = GetComponent<EnemyPatrollingBasic>();
        attacking = false;
        myAnim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if (healthEnemy.hitted)
        {
            timeToAttack = 0;
        }

        if (timeToAttack < cooldown && !healthEnemy.hitted)
        {
            timeToAttack += Time.deltaTime;
        }
        if (detection.visionArea)
        {
            IsInRangeToAttack();
        }
        if(healthEnemy.hitted == true)
        {
            StopAttack();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, rangeStartAttack);

        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_attack);

        var direction = Quaternion.AngleAxis(FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction * range_attack);

        var direction2 = Quaternion.AngleAxis(-FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction2 * range_attack);
    }
    public bool CheckIfCanAttack()
    {
        if(rb.velocity.y != 0 || healthEnemy.hitted || patrol.attacking || timeToAttack < cooldown)
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
        myAnim.Play("AttackSmall");
        StartCoroutine(AttackState());
        StartCoroutine(Attack());
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(attackSound);
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        CheckIfPlayerInsideAttack();
    }

    IEnumerator AttackState()
    {
        yield return new WaitForSeconds(attackStoppedTime);
        //attacking = false;
        patrol.attacking = false;
    }

    public void StopAttack()
    {
        StopAllCoroutines();
        //attacking = false;
        patrol.attacking = false;
    }

    public void IsInRangeToAttack()
    {
        Collider2D[] collide = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, rangeStartAttack, WhatIsPlayer);
        foreach (var player in collide)
        {
            if (player != null)
            {
                if (CheckIfCanAttack() && IsInAngle(player, FOV))
                {
                    
                    DoAttack();
                }

            }
        }
    }

    void CheckIfPlayerInsideAttack()
    {
        Collider2D[] collide = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, range_attack, WhatIsPlayer);
        foreach (var player in collide)
        {
            if (player != null)
            {
                if (IsInAngle(player, FOV))
                {
                   
                    HealthSystemPlayer.instance.TakeDamage(damage);
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

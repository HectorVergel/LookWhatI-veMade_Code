using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackGroundRanged : MonoBehaviour
{
    public float attackDelay;
    public float attackStoppedTime;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] int damage;
    [SerializeField] float FOV;
    [SerializeField] float timeAnimation;
    [SerializeField] LayerMask WhatIsPlayer;
    [SerializeField] private float range_attack;
    [SerializeField] private float rangeStartAttack;
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform firePoint;
    Rigidbody2D rb;
    [SerializeField] private float cooldown;
    float timeToAttack;
    HealthSystemEnemy healthEnemy;
    RangedPatrolling patrol;

    EnemyDetectionVariant detection;
    Animator animator;
    AudioSource audioSource;
    public AudioClip sound;
    void Start()
    {
        patrol = GetComponent<RangedPatrolling>();
        rb = GetComponent<Rigidbody2D>();
        healthEnemy = GetComponent<HealthSystemEnemy>();
        detection = GetComponent<EnemyDetectionVariant>();
        animator = GetComponent<Animator>();
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
        IsInRangeToAttack();
        if (healthEnemy.hitted == true)
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
    
    
    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(timeAnimation);
        animator.Play("AttackRanged");
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }
    public bool CheckIfCanAttack()
    {
        if (rb.velocity.y != 0 || healthEnemy.hitted || patrol.attacking || !detection.visionArea || timeToAttack < cooldown)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    IEnumerator Attack()
    {
        yield return new WaitForSeconds(attackDelay);
        var a = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        a.GetComponent<enemyBullet>().damage = damage;
    }

    IEnumerator AttackState()
    {
        float time = 0;
        while(time<attackStoppedTime)
        {
            time+=Time.deltaTime;
            patrol.FlipToPlayer();
            yield return null;
        }
        patrol.attacking = false;
    }
    public void DoAttack()
    {
        patrol.attacking = true;
        StartCoroutine(StartAnimation());
        StartCoroutine(AttackState());
        StartCoroutine(Attack());
    }
    public void StopAttack()
    {
        StopAllCoroutines();
        patrol.attacking = false;
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

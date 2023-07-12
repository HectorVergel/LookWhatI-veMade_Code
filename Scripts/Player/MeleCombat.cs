using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleCombat : MonoBehaviour
{
    public bool airAttackTutorial;
    public bool attackTutorial;
    public bool fullComboTutorial;
    [SerializeField] float forceOfImpulse;
    public float minimumHeightToAirAttack;
    public static MeleCombat instance;
    [SerializeField] AudioClip soundAttack12;
    [SerializeField] AudioClip soundAttack3;
    [SerializeField] AudioClip soundAttack4;
    Animator myAnim;
    public AudioSource audioAttacks;
    public bool isAttacking;
    public bool combing;

    public float timeToPlaySoundAttack3;
    public float timeToPlaySoundAttack4;

    Rigidbody2D rb;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] LayerMask WhatIsEnemy;
    public Collider2D boxCollider2d;
    
    [SerializeField]
    private float FOV_1;

    [SerializeField]
    private float FOV_2;

    [SerializeField]
    private float FOV_3;

    [SerializeField]
    private float FOV_4;

    [SerializeField]
    private float range_1;

    [SerializeField]
    private float range_2;

    [SerializeField]
    private float range_3;

    [SerializeField]
    private float range_4;

    private float currentDamage;

    public float timeAttack3;
    public float timeAttack4;
    [SerializeField] ParticleSystem attackMeleeVFX;

    [SerializeField] private float multiplier1;
    [SerializeField] private float multiplier2;
    [SerializeField] private float multiplier3;
    public bool airCombo = true;

    HealthSystemPlayer health;
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
        isAttacking = false;
        combing = false;
        attackMeleeVFX.Stop();
        health = GetComponent<HealthSystemPlayer>();    
    }

    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        
        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_1);
        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_2);
        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_3);
        Gizmos.DrawWireSphere(boxCollider2d.bounds.center,range_4);
        Gizmos.color = Color.red;

        var direction = Quaternion.AngleAxis(FOV_1 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction * range_1);

        var direction2 = Quaternion.AngleAxis(-FOV_1 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction2 * range_1);


        var direction3 = Quaternion.AngleAxis(FOV_2 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction3 * range_2);

        var direction4 = Quaternion.AngleAxis(-FOV_2 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction4 * range_2);


        var direction5 = Quaternion.AngleAxis(FOV_3 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction5 * range_3);

        var direction6 = Quaternion.AngleAxis(-FOV_3 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction6 * range_3);


        var direction7 = Quaternion.AngleAxis(FOV_4 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction7 * range_4);

        var direction8 = Quaternion.AngleAxis(-FOV_4 / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(boxCollider2d.bounds.center, direction8 * range_4);
    }

    private void OnEnable()
    {
        PlayerInputs.OnAttack += DoAttack;
    }

    private void OnDisable()
    {
        PlayerInputs.OnAttack -= DoAttack;
        
    }

    private void Update() {

        if (PlayerJump.instance.grounded)
        {
            airCombo = true;
        }

        
    }
    public void DoAttack()
    {
        isAttacking = true;
        
    }
    void Tutorial()
    {
        if(FindObjectOfType<TutorialAttack>() !=null)
        {
            if(FindObjectOfType<TutorialAttack>().triggered)
            {
                FindObjectOfType<TutorialAttack>().Done();
            }
        }
    }

    public void Attack1()
    {
        currentDamage = StatsSystem.instance.currentStrength;
        IsInRange(range_1,FOV_1);
        attackTutorial = true;
        PlaySound(soundAttack12);
        Tutorial();
        
    }
    public void Attack2()
    {
        currentDamage = StatsSystem.instance.currentStrength;
        currentDamage *= multiplier1;
        currentDamage = Mathf.Round(currentDamage);
        IsInRange(range_1, FOV_1);
        PlaySound(soundAttack12);
       
    }
    public void Attack3()
    {
        StartCoroutine(WaitToAttack3());
        StartCoroutine(PlaySoundAttack3());
    }
    IEnumerator WaitToAttack3()
    {
        yield return new WaitForSeconds(timeAttack3);
        currentDamage = StatsSystem.instance.currentStrength;
        currentDamage *= multiplier2;
        currentDamage = Mathf.Round(currentDamage);
        IsInRange(range_2, FOV_2);

    }
    IEnumerator PlaySoundAttack3()
    {
        yield return new WaitForSeconds(timeToPlaySoundAttack3);
        PlaySound(soundAttack3);

    }
    public void Attack4()
    {
        StartCoroutine(WaitToAttack4());
        StartCoroutine(PlaySoundAttack4());
    }
    IEnumerator WaitToAttack4()
    {
        yield return new WaitForSeconds(timeAttack4);
        currentDamage = StatsSystem.instance.currentStrength;
        currentDamage *= multiplier3;
        currentDamage = Mathf.Round(currentDamage);
        IsInRange(range_3, FOV_3);
        attackMeleeVFX.Play();

    }
    IEnumerator PlaySoundAttack4()
    {
        yield return new WaitForSeconds(timeToPlaySoundAttack4);
        PlaySound(soundAttack4);

    }
    public void AttackAir1()
    {
        airAttackTutorial = true;
        currentDamage = StatsSystem.instance.currentStrength;
        currentDamage = Mathf.Round(currentDamage);
        IsInRangeSimple(range_4, FOV_4);
        PlaySound(soundAttack12);
        Tutorial();
    }
    public void AttackAir2()
    {
        currentDamage = StatsSystem.instance.currentStrength;
        currentDamage = Mathf.Round(currentDamage);
        IsInRangeSimple(range_4, FOV_4);
        PlaySound(soundAttack12);
    }
    void PlaySound(AudioClip clip)
    {
        audioAttacks.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioAttacks.PlayOneShot(clip);
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

     public void IsInRange(float range, float FOV)
    {
      
         Collider2D [] colliders =  Physics2D.OverlapCircleAll(boxCollider2d.bounds.center,range, WhatIsEnemy);
        
        foreach(var enemy in colliders)
        {
            
            if (IsInAngleSimple(enemy, FOV))
            {
                
                if(enemy.GetComponent<IHaveHealth>() != null)
                {
                    enemy.GetComponent<IHaveHealth>().TakeDamage(currentDamage);
                }
                if (enemy.GetComponent<MineBox>() != null)
                {
                    enemy.GetComponent<MineBox>().DestroyObject();
                }
                
            }
        }
    }

    public void Hitted()
    {
        StopAllCoroutines();
        isAttacking = false;
        combing = false;


    }
    public void IsInRangeSimple(float range, float FOV)
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(boxCollider2d.bounds.center, range, WhatIsEnemy);

        foreach (var enemy in colliders)
        {

            if (IsInAngleSimple(enemy, FOV))
            {

                if (enemy.GetComponent<IHaveHealth>() != null)
                {
                    enemy.GetComponent<IHaveHealth>().TakeDamage(currentDamage);
                }
                if (enemy.GetComponent<MineBox>() != null)
                {
                    enemy.GetComponent<MineBox>().DestroyObject();
                }
            }
        }
    }

    public bool IsInAngle(Collider2D enemy,float FOV, float range)
    {
        float angle = GetAngleToPlayer(enemy,range);
        
       return FOV >= 2 * angle;
        
        
    }

    private float GetAngleToPlayer(Collider2D enemy, float range)
    {
        
        Vector2 v1 = transform.right;
        Vector2 v2 = -transform.right;
        RaycastHit2D[] ray = Physics2D.RaycastAll(boxCollider2d.bounds.center, transform.right, range, WhatIsEnemy);
        foreach (var item in ray)
        {
            if(item.collider == enemy)
            {
                v2 = item.point - (Vector2)boxCollider2d.bounds.center;
            }
        }
       
          
        return Vector2.Angle(v1,v2);
    }



    public bool IsInAngleSimple(Collider2D enemy, float FOV)
    {
        float[] angles = GetAngleToPlayerSimple(enemy);

        foreach (var angle in angles)
        {
           if (FOV >= 2 * angle)
           {
                return true;
           }
            
            
        }
        return false;

    }

    private float[] GetAngleToPlayerSimple(Collider2D enemy)
    {

        Vector2 v1 = transform.right;
        float[] angles = new float[5];

        Vector2[] enemyVertex = new Vector2[4];
        enemyVertex[0] = (Vector2)enemy.bounds.center + new Vector2(enemy.bounds.size.x, -enemy.bounds.size.y) * 0.5f;
        enemyVertex[1] = (Vector2)enemy.bounds.center + new Vector2(-enemy.bounds.size.x, -enemy.bounds.size.y) * 0.5f;
        enemyVertex[2] = (Vector2)enemy.bounds.center + new Vector2(enemy.bounds.size.x, enemy.bounds.size.y) * 0.5f;
        enemyVertex[3] = (Vector2)enemy.bounds.center + new Vector2(-enemy.bounds.size.x, enemy.bounds.size.y) * 0.5f;
        
        

        Vector2 v2 = enemyVertex[0] - (Vector2)boxCollider2d.bounds.center;

        Vector2 v3 = enemyVertex[1] - (Vector2)boxCollider2d.bounds.center;

        Vector2 v4 = enemyVertex[2] - (Vector2)boxCollider2d.bounds.center;

        Vector2 v5 = enemyVertex[3] - (Vector2)boxCollider2d.bounds.center;

        Vector2 v6 = Physics2D.ClosestPoint(boxCollider2d.bounds.center, enemy) - (Vector2)boxCollider2d.bounds.center;

        angles[0] = Vector2.Angle(v1, v2);
        angles[1] = Vector2.Angle(v1, v3);
        angles[2] = Vector2.Angle(v1, v4);
        angles[3] = Vector2.Angle(v1, v5);
        angles[4] = Vector2.Angle(v1, v6);

        return angles;
    }

    public bool GoingDown()
    {
        if (rb.velocity.y <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void LockMovementX()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    public void Impulse()
    {
        if (transform.localRotation.y == 1)
        {
            rb.velocity = new Vector2(-forceOfImpulse, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(forceOfImpulse, rb.velocity.y);
        }

    }

    public void FlipInDirection()
    {
        
        if (PlayerMovement.instance.horizontal < 0)
        {
            
            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
        else if(PlayerMovement.instance.horizontal > 0)
        {
            
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
    }

    
}

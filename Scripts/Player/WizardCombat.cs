using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardCombat : MonoBehaviour
{
    public bool airAttackTutorial;
    public bool attackTutorial;
    public bool fullComboTutorial; //para que no haya errores lo dejo
    public float minimumHeightToAirAttack;
    public static WizardCombat instance;
    [SerializeField] AudioClip soundAttack;
    Animator myAnim;
    public AudioSource audioAttacks;
    public bool isAttacking;
    public bool combing;

    Rigidbody2D rb;
    [SerializeField] LayerMask WhatIsGround;

    [SerializeField] GameObject magicBall;

    [SerializeField] GameObject magicAirBall;

    [SerializeField] Transform firePoint;
    [SerializeField] Transform firePointAir;

    public Collider2D boxCollider2d;

    public float timeToDoAttackWizard;

    public float timeToDoAttackWizardAir;
    private void Awake() 
    {
        instance = this;
    }
    void Start()
    {
        
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        myAnim = GetComponent<Animator>();
        fullComboTutorial = true;
        airAttackTutorial = false;
        attackTutorial = false;
        combing = false;
        isAttacking = false;
    }

    private void OnEnable()
    {
        PlayerInputs.OnAttack += DoAttack;
    }

    private void OnDisable()
    {
        PlayerInputs.OnAttack -= DoAttack;
        
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
    public void DoAttack()
    {
        isAttacking = true;
    }

    public void Attack()
    {
        Tutorial();
        StartCoroutine(WaitToAttack());
        audioAttacks.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioAttacks.PlayOneShot(soundAttack);
    }

    public void AirAttack()
    {
        Tutorial();
        StartCoroutine(WaitToAttackAir());
        audioAttacks.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioAttacks.PlayOneShot(soundAttack);
    }

    IEnumerator WaitToAttack()
    {
        yield return new WaitForSeconds(timeToDoAttackWizard);
        CastMagicBall();
    }

    IEnumerator WaitToAttackAir()
    {
        yield return new WaitForSeconds(timeToDoAttackWizardAir);
        CastMagicAirBall();
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

    void CastMagicBall()
    {
        attackTutorial = true;
        Instantiate(magicBall,firePoint.position,Quaternion.identity);
    }

    void CastMagicAirBall()
    {
        airAttackTutorial = true;
        Instantiate(magicAirBall,firePointAir.position,Quaternion.identity);
    }
    public void Hitted()
    {
        StopAllCoroutines();
        isAttacking = false;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public bool bloqued;
    public static PlayerJump instance;
    public float jumpHeight;
    private float gravity;
    private float jumpVelocity;
    public float timeToApex;

    public float jumpTime;
    private float jumpTimeCounter;
    public bool isJumping;
    private bool buttonPressed;
    [SerializeField] float jumpRounded;

    [SerializeField] ParticleSystem jumpUpVFX;
    [SerializeField] ParticleSystem jumpDownVFX;

    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip soundUp;

    public AudioSource audioSource;

    Vector2 _lastVelocity;

    public float groundCheckHeight;

    public float velOnFall;

    public Collider2D boxCollider2d;
    Rigidbody2D rb;
    Animator anim;
    bool lastGrounded = false;



   

    [SerializeField] LayerMask WhatIsGround;

    public bool grounded = true;

    [System.NonSerialized]
    public Transform lastPlatform;
    public float variableGravedad;


    private void Awake()
    {
        instance = this;
    }

    private void OnDrawGizmos()
    {
        //distancia de salto minima
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(new Vector3(boxCollider2d.bounds.center.x,boxCollider2d.bounds.center.y+boxCollider2d.bounds.extents.y,0),new Vector3(boxCollider2d.bounds.center.x,boxCollider2d.bounds.center.y+boxCollider2d.bounds.extents.y + jumpHeight,0));
    }
    void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        SetGravity();
        SetVelocity();
    }

    private void OnEnable()
    {
        bloqued = false;
        PlayerInputs.OnJump += DoJump;
        PlayerInputs.OnEndJump += EndJump;
    }

    private void OnDisable()
    {
        PlayerInputs.OnJump -= DoJump;
        PlayerInputs.OnEndJump -= EndJump;
    }

    private void FixedUpdate()
    {
        GetLastPlatform();
        if (OnZenit() || GoingDown() && !PlayerRoll.instance.doingRoll && !PlayerDash.instance.doingAirDash && !grounded)
        {
            SlowFall();
        }

  
        if (isJumping)
        {
            DisableDownParticles();
        }


        if (OnZenit())
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }

        if (CheckIfTouchedRoof())
        {
            EndJump();
        }

        IsGrounded();
        CheckIfHoldingButton();
        CheckIfTouchedFloor();
        _lastVelocity = rb.velocity;
        lastGrounded = grounded;


        anim.SetBool("Grounded", grounded);
       
    }

    


    public void DoJump()
    {

        if(!bloqued)
        {

            SetGravity();
            if(Character.currentType == CharacterType.Warrior)
            {
                if(grounded && !MeleCombat.instance.combing && !PlayerDash.instance.doingAirDash && !PlayerRoll.instance.doingRoll && !Ability1Manager.instance.doAbility && !Ability2Manager.instance.doAbility && rb != null)
                {
                    
                    EnableUpParticles();
                    audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
                    audioSource.PlayOneShot(soundUp);
                    Tutorial();
                    var vel = new Vector2(rb.velocity.x, jumpVelocity * Time.fixedDeltaTime * jumpRounded);
                    rb.velocity = vel;
                    buttonPressed = true;
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                }
            }
            else
            {
                if(grounded && !WizardCombat.instance.combing && !PlayerDash.instance.doingAirDash && !PlayerRoll.instance.doingRoll && !Ability1Manager.instance.doAbility && !Ability2Manager.instance.doAbility && rb != null)
                {
                    
                    EnableUpParticles();
                    audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
                    audioSource.PlayOneShot(soundUp);
                    Tutorial();
                    var vel = new Vector2(rb.velocity.x, jumpVelocity * Time.fixedDeltaTime * jumpRounded);
                    rb.velocity = vel;
                    buttonPressed = true;
                    isJumping = true;
                    jumpTimeCounter = jumpTime;
                }
            }    
        }
    }
    public void EndJump()
    {
        buttonPressed = false;
        isJumping = false;
    }

    void Tutorial()
    {
        if(FindObjectOfType<TutorialJump>() != null)
        {
            FindObjectOfType<TutorialJump>().Done();
        }
    }


    private void IsGrounded()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast((Vector2)boxCollider2d.bounds.center, (Vector2)boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckHeight, WhatIsGround);
        grounded = raycastHit2d.collider != null;
    }

    private void GetLastPlatform()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckHeight, WhatIsGround);
        if(raycastHit2d.collider != null)
        {
            lastPlatform = raycastHit2d.collider.transform; 
        }
    }

    public void SetGravity()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToApex, 2);
        rb.gravityScale = gravity / Physics2D.gravity.y;
    }

    public void SlowFall()
    {
        
       
        rb.gravityScale = velOnFall;
    }

    private void SetVelocity()
    {
        
        jumpVelocity = Mathf.Abs(gravity) * timeToApex;
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

    private void CheckIfHoldingButton()
    {

        if (buttonPressed && isJumping && !CheckIfTouchedRoof() && !PlayerDash.instance.doingAirDash && !PlayerRoll.instance.doingRoll && !Ability1Manager.instance.doAbility && !Ability2Manager.instance.doAbility)
        {
            if (jumpTimeCounter > 0)
            {
                

                    var vel = new Vector2(rb.velocity.x, jumpVelocity * Time.fixedDeltaTime * jumpRounded);
                    vel.y /= 1.2f;
                    rb.velocity = vel;
                    jumpTimeCounter -= Time.fixedDeltaTime;
                    
                
            }
            else
            {
                
                isJumping = false;
                
            }
        }
    }

    void EnableUpParticles()
    {
        jumpUpVFX.Play();
    }

    void EnableDownParticles()
    {
        jumpDownVFX.Play();
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
    }

    void DisableDownParticles()
    {

        jumpDownVFX.Stop();
    }



    public bool OnZenit()
    {
        if (_lastVelocity.y > 0 && rb.velocity.y <= 0) { return true; } else { return false; }
        //if (rb.velocity.y < 0.5f && rb.velocity.y > 0) { return true; } else { return false; }
    }


    public bool Moving()
    {
        if (rb.velocity.x > 0 || rb.velocity.x < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CheckIfTouchedFloor()
    {
        if(!lastGrounded && grounded && rb.velocity.y <= 0)
        {
            SlowFall();
            EnableDownParticles();
        }
    }

    bool CheckIfTouchedRoof()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, groundCheckHeight, WhatIsGround);
        return raycastHit2d.collider != null;
    }
    public bool CheckIfFalled()
    {
        if(rb.velocity.y < 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    
}

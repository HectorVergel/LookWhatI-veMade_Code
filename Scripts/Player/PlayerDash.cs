using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDash : MonoBehaviour
{
    public bool bloqued;
    Image image;
    public static PlayerDash instance;
    public bool airDashed = false;
    public bool doingAirDash = false;
    public bool canAirDash;
    public float minimumHeightToAirDash;

    [SerializeField] int dashDamage;
    public float airDashDistance;
    Rigidbody2D rb;
    Animator anim;
    public Collider2D boxCollider2d;
    [SerializeField] float dashVelocity;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] LayerMask WhatIsEnemy;

    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] ParticleSystem dashVFX;

    [SerializeField] float timeToPlay;

    public AudioSource audioSource;
    public string dashName;

    private void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        dashVFX.Stop();
    }

    private void OnDrawGizmos()
    {
        //distancia de dash
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(boxCollider2d.bounds.center.x+boxCollider2d.bounds.extents.x,boxCollider2d.bounds.center.y,0),new Vector3(boxCollider2d.bounds.center.x+boxCollider2d.bounds.extents.x + airDashDistance - 0.2f,boxCollider2d.bounds.center.y,0));
        //altura minima para hacer dash
        Gizmos.DrawLine(new Vector3 (boxCollider2d.bounds.center.x,boxCollider2d.bounds.center.y - boxCollider2d.bounds.extents.y,0),new Vector3 (boxCollider2d.bounds.center.x,boxCollider2d.bounds.center.y - minimumHeightToAirDash - boxCollider2d.bounds.extents.y,0));
    }
    private void Update()
    {
        CheckStopAirDash();
        ResetCanAirDash();
        if(image != null)
        {
            if(CanIDash())
            {
                image.fillAmount = 1;
            }
            else
            {
                image.fillAmount = 0;
            }
        }
        else if(GameObject.FindGameObjectWithTag("DashUI") != null)
        {
            image = GameObject.FindGameObjectWithTag("DashUI").GetComponent<Image>();
            if(CanIDash())
            {
                image.fillAmount = 1;
            }
            else
            {
                image.fillAmount = 0;
            }
        }

    }

    bool CanIDash()
    {
        if(!doingAirDash && (canAirDash || airDashed) && HaveHeightToAirDash())
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnEnable()
    {
        bloqued = false;
        PlayerInputs.OnDash += OnDash;
    }

    private void OnDisable()
    {
        PlayerInputs.OnDash -= OnDash;
    }

    public void OnDash()
    {
        if(!bloqued)
        {
            if(!CanIDash() && !PlayerRoll.instance.CanRoll())
            {
                audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
                audioSource.PlayOneShot(noManaSound);
            }
            if (!doingAirDash && PlayerPrefs.GetInt(dashName,0) == 1)
            {
                airDashed = true;
            }
        }
    }

    void Tutorial()
    {
        if(FindObjectOfType<TutorialDash>() !=null)
        {
            if(FindObjectOfType<TutorialDash>().triggered)
            {
                FindObjectOfType<TutorialDash>().Done();
            }
        }
    }

    public void StartAirDash()
    {
        Tutorial();
        StartCoroutine(AirDash());
        dashVFX.Play();
    }
    public bool HaveHeightToAirDash()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, minimumHeightToAirDash, WhatIsGround);
        return raycastHit2d.collider == null;
    }
    public bool CanIAirDash()
    {
        return HaveHeightToAirDash() && canAirDash;
    }
    IEnumerator AirDash()
    {
        PlayerJump.instance.EndJump();
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
        float XfinalPosition;
        canAirDash = false;
        doingAirDash = true;
        rb.gravityScale = 0;
        
        if (transform.localRotation.y == 1)
        {
            
            XfinalPosition = boxCollider2d.bounds.center.x - airDashDistance;
            rb.velocity = new Vector2(-dashVelocity, 0);
        }
        else
        {
            
            XfinalPosition = boxCollider2d.bounds.center.x + airDashDistance;
            rb.velocity = new Vector2(dashVelocity, 0);
        }

        while (!(boxCollider2d.bounds.center.x >= XfinalPosition - 0.2f && boxCollider2d.bounds.center.x <= XfinalPosition + 0.2f))
        {
            rb.gravityScale = 0;
            if (transform.localRotation.y == 1)
            {
                rb.velocity = new Vector2(-dashVelocity, 0);
            }
            else
            {
                rb.velocity = new Vector2(dashVelocity, 0);
            }
            yield return null;
        }

        if (transform.localRotation.y == 1)
        {
            rb.velocity = new Vector2(0, 0);
        }
        else
        {
            rb.velocity = new Vector2(0, 0);
        }
        PlayerJump.instance.SlowFall();
        doingAirDash = false;
        
    }


    void CheckStopAirDash()
    {
        if (doingAirDash)
        {

            if (GetEnemyInFront())
            {
                HealthSystemPlayer.instance.hitted = true;
                doingAirDash = false;
                PlayerJump.instance.SlowFall();
                StopAllCoroutines();
                GetEnemyInFront().collider.GetComponent<IHaveHealth>().TakeDamage(dashDamage);
                StartCoroutine(HittedTrue());
                if(transform.localRotation.y == 1)
                {
                    rb.velocity = new Vector2(1, -1);
                }
                else
                {
                    rb.velocity = new Vector2(-1, -1);
                }
                audioSource.Stop();
            }

            if (HaveSomethingInFront() || PlayerJump.instance.grounded)
            {
                doingAirDash = false;
                dashVFX.Stop();
                PlayerJump.instance.SlowFall();
                StopAllCoroutines();
                audioSource.Stop();
                
            }

        }
    }

    IEnumerator HittedTrue()
    {
        yield return new WaitForSeconds(0.2f);
        HealthSystemPlayer.instance.hitted = false;
    }

    public bool HaveSomethingInFront()
    {
        if (transform.localRotation.y == 1)
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, PlayerJump.instance.groundCheckHeight, WhatIsGround);
            return raycastHit2d.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.right, PlayerJump.instance.groundCheckHeight, WhatIsGround);
            return raycastHit2d.collider != null;
        }

    }


    public RaycastHit2D GetEnemyInFront()
    {
        if (transform.localRotation.y == 1)
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, PlayerJump.instance.groundCheckHeight, WhatIsEnemy);
            return raycastHit2d;
        }
        else
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.right, PlayerJump.instance.groundCheckHeight, WhatIsEnemy);
            return raycastHit2d;
        }
    }

    

   


    public bool HaveSomethingAtTop()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.up, PlayerJump.instance.groundCheckHeight, WhatIsGround);
        return raycastHit2d.collider != null;
    }

    private void ResetCanAirDash()
    {   
        if (PlayerJump.instance.grounded && !HaveSomethingInFront())
        {
            canAirDash = true;
        }
    }
}

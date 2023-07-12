using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerRoll : MonoBehaviour
{
    public bool bloqued;
    Image image;
    public float roofDetection;
    public static PlayerRoll instance;
    public bool rolled = false;
    public bool doingRoll = false;
    public bool canRoll = true;
    public float rollVelocity;
    public float rollDistance;
    public float cooldown;
    float timer;
    Rigidbody2D rb;
    public Collider2D boxCollider2d;

    public Collider2D smallboxCollider2d;
    [SerializeField] LayerMask WhatIsGround;

    [SerializeField] AudioClip sound;
    [SerializeField] AudioClip noManaSound;

    [SerializeField] float timeToPlay;

    public AudioSource audioSource;
    public string rollName;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update

    private void OnEnable()
    {
        bloqued = false;
        PlayerInputs.OnRoll += OnRoll;
    }

    private void OnDisable()
    {
        PlayerInputs.OnRoll -= OnRoll;
    }

    void Start()
    {
        timer = cooldown;
        boxCollider2d = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDrawGizmos()
    {
        //distancia de dash
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(new Vector3(boxCollider2d.bounds.center.x+boxCollider2d.bounds.extents.x,boxCollider2d.bounds.center.y,0),new Vector3(boxCollider2d.bounds.center.x+boxCollider2d.bounds.extents.x + rollDistance - 0.2f,boxCollider2d.bounds.center.y,0));
        Gizmos.DrawLine(new Vector3(smallboxCollider2d.bounds.center.x,smallboxCollider2d.bounds.center.y + smallboxCollider2d.bounds.extents.y,0),new Vector3(smallboxCollider2d.bounds.center.x,smallboxCollider2d.bounds.center.y + smallboxCollider2d.bounds.extents.y + roofDetection,0));
    }
    // Update is called once per frame
    void Update()
    {
        CooldownRoll();
        CheckStopRoll();
        ResetRoll();
        if(image != null)
        {
            if(CanRoll())
            {
                image.fillAmount = 1;
            }
            else
            {
                image.fillAmount = 0;
            }
        }
        else if(GameObject.FindGameObjectWithTag("RollUI") != null)
        {
            image = GameObject.FindGameObjectWithTag("RollUI").GetComponent<Image>();
            if(CanRoll())
            {
                image.fillAmount = 1;
            }
            else
            {
                image.fillAmount = 0;
            }
        }
    }

    public bool CanRoll()
    {
        if(!doingRoll && canRoll && PlayerJump.instance.grounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void OnRoll()
    {
        if (!doingRoll && PlayerJump.instance.grounded && timer >= cooldown && PlayerPrefs.GetInt(rollName,0) == 1 && !bloqued)
        {
            rolled = true;
        }
    }

    public void StartRoll()
    {
        StartCoroutine(Roll());
    }


    void Tutorial()
    {
        if(FindObjectOfType<TutorialRoll>() !=null)
        {
            if(FindObjectOfType<TutorialRoll>().triggered)
            {
                FindObjectOfType<TutorialRoll>().Done();
            }
        }
    }
    IEnumerator Roll()
    {
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(sound);
        Tutorial();
        bool passedBy = false;
        float XfinalPosition;
        canRoll = false;
        doingRoll = true;
        gameObject.layer = 11;
        
        if (transform.localRotation.y == 1)
        {
            
            XfinalPosition = boxCollider2d.bounds.center.x - rollDistance;
            rb.velocity = new Vector2(-rollVelocity, 0);
        }
        else
        {
            
            XfinalPosition = boxCollider2d.bounds.center.x + rollDistance;
            rb.velocity = new Vector2(rollVelocity, 0);
        }
        while (!passedBy || CheckIfWallAtTop())
        {
            if(!passedBy)
            {
                passedBy = boxCollider2d.bounds.center.x >= XfinalPosition - 0.2f && boxCollider2d.bounds.center.x <= XfinalPosition + 0.2f;
            }
            if (transform.localRotation.y == 1)
            {
                rb.velocity = new Vector2(-rollVelocity, 0);
            }
            else
            {
                rb.velocity = new Vector2(rollVelocity, 0);
            }
            
            yield return null;
        }
       
        rb.velocity = new Vector2(0,0);
        doingRoll = false;
        gameObject.layer = 9;
    }

    bool CheckIfWallAtTop()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(smallboxCollider2d.bounds.center, smallboxCollider2d.bounds.size, 0f, Vector2.up, roofDetection, WhatIsGround);
        return raycastHit2d.collider != null;
    }
    void CheckStopRoll()
    {
        if (doingRoll)
        {
            if (HaveSomethingInFrontBottom() || !PlayerJump.instance.grounded)
            {
                rb.velocity = new Vector2(0,0);
                gameObject.layer = 9;
                doingRoll = false;
                StopAllCoroutines();
                audioSource.Stop();
            }

        }
    }

    public bool HaveSomethingInFrontBottom()
    {
        if (transform.localRotation.y == 1)
        {
            
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(smallboxCollider2d.bounds.center, smallboxCollider2d.bounds.size, 0f, Vector2.left, PlayerJump.instance.groundCheckHeight, WhatIsGround);
            return raycastHit2d.collider != null;
        }
        else
        {
            
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(smallboxCollider2d.bounds.center, smallboxCollider2d.bounds.size, 0f, Vector2.right, PlayerJump.instance.groundCheckHeight, WhatIsGround);
            return raycastHit2d.collider != null;
        }

    }


    void CooldownRoll()
    {
        if(timer >= cooldown)
        {
            canRoll = true;
        }
        else
        {
            canRoll = false;
            timer += Time.deltaTime;
        }
    }

    void ResetRoll()
    {
        if(!PlayerJump.instance.grounded)
        {
            rolled = false;
        }
    }

    public void ResetTime()
    {
        timer = 0;
    }
}

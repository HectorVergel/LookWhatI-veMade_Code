using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool bloqued;
    Rigidbody2D rb;
    Animator anim;
    public float horizontal;
    public float speed;
    public static PlayerMovement instance;

    [SerializeField] ParticleSystem walkVFX;

    public AudioSource audioSource;
    public AudioClip moveSound;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        bloqued = false;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }



    private void OnEnable()
    {
        audioSource.clip = moveSound;
        PlayerInputs.OnMove += SetHorizontal;
    }

    private void OnDisable()
    {
        PlayerInputs.OnMove -= SetHorizontal;
    }

    void Update()
    {
        Move();
        anim.SetFloat("Velocity", rb.velocity.x);
        if (!GetComponent<HealthSystemPlayer>().hitted)
        {
            Flip();
        }
    }

    void Move()
    {
        if(!bloqued)
        {
            if (Character.currentType == CharacterType.Warrior)
            {
                if (!MeleCombat.instance.combing && !PlayerDash.instance.doingAirDash && !PlayerRoll.instance.doingRoll && !HealthSystemPlayer.instance.hitted && !Ability1Manager.instance.doAbility && !Ability2Manager.instance.doAbility)
                {
                    rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
                    StartCoroutine(EnableParticles());
                    if (horizontal != 0 && PlayerJump.instance.grounded)
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                    }
                    else
                    {
                        if (audioSource.isPlaying)
                        {
                            audioSource.Stop();
                        }
                    }
                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
            else
            {
                if (!WizardCombat.instance.combing && !PlayerDash.instance.doingAirDash && !PlayerRoll.instance.doingRoll && !HealthSystemPlayer.instance.hitted && !Ability1Manager.instance.doAbility && !Ability2Manager.instance.doAbility)
                {
                    rb.velocity = new Vector2(horizontal * speed * Time.fixedDeltaTime, rb.velocity.y);
                    StartCoroutine(EnableParticles());
                    if (horizontal != 0 && PlayerJump.instance.grounded)
                    {
                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                    }
                    else
                    {
                        if (audioSource.isPlaying)
                        {
                            audioSource.Stop();
                        }
                    }
                }
                else
                {
                    if (audioSource.isPlaying)
                    {
                        audioSource.Stop();
                    }
                }
            }
        }


        
    }



    void Flip()
    {

        if (rb.velocity.x > 0.01)
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }
        else if (rb.velocity.x < -0.01)
        {

            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }


    }

    public void SetHorizontal(float x)
    {
        horizontal = x;
    }

    IEnumerator EnableParticles()
    {
        yield return new WaitForFixedUpdate();
        if (horizontal != 0 && PlayerJump.instance.grounded)
        {
            if (!walkVFX.isEmitting || !walkVFX.isPlaying)
            {
                walkVFX.Play();
            }



        }
        else
        {
            if (walkVFX.isEmitting || walkVFX.isPlaying)
            {
                walkVFX.Stop();
            }

        }

    }

}

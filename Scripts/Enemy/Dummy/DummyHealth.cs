using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealth : MonoBehaviour, IHaveHealth
{
    [SerializeField]
    float timeHitEffect = 0.35f;

    private float timeToChangeColor;

    private bool startTimerColor;

    Animator anim;
    AudioSource audioSource;
    public AudioClip soundHit;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (startTimerColor)
        {
            BackToOriginalColor();
        }
    }
    public void TakeDamage(float amountOfDamge)
    {
        
        if (CheckIfCrit())
        {
            
            GetComponent<PrintDamage>().ShowText(amountOfDamge * 2, true);
        }
        else
        {
            
            GetComponent<PrintDamage>().ShowText(amountOfDamge, false);
        }
        EnemyHitResponse();
        audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
        audioSource.PlayOneShot(soundHit);

    }

    bool CheckIfCrit()
    {
        float rnd = Random.Range(1, 100);
        if (rnd < StatsSystem.instance.currentLuck * 100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EnemyHitResponse()
    {
        if (this.gameObject.tag == "Enemy")
        {
            ColorChange();
            anim.Play("EnemyHitTemporal");
        }

    }

    void ColorChange()
    {
        var color = GetComponent<SpriteRenderer>().material.color = Color.red;
        startTimerColor = true;
    }
    void BackToOriginalColor()
    {
        if (timeToChangeColor >= timeHitEffect)
        {
            var color = GetComponent<SpriteRenderer>().material.color = Color.white;
            startTimerColor = false;
            timeToChangeColor = 0;
        }
        else
        {
            timeToChangeColor += Time.deltaTime;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthSystem : MonoBehaviour, IHaveHealth
{
    public float maxHealth;
    float currentHealth;
    public float rageHealth;
    public bool rage;
    public bool isAlive;
    public ParticleSystem hitVFX;
    public ParticleSystem hitVFX1;
    public ParticleSystem hitVFX2;
    public ParticleSystem rageVFX3;
    PrintDamage printDamage;
    BossHealthBar healthBar;
    public GameObject dieAnim;
    Animator anim;
    [SerializeField] AudioClip hit;
    AudioSource audioSource;
    private void Start() {
        printDamage = GetComponent<PrintDamage>();
        audioSource = GetComponent<AudioSource>();
        currentHealth = maxHealth;
        rage = false;
        isAlive = true;
        hitVFX.Stop();
        hitVFX1.Stop();
        hitVFX2.Stop();
        healthBar = FindObjectOfType<BossHealthBar>();
        healthBar.SetHelath(currentHealth,maxHealth,false);
        anim = GetComponent<Animator>();
    }
    public void TakeDamage(float amount)
    {
        if(isAlive)
        {
            HitVisuals();
            PlaySound(hit);
            if (CheckIfCrit())
            {
                if(Character.currentType == CharacterType.Warrior)
                {
                    currentHealth -= amount * 2;
                    printDamage.ShowText(amount * 2, true);
                }
                else
                {
                    currentHealth -= amount * 4;
                    printDamage.ShowText(amount * 4, true);
                }
            }
            else
            {
                currentHealth -= amount;
                printDamage.ShowText(amount, false);
            }
            if(currentHealth <= rageHealth && !rage)
            {
                ActivateRage();
            }

            healthBar.SetHelath(currentHealth,maxHealth,currentHealth<=rageHealth);

            if(currentHealth <= 0)
            {
                Die();
            }
        }
    }
    public void PlaySound(AudioClip sound)
    {
        audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
        audioSource.PlayOneShot(sound);
    }
    void ActivateRage()
    {
        rage = true;
        rageVFX3.Play();
        FindObjectOfType<SpearSpawner>().ActivateSpears();
        //poner cosita que queramos al activarse el rage
    }
    void Die()
    {
        rageVFX3.Stop();
        Destroy(healthBar.gameObject);
        Instantiate(dieAnim,transform.position,Quaternion.identity);
        Destroy(this.gameObject);
    }
    void HitVisuals()
    {
        hitVFX.Play();
        hitVFX1.Play();
        hitVFX2.Play();
    }
    bool CheckIfCrit()
    {
        float rnd = Random.Range(1,100);
        if(rnd < StatsSystem.instance.currentLuck*100)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

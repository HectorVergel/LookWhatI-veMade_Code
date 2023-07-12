using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] GameObject bulletPrefab;
    [SerializeField] float fireRate;
    [SerializeField] float timeAnim;
    [SerializeField] Transform firePoint;
    Animator animator;
    private float timer;
    AudioSource audioSource;
    public AudioClip sound;
    public float soundDelay;
    bool played = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        timer = 4;
    }
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= fireRate)
        {
            Shoot();
            timer = 0;
            played = false;
        }
        if(timer >= fireRate - soundDelay && !played)
        {
            audioSource.pitch = 1f + Random.Range(-0.1f,0.1f);
            audioSource.PlayOneShot(sound);
            played = true;
        }
    }

    void Shoot()
    {
        StartCoroutine(PlayAnimation());
        var b = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        b.GetComponent<turretBullet>().forward = -transform.right;
        b.transform.localRotation = transform.localRotation;
    }

    IEnumerator PlayAnimation()
    {
        yield return new WaitForSeconds(timeAnim);
        animator.Play("turret");
    }
}

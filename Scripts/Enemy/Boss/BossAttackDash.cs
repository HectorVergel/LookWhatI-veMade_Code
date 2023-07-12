using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackDash : MonoBehaviour
{
    public float velocity;
    public float stopDistance;
    public float startTime;
    public bool dashing;
    public BoxCollider2D playerBox;
    public static BossAttackDash instance;
    public BoxCollider2D box;
    Rigidbody2D rb;
    AudioSource myAudio;
    public AudioClip attackSound;
    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
    private void Awake() {
        instance = this;
        rb = GetComponent<Rigidbody2D>();
        dashing = false;
        myAudio = GetComponent<AudioSource>();
        vfx1.Stop();
        vfx2.Stop();
    }
    private void Start() {
        GetPlayer();
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(box.bounds.center,box.bounds.center + new Vector3(stopDistance,0,0));
    }
    public void StartAttack()
    {
        dashing = true;
        StartCoroutine(Dash());
    }
    IEnumerator Dash()
    {
        yield return new WaitForSeconds(startTime);
        myAudio.PlayOneShot(attackSound);
        vfx1.Play();
        vfx2.Play();
        while (Mathf.Abs(playerBox.bounds.center.x - box.bounds.center.x) - playerBox.bounds.extents.x >= stopDistance)
        {
            rb.velocity = new Vector2(velocity*Time.fixedDeltaTime,0) * (Vector2)transform.right;
            yield return null;
        }
        StopDash();
    }
    void StopDash()
    {
        rb.velocity = new Vector2(0,0);
        dashing = false;
    }
    void GetPlayer()
    {
        playerBox = FindObjectOfType<HealthSystemPlayer>().GetComponent<BoxCollider2D>();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashState : MonoBehaviour, SMCInterface
{
    public string stateName;
    ShieldEnemyController controller;
    public SMCInterface _nextState { get => GetComponent<EndDashState>();}
    public ShieldEnemy shield;
    public Transform groundPoint;
    public LayerMask WhatIsGrounded;
    public float distanceToDetectWall;
    BoxCollider2D box;
    Rigidbody2D rb;
    public float dashSpeed;
    public float dashTime;
    public int damage;
    public GameObject sound;
    public float soundTime;
    public ParticleSystem vfx;

    private void Start() {
        controller = GetComponent<ShieldEnemyController>();
        box = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnEnter()
    {
        controller.anim.Play(stateName);
        shield.shieldEnabled = true;
        StartCoroutine(Dash());
        StartCoroutine(SoundDash());
        vfx.Play(); 
    }

    public void OnExit()
    {
        StopAllCoroutines();
        shield.shieldEnabled = false;
        rb.velocity = new Vector2(0,rb.velocity.y);
        controller.ChangeCurrentState(_nextState);
        vfx.Stop();
    }

    public void OnUpdate()
    {
        if(!GroundDetected() || WallDetected())
        {
            OnExit();
        }
    }

    bool GroundDetected()
    {
        var hit = Physics2D.Raycast(groundPoint.position, Vector2.down, 0.04f, WhatIsGrounded);
        return hit.collider != null;
    }

    bool WallDetected()
    {
        if (transform.eulerAngles.y == 180)
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0f, Vector2.left, distanceToDetectWall, WhatIsGrounded);
            return raycastHit2d.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(box.bounds.center, box.bounds.size, 0f, Vector2.right, distanceToDetectWall, WhatIsGrounded);
            return raycastHit2d.collider != null;
        }
    }

    IEnumerator Dash()
    {
        float timer = 0;
        while(timer < dashTime)
        {
            rb.velocity = new Vector2(dashSpeed * Time.fixedDeltaTime * transform.right.x, rb.velocity.y);
            timer+=Time.deltaTime;
            yield return null;
        }
        OnExit();
    }

    IEnumerator SoundDash()
    {
        Instantiate(sound,transform.position,Quaternion.identity);
        yield return new WaitForSeconds(soundTime);
        StartCoroutine(SoundDash());
    }

    
}

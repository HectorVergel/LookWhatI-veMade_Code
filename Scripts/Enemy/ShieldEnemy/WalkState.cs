using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkState : MonoBehaviour, SMCInterface
{
    public string stateName;
    ShieldEnemyController controller;
    BoxCollider2D target;
    Rigidbody2D rb;
    public BoxCollider2D box;
    public float rangeDetection;
    public LayerMask whatIsPlayer;
    public float speed;
    float hittedTime;
    public float hittedCooldown;

    [SerializeField] Transform groundPoint;
    [SerializeField] float distanceToDetectWall;
    [SerializeField] LayerMask WhatIsGrounded;

    [SerializeField] int maxTimeToChangeDirection;
    [SerializeField] int minTimeToChangeDirection;
    float counterChangeDirection;
    HealthSystemEnemy healthEnemy;
    KnockBack knock;
    int randomNumber;


    public SMCInterface _nextState { get => GetComponent<PreDashState>();}
    private void Start() {
        knock = GetComponent<KnockBack>();
        healthEnemy = GetComponent<HealthSystemEnemy>();
        controller = GetComponent<ShieldEnemyController>();
        rb = GetComponent<Rigidbody2D>();
        target = controller.target;
        hittedTime = hittedCooldown;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(box.bounds.center, rangeDetection);
    }

    public void OnEnter()
    {
        controller.anim.Play(stateName);
        counterChangeDirection = 0;
        hittedTime = 0;
    }

    public void OnExit()
    {
        rb.velocity = new Vector2(0,rb.velocity.y);
        if(controller.CheckIfFacingPlayer())
        {
            Flip();
        }
        controller.ChangeCurrentState(_nextState);
    }

    public void OnUpdate()
    {
        if(CheckPlayerInRange() && hittedTime > hittedCooldown)
        {
            OnExit();
        }
        else
        {
            if(!knock.knocked)
            {
                Patrol();
            }
            ResetHitted();
        }
    }

    void Patrol()
    {
        if((!GroundDetected() && rb.velocity.y == 0) || WallDetected() && !healthEnemy.hitted)
        {
            Flip();
        }
        else
        {
            ChangeDirectionRandomly();
        }


        if (!healthEnemy.hitted)
        {
            rb.velocity = new Vector2(transform.right.x * speed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
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
    void Flip()
    {
        if (transform.localRotation.y == 0)
        {

            transform.localRotation = new Quaternion(0, 180, 0, 0);
        }
        else
        {
            transform.localRotation = new Quaternion(0, 0, 0, 0);
        }

        randomNumber = Random.Range(minTimeToChangeDirection, maxTimeToChangeDirection);
    }
    void ChangeDirectionRandomly()
    {
        counterChangeDirection += Time.deltaTime;
        if (counterChangeDirection >= randomNumber)
        {  
            Flip();
            counterChangeDirection = 0;
        }
    }
    bool CheckPlayerInRange()
    {
        if(target != null)
        {
            Collider2D collider = Physics2D.OverlapCircle(box.bounds.center,rangeDetection,whatIsPlayer);
            return collider != null;
        }
        else
        {
            target = controller.target;
            return false;
        }
    }

    void ResetHitted()
    {
        if(healthEnemy.hitted)
        {
            hittedTime = 0;
        }
        else
        {
            hittedTime+=Time.deltaTime;
        }
    }
}

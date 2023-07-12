using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrollingBasic : MonoBehaviour
{
   float speedPatrolling;
   [SerializeField] float speedFolloing;
   [SerializeField] Transform detectionPoint;
    [SerializeField] Transform groundPoint;
   [SerializeField] float distanceToDetectWall;
   [SerializeField] LayerMask WhatIsGrounded;

    [SerializeField] int maxTimeToChangeDirection;
    [SerializeField] int minTimeToChangeDirection;

    [SerializeField] float timeStoppedBetweenChange;
    float counterChangeDirection;

    [SerializeField] float maxSpeedPatrolling;
    [SerializeField] float minSpeedPatrolling;


    BoxCollider2D boxCollider2d;


    Rigidbody2D rb;
   AttackSmall attackSmall;
   HealthSystemEnemy healthEnemy;
   EnemyDetection detection;
   Transform player;
    KnockBack knock;

    bool canMove = true;
    int randomNumber;

    public bool attacking;
   

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        attackSmall = GetComponent<AttackSmall>();
        healthEnemy = GetComponent<HealthSystemEnemy>();
        detection = GetComponent<EnemyDetection>();
        randomNumber = Random.Range(minTimeToChangeDirection, maxTimeToChangeDirection);
        speedPatrolling = GetRandomSpeed();
        knock = GetComponent<KnockBack>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        SetTarget();
    }

    // Update is called once per frame

    void Update()
    {
        if(player == null)
        {
            SetTarget();
        }
        

        if (!knock.knocked && healthEnemy.isAlive)
        {
            if (rb.velocity.y == 0 && !attacking)
            {
                if ((detection.visionArea || healthEnemy.hitted) && player != null)
                {
                    FollowTarget();
                }
                else
                {
                    Patrol();
                }
            }
            else
            {
               
                if (detection.visionArea)
                {
                    rb.velocity = new Vector2(0, rb.velocity.y);
                }
               
            }
        }
        
       
    }

    
       
    void SetTarget()
    {
        if(FindObjectOfType<HealthSystemPlayer>() != null)
        {
            player = FindObjectOfType<HealthSystemPlayer>().transform;
        }
    }
    void Patrol()
    {
        if((!GroundDetected() && rb.velocity.y == 0) || WallDetected())
        {
            Flip();
        }
        else
        {
            ChangeDirectionRandomly();
        }


        if (canMove && !healthEnemy.hitted)
        {
            rb.velocity = new Vector2(transform.right.x * speedPatrolling * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        

    }

    void ChangeDirectionRandomly()
    {
        counterChangeDirection += Time.deltaTime;

        
        if (counterChangeDirection >= randomNumber)
        {
            
            StartCoroutine(ChangeDirection());
            counterChangeDirection = 0;
            
            
        }
    }


    

    private float GetRandomSpeed()
    {
        return Random.Range(minSpeedPatrolling, maxSpeedPatrolling);
    }


    IEnumerator ChangeDirection()
    {

        canMove = false;
        yield return new WaitForSeconds(timeStoppedBetweenChange);
        canMove = true;
        Flip();
        
    }

    public void FollowTarget()
    {
        if(player.position.x > transform.position.x)
        {
            if(transform.localRotation.y != 0)
            {
                Flip();
            }
        }
        else
        {
            if(transform.localRotation.y != 1)
            {
                Flip();
            }
        }
        if (!healthEnemy.hitted)
        {
            rb.velocity = new Vector2(transform.right.x * speedFolloing * Time.fixedDeltaTime, rb.velocity.y);

        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
    
    }

    void Flip()
    {
        if (!attacking)
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
       
        
    }

    private bool GroundDetected()
    {
        var hit = Physics2D.Raycast(groundPoint.position, Vector2.down, 0.04f, WhatIsGrounded);
        return hit.collider != null;
    }

    private bool WallDetected()
    {
        if (transform.eulerAngles.y == 180)
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.left, distanceToDetectWall, WhatIsGrounded);
            return raycastHit2d.collider != null;
        }
        else
        {
            RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.right, distanceToDetectWall, WhatIsGrounded);
            return raycastHit2d.collider != null;
        }
    }
}
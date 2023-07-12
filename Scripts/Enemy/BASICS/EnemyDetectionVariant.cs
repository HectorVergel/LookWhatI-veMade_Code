using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetectionVariant : MonoBehaviour
{
    [SerializeField] LayerMask WhatIsPlayer;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] private float range_follow;
    [SerializeField] LayerMask WhatIsGround;
    public Transform visionPoint1;
    public Transform visionPoint2;
    public float groundCheckHeight;
    public bool visionArea;
    public Transform player;

    void Start()
    {
        if (player == null)
        {
            SetTarget();
        }
    }

    
    void Update()
    {
        if (player == null)
        {
            SetTarget();
        }
      IsInRangeToFollow(range_follow);
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_follow);
    }

   
    void SetTarget()
    {
        if (FindObjectOfType<HealthSystemPlayer>() != null)
        {
            player = FindObjectOfType<HealthSystemPlayer>().transform;
        }
    }
    public void IsInRangeToFollow(float range)
    {
        Collider2D collider = Physics2D.OverlapCircle(boxCollider2d.bounds.center, range, WhatIsPlayer);

        if (collider != null)
        {
            RaycastHit2D ray = Physics2D.Raycast(visionPoint1.position, (collider.bounds.center - visionPoint1.position).normalized, (collider.bounds.center - visionPoint1.position).magnitude, WhatIsGround);
            RaycastHit2D ray2 = Physics2D.Raycast(visionPoint2.position, (collider.bounds.center - visionPoint2.position).normalized, (collider.bounds.center - visionPoint2.position).magnitude, WhatIsGround);

            if(!ray || !ray)
            {
                visionArea = true;
            }
            else
            {
                visionArea = false;
            }
        }
        else
        {
            visionArea = false;
        }
    }
}
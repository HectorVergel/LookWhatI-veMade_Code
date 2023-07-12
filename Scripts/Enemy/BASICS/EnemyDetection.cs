using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{

    [SerializeField] LayerMask WhatIsPlayer;
    [SerializeField] Collider2D boxCollider2d;
    [SerializeField] private float range_follow;
    [SerializeField] float offset;
    [SerializeField] LayerMask WhatIsGround;
    [SerializeField] Transform visionAreaStart;
    public float groundCheckHeight;
    public bool visionArea;
    Transform player;
    [System.NonSerialized]
    public Transform lastPlatform;

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
        GetLastPlatform();
        if (CheckIfInSamePlatform())
        {
           IsInRangeToFollow(range_follow);
        }
        else
        {
            visionArea = false;
        }
       
        
    }

    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(boxCollider2d.bounds.center, range_follow);

        


        Gizmos.color = Color.blue;

        Gizmos.DrawLine(visionAreaStart.position, visionAreaStart.position - new Vector3(0, offset, 0));
        Gizmos.DrawLine(visionAreaStart.position, visionAreaStart.position + new Vector3(0, offset, 0));
    }

    private void GetLastPlatform()
    {
        RaycastHit2D raycastHit2d = Physics2D.BoxCast(boxCollider2d.bounds.center, boxCollider2d.bounds.size, 0f, Vector2.down, groundCheckHeight, WhatIsGround);
        if (raycastHit2d.collider != null)
        {
            lastPlatform = raycastHit2d.collider.transform;
        }
    }
    void SetTarget()
    {
        if (FindObjectOfType<HealthSystemPlayer>() != null)
        {
            player = FindObjectOfType<HealthSystemPlayer>().transform;
        }
    }
    public bool CheckIfInSamePlatform()
    {
        if (player != null)
        {
            if (lastPlatform == player.GetComponent<PlayerJump>().lastPlatform)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }
    public void IsInRangeToFollow(float range)
    {
        Collider2D collider = Physics2D.OverlapCircle(boxCollider2d.bounds.center, range, WhatIsPlayer);

        if (collider != null)
        {
           
            RaycastHit2D ray = Physics2D.Raycast(visionAreaStart.position + new Vector3(0,offset,0), (collider.transform.position - this.transform.position).normalized, (collider.transform.position - this.transform.position).magnitude, WhatIsGround);
            RaycastHit2D ray2 = Physics2D.Raycast(visionAreaStart.position - new Vector3(0,offset,0), (collider.transform.position - this.transform.position).normalized, (collider.transform.position - this.transform.position).magnitude, WhatIsGround);

            if(!ray || !ray2)
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
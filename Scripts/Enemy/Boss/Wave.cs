using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int damage;
    public float velocityNormal;
    public float velocityRage;
    public int direction;
    public float distanceDetection;
    public float detectionOffset;
    public LayerMask whatIsGround;
    public CircleCollider2D col;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if(FindObjectOfType<BossHealthSystem>().rage)
        {
            rb.velocity = new Vector2(velocityRage * Time.fixedDeltaTime,0) * direction;
        }
        else
        {
            rb.velocity = new Vector2(velocityNormal * Time.fixedDeltaTime,0) * direction;
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(col.bounds.center,col.bounds.center + new Vector3(distanceDetection,0,0));
        Gizmos.DrawLine(col.bounds.center,col.bounds.center + new Vector3(0,detectionOffset,0));
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            other.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
        }
    }
    private void Update() {
        CheckIfWall();
    }

    void CheckIfWall()
    {
        RaycastHit2D ray;

        if(rb.velocity.x > 0)
        {
            ray = Physics2D.Raycast((Vector2)col.bounds.center + new Vector2(0, detectionOffset), transform.right, distanceDetection, whatIsGround);
        }
        else
        {
            ray = Physics2D.Raycast((Vector2)col.bounds.center + new Vector2(0, detectionOffset), -transform.right, distanceDetection, whatIsGround);
        }
       

        if(ray.collider != null)
        {
            if(ray.collider.tag != "Spear")
            {
                //cambiar destroy por animacion de la wave desapareciendo pabajo i despues destroy
                Destroy(this.gameObject);
            }
        }
        
    }
}

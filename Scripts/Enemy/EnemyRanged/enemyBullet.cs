using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public int damage;
    [SerializeField] float speed;
    [SerializeField] GameObject vfx;
    BoxCollider2D player;
    Rigidbody2D rb;
    public bool defelected;
    void Start()
    {
        defelected = false;
        rb = GetComponent<Rigidbody2D>();
        SetTarget();
        Move();
    }

    void SetTarget()
    {
        if(FindObjectOfType<HealthSystemPlayer>() != null)
        {
            player = FindObjectOfType<HealthSystemPlayer>().GetComponent<BoxCollider2D>();   
        }
    }
    void Move()
    {
        if(player != null)
        {
            Vector2 directionToPlayer = (player.bounds.center - GetComponent<BoxCollider2D>().bounds.center).normalized;
            rb.velocity = directionToPlayer * speed * Time.fixedDeltaTime; 
        }
        else
        {
            Destroy(this.gameObject);
        }  
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
            Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if(collision.tag == "Enemy" && defelected)
        {
            collision.GetComponent<IHaveHealth>().TakeDamage(damage);
            Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
        if (collision.tag == "Ground")
        {
            Instantiate(vfx, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
            
        }

    }
}

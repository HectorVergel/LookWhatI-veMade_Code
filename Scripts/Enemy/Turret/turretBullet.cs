using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretBullet : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float speed;
    [System.NonSerialized]
    public Vector2 forward;
    public GameObject vfx;
    Rigidbody2D rb;
    public bool defelected;
    void Start()
    {
        defelected = false;
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    void Move()
    {
        rb.velocity = forward * speed * Time.fixedDeltaTime;
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

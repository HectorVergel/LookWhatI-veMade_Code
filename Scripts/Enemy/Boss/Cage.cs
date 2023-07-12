using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cage : MonoBehaviour
{
    [SerializeField] float velocity;
    [SerializeField] int damage;
    public int direction;
    BoxCollider2D box;
    Rigidbody2D rb;
    public bool defelected;
   
    [SerializeField] GameObject vfx;
    void Start()
    {
        defelected = false;
        rb = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
        rb.velocity = new Vector2(velocity * direction * Time.fixedDeltaTime, 0);
        if(rb.velocity.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.tag == "Player")
        {
            collision.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
            OnDestroy();
        }
        if(collision.tag == "Enemy" && defelected)
        {
            collision.GetComponent<IHaveHealth>().TakeDamage(damage);
            OnDestroy();
        }
        if(collision.tag == "Ground")
        {
            OnDestroy();
        }
    }

    void OnDestroy()
    {
        //particulas
        Instantiate(vfx, box.bounds.center, Quaternion.identity);
        Destroy(gameObject);
    }
}

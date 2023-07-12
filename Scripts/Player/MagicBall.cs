using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBall : MonoBehaviour
{
    [SerializeField] float damageScaling;
    [SerializeField] float speed;
    [SerializeField] float manaRegenPercent;
    [SerializeField] float range;
    [SerializeField] GameObject destroyVFX;
    [SerializeField] LayerMask WhatIsEnemy;
    Rigidbody2D rb;
    [SerializeField]Collider2D collider2d;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Move();
    }

    void Move()
    { 
        if(FindObjectOfType<HealthSystemPlayer>().transform.localRotation.y == 1)
        {
            rb.velocity = new Vector2(-speed,0);
        }
        else
        {
            rb.velocity = new Vector2(speed,0);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(collider2d.bounds.center, range);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Enemy")
        {
            CheckHit();
            Destroy(this.gameObject);
            Instantiate(destroyVFX, this.transform.position, Quaternion.identity);
        }
        if(collision.tag == "Ground")
        {
            CheckHit();
            Destroy(this.gameObject);
            Instantiate(destroyVFX, this.transform.position, Quaternion.identity);
        }
        if(collision.gameObject.layer == 19)
        {
            if (collision.GetComponent<MineBox>() != null)
            {
                CheckHit();
                collision.GetComponent<MineBox>().DestroyObject();
                Destroy(this.gameObject);
                Instantiate(destroyVFX, this.transform.position, Quaternion.identity);
            }
        }
       
    }

    void CheckHit()
    {
        bool hittedEnemy = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(collider2d.bounds.center, range, WhatIsEnemy);

        foreach (var enemy in colliders)
        {
            float damage = 1 + Mathf.RoundToInt(PlayerPrefs.GetInt("Strength",0) * damageScaling + 0.01f);
            enemy.GetComponent<IHaveHealth>().TakeDamage(damage);
            hittedEnemy = true;
        }

        if (hittedEnemy)
        {
            FindObjectOfType<HealthSystemPlayer>().GetComponent<Mana>().AddMana(manaRegenPercent);
        }
    }
}

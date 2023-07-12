using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public bool knocked;
    [SerializeField] private float friction;
    IEnumerator DoKnockBackLateral(bool right,float knockbackLateral)
    {   
        knocked = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (right)
        {
            
            rb.velocity = new Vector2(knockbackLateral * Time.fixedDeltaTime, rb.velocity.y);
            while (rb.velocity.x > 0)
            {

                    rb.velocity = new Vector2(rb.velocity.x - friction * Time.fixedDeltaTime, rb.velocity.y);
                
                yield return null;
            }

        }
        else
        {
            rb.velocity = new Vector2(-knockbackLateral * Time.fixedDeltaTime, rb.velocity.y);
            while (rb.velocity.x < 0)
            {
               
                    rb.velocity = new Vector2(rb.velocity.x + friction * Time.fixedDeltaTime, rb.velocity.y);
                
                yield return null;
            }

        }

        



        knocked = false;
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    IEnumerator DoKnockBackTop(float knockbackTop)
    {
        knocked = true;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(0, knockbackTop * Time.fixedDeltaTime);
        while (rb.velocity.y > 0 && !PlayerJump.instance.grounded)
        {
            yield return null;
        }
        rb.velocity = new Vector2(0, rb.velocity.y);
        knocked = false;
    }

    public void CallKnockBackLateral(bool right,float forceKnockback)
    {
        StartCoroutine(DoKnockBackLateral(right,forceKnockback));
    }
    public void CallKnockBackTop(float forceKnockback)
    {
        StartCoroutine(DoKnockBackTop(forceKnockback));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitCollision : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] int hitCollisionDamage;
    [SerializeField] BoxCollider2D box;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            if (!collision.gameObject.GetComponent<HealthSystemPlayer>().invulnerable && !PlayerDash.instance.doingAirDash && !HealthSystemPlayer.instance.hitted)
            {
                collision.gameObject.GetComponent<HealthSystemPlayer>().TakeDamage(hitCollisionDamage);
                
                
            }
            
        }
    }


   
}

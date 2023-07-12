using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldEnemy : MonoBehaviour
{
    public DashState dash;
    public bool shieldEnabled;
    [System.NonSerialized]
    public BoxCollider2D shieldBox;
    private void Start() {
        shieldBox = GetComponent<BoxCollider2D>();
        shieldEnabled = false;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && shieldEnabled)
        {
            other.GetComponent<HealthSystemPlayer>().TakeDamage(dash.damage);
        }
    }
}

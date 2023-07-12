using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingPoint : MonoBehaviour
{
    public Transform nextPoint;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Enemy")
        {
            
            EnemyAirPatrolling patrolling = other.GetComponent<EnemyAirPatrolling>();
            if(patrolling != null)
            {
                patrolling.nextPoint = nextPoint;
                
            }
        }
    }
}

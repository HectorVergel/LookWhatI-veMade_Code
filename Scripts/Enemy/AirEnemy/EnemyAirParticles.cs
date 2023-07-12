using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAirParticles : MonoBehaviour
{
    public Transform mainParticles;
    public Transform trailParticles;
    Transform myTransform;
    private void Start() {
        myTransform = GetComponent<Transform>();
    }

    void Update()
    {
        mainParticles.position = myTransform.position;
        trailParticles.position = myTransform.position;
    }
}

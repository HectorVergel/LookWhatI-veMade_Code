using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAirPatrolling : MonoBehaviour
{
    EnemyDetectionVariant detection;
    AIDestinationSetter aiDestination;
    public Transform nextPoint;
    public Transform myTransform;
    AudioSource audioSource;
    EnemyAirAttack airAttack;
    public AudioClip sound;
    private void Start() {
        detection = GetComponent<EnemyDetectionVariant>();
        aiDestination = GetComponent<AIDestinationSetter>();
        myTransform = GetComponent<Transform>();
        airAttack = GetComponent<EnemyAirAttack>();
        audioSource = GetComponent<AudioSource>();

    }

    private void Update() {
        if(detection.visionArea)
        {
            aiDestination.target = detection.player;
            if(!audioSource.isPlaying && !airAttack.dead)
            {
                audioSource.clip = sound;
                audioSource.Play();
            }
        }
        else
        {
            aiDestination.target = nextPoint;
            if(audioSource.isPlaying && !airAttack.dead)
            {
                audioSource.Stop();
            }
        }
        myTransform.position = new Vector3(myTransform.position.x,myTransform.position.y,0);
    }
}

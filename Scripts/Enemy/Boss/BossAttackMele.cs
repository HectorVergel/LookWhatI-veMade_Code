using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackMele : MonoBehaviour
{
    public int damage;
    public float timeDamage;
    public float range;
    public float FOV;
    public LayerMask WhatIsPlayer;
    public static BossAttackMele instance;
    public BoxCollider2D box;
    AudioSource myAudio;
    public AudioClip attackSound;
    private void Awake() {
        instance = this;
    }

    private void Start()
    {
        myAudio =   GetComponent<AudioSource>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(box.bounds.center, range);

        var direction = Quaternion.AngleAxis(FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(box.bounds.center, direction * range);

        var direction2 = Quaternion.AngleAxis(-FOV / 2, transform.forward) * transform.right;

        Gizmos.DrawRay(box.bounds.center, direction2 * range);
    }
    public void StartAttack()
    {
        StartCoroutine(Attack());
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(timeDamage);
        myAudio.PlayOneShot(attackSound);   
        IsInRange();
    }

    void DealDamage(Collider2D player)
    {
        if(player != null)
        {
            player.GetComponent<HealthSystemPlayer>().TakeDamage(damage);
        }
    }
    public void IsInRange()
    {
        Collider2D player = Physics2D.OverlapCircle(box.bounds.center, range, WhatIsPlayer);
        if (player != null)
        {
            if (IsInAngle(player))
            {
                
                DealDamage(player);
            }
        }
    }
    public bool IsInAngle(Collider2D player)
    {
        float[] angles = GetAngleToPlayer(player);

        foreach (var angle in angles)
        {
           if (FOV >= 2 * angle)
           {
                return true;
           }
        }
        return false;
    }
    private float[] GetAngleToPlayer(Collider2D player)
    {

        Vector2 v1 = transform.right;
        float[] angles = new float[5];

        Vector2[] playerVertex = new Vector2[4];
        playerVertex[0] = (Vector2)player.bounds.center + new Vector2(player.bounds.size.x, -player.bounds.size.y) * 0.5f;
        playerVertex[1] = (Vector2)player.bounds.center + new Vector2(-player.bounds.size.x, -player.bounds.size.y) * 0.5f;
        playerVertex[2] = (Vector2)player.bounds.center + new Vector2(player.bounds.size.x, player.bounds.size.y) * 0.5f;
        playerVertex[3] = (Vector2)player.bounds.center + new Vector2(-player.bounds.size.x, player.bounds.size.y) * 0.5f;
        
        

        Vector2 v2 = playerVertex[0] - (Vector2)box.bounds.center;

        Vector2 v3 = playerVertex[1] - (Vector2)box.bounds.center;

        Vector2 v4 = playerVertex[2] - (Vector2)box.bounds.center;

        Vector2 v5 = playerVertex[3] - (Vector2)box.bounds.center;

        Vector2 v6 = Physics2D.ClosestPoint(box.bounds.center, player) - (Vector2)box.bounds.center;

        angles[0] = Vector2.Angle(v1, v2);
        angles[1] = Vector2.Angle(v1, v3);
        angles[2] = Vector2.Angle(v1, v4);
        angles[3] = Vector2.Angle(v1, v5);
        angles[4] = Vector2.Angle(v1, v6);

        return angles;
    }
}

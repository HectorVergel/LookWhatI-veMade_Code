using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugLightMovement : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float velocity;
    float direction = 1;
    float timer;
    Rigidbody2D rb;
    float maxTime = 1f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeDirection();
        rb.velocity = new Vector2(rb.velocity.x, velocity * direction * Time.deltaTime);

    }


    void ChangeDirection()
    {
        timer += Time.deltaTime;

        if (timer >= maxTime)
        {
            direction = -direction;
            maxTime = Random.Range(maxTime/1.5f, maxTime * 1.5f);
            timer=0f;
        }
    }
}

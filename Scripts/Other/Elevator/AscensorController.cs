using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensorController : MonoBehaviour
{
    public AscensorTrigger trigger;
    public Transform startPosition;
    public Transform desacceleratePosition;
    public Transform finalPosition;
    public float maxVelocity;
    public float acceleration;
    public float desacceleration;
    public bool goDown;
    Rigidbody2D rb;
    public GameObject door;
    DetallesAscensor detalles;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        detalles = GetComponent<DetallesAscensor>();
        goDown = true;
        transform.position = startPosition.position;
    }

    public void StartTravel()
    {
        StartCoroutine(Travel());
    }

    IEnumerator Travel()
    {
        BlockAll(true);
        detalles.StartAllDetails();
        door.SetActive(true);
        goDown = false;
        while (transform.position.y < finalPosition.position.y)
        {
            if(transform.position.y < desacceleratePosition.position.y)
            {
                //acelerando / maxima velocidad
                if(rb.velocity.y < maxVelocity)
                {
                    rb.velocity += new Vector2(0,acceleration*Time.deltaTime);
                }
                else
                {
                    rb.velocity = new Vector2(0,maxVelocity*Time.deltaTime);
                }

                yield return null;
            }
            else
            {
                //desacelerando
                if(rb.velocity.y > 0)
                {
                    rb.velocity -= new Vector2(0,desacceleration*Time.deltaTime);
                }
                else
                {
                    rb.velocity = new Vector2(0,0);
                }
                yield return null;
            }
            
        }
        rb.velocity = new Vector2(0,0);
        transform.position = new Vector3(transform.position.x,finalPosition.position.y,0);
        BlockAll(false);
        detalles.StopAllDetails();
        door.SetActive(false);
        while (!goDown)
        {
            yield return null;
        }
        GoDown();
    }


    void GoDown()
    {
        goDown = true;
        transform.position = new Vector3(transform.position.x,startPosition.position.y,0);
        trigger.enable = true;
        detalles.ResetAllDetails();
    }

    void BlockAll(bool state)
    {
        PlayerJump.instance.bloqued = state;
        Ability1Manager.instance.bloqued = state;
        Ability2Manager.instance.bloqued = state;
        PlayerDash.instance.bloqued = state;
        PlayerRoll.instance.bloqued = state;
    }
}

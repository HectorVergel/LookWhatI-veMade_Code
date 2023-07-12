using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPlatform : MonoBehaviour
{
    BoxCollider2D box;
    float yPlatform;
    BoxCollider2D playerBox;
    bool canGhost;
    private void Start() {
        canGhost = true;
        box = GetComponent<BoxCollider2D>();
        yPlatform = (box.bounds.center + new Vector3(0,box.bounds.extents.y,0)).y;
        GetPlayer();
    }
    void FixedUpdate()
    {
        if(playerBox != null)
        {
            CheckPlayerPosition();
        }
        else
        {
            GetPlayer();
        }
    }
    void CheckPlayerPosition()
    {
        var yPlayer = (playerBox.bounds.center - new Vector3(0,playerBox.bounds.extents.y,0)).y;
        if(yPlayer > yPlatform && canGhost)
        {
            gameObject.layer = 3;
        }
        else
        {
            gameObject.layer = 21;
        }
    }
    void GetPlayer()
    {
        if(FindObjectOfType<HealthSystemPlayer>() != null)
        {
            playerBox = FindObjectOfType<HealthSystemPlayer>().GetComponent<BoxCollider2D>();
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Player")
        PlayerInputs.OnDown += GoDown;
    }
    private void OnCollisionExit2D(Collision2D other) {
        if (other.collider.tag == "Player")
            PlayerInputs.OnDown -= GoDown;
    }

    void GoDown()
    {
        if(FindObjectOfType<TutorialDown>() !=null)
        {
            if(FindObjectOfType<TutorialDown>().triggered)
            {
                FindObjectOfType<TutorialDown>().Done();
            }
        }
        gameObject.layer = 21;
        canGhost = false;
        StartCoroutine(CanGhost());
    }
    
    IEnumerator CanGhost()
    {
        var yPlayer = (playerBox.bounds.center - new Vector3(0,playerBox.bounds.extents.y,0)).y;
        while(yPlayer > yPlatform)
        {
            yPlayer = (playerBox.bounds.center - new Vector3(0,playerBox.bounds.extents.y,0)).y;
            yield return null;
        };
        this.canGhost = true;
       

    }

}

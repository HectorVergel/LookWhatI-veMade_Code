using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour
{
    public float timeToInteract;
    bool interactuable = false;
    public string unlockName;
    public Sprite meleSprite;
    public Sprite wizardSprite;
    public SpriteRenderer icon;
    public GameObject popUp;
    private void Start() {
        if(Character.currentType == CharacterType.Warrior)
        {
            icon.sprite = meleSprite;
        }
        else
        {
            icon.sprite = wizardSprite;
        }
    }
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.collider.tag == "Ground")
        {
            GetComponent<CircleCollider2D>().isTrigger = true;
            gameObject.layer = 13;
            GetComponent<Rigidbody2D>().gravityScale = 0;
            GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
            StartCoroutine(TimeToInteractuable());
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && interactuable)
        {
            GetOrb();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.tag == "Player" && interactuable)
        {
            GetOrb();
            Destroy(this.gameObject);
        }
    }
    private void Update() {
        transform.rotation = new Quaternion(0,0,0,0);
    }

    void GetOrb()
    {
        PlayerPrefs.SetInt(unlockName,1);
        FindObjectOfType<Hud>().ResetAbiltyUI();
        var pop = Instantiate(popUp,transform.position,Quaternion.identity);
        pop.GetComponentInChildren<OrbPopUp>().orbName = unlockName;
    }
    IEnumerator TimeToInteractuable()
    {
        yield return new WaitForSeconds(timeToInteract);
        interactuable = true;
    }
}

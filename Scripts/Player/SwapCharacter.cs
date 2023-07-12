using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapCharacter : MonoBehaviour
{
    public bool goingUp;
    public bool goingDown;
    bool swaping;
    public float alphaSpeed;
    public float portalSpeed;
    public Image image;
    [SerializeField] GameObject warrior;
    [SerializeField] SpriteRenderer[] portal;
    [SerializeField] GameObject portals;
    Vector3 playerPos;
    [SerializeField] GameObject wizard;
    [SerializeField] ParticleSystem particle;
    [SerializeField] ParticleSystem particle1;
 

    [SerializeField] Transform respawnPoint;
    AudioSource audioSource;
    public AudioClip sound;
    public Animator anim;

    private void Start() {
        goingUp = false;
        audioSource = GetComponent<AudioSource>();
        particle.Stop();
        particle1.Stop();
        foreach (var item in portal)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
        }
    }
    public void Swap()
    {
        if (!swaping)
        {
            StartCoroutine(FadeIn());
            swaping = true;
        }
        
    }
    
    void RespawnPlayer()
    {
        Destroy(GameObject.FindWithTag("Player"));
        StartCoroutine(SpawnPlayer());
        
    }

    IEnumerator FadeIn()
    {
        BlockAll(true);
        particle.Play();
        particle1.Play();
        audioSource.pitch = 1f + Random.Range(-0.1f, 0.1f);
        audioSource.PlayOneShot(sound);
        playerPos = PlayerJump.instance.boxCollider2d.bounds.center;
        portals.transform.position = new Vector3(playerPos.x, portals.transform.position.y, portals.transform.position.z);
        anim.SetTrigger("Activate");

        while (portal[0].color.a < 1)
        {
            foreach (var item in portal)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, item.color.a + portalSpeed * Time.deltaTime);
            }
            
            yield return null;
        }
        foreach (var item in portal)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 1);
        }
        
        if (PlayerPrefs.GetInt("Character", 0) == 0)
        {
            PlayerPrefs.SetInt("Character", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Character", 0);
        }
        RespawnPlayer();
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForFixedUpdate();
        BlockAll(true);

        while (portal[0].color.a > 0)
        {
            foreach (var item in portal)
            {
                item.color = new Color(item.color.r, item.color.g, item.color.b, item.color.a - portalSpeed * Time.deltaTime);
            }
           
            yield return null;
        }
        foreach (var item in portal)
        {
            item.color = new Color(item.color.r, item.color.g, item.color.b, 0);
        }
        particle.Stop();
        particle1.Stop();
        BlockAll(false);
        swaping = false;
    }

    void BlockAll(bool state)
    {
        PlayerMovement.instance.bloqued = state;
        PlayerMovement.instance.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0,0);
        PlayerJump.instance.bloqued = state;
        Ability1Manager.instance.bloqued = state;
        Ability2Manager.instance.bloqued = state;
        PlayerDash.instance.bloqued = state;
        PlayerRoll.instance.bloqued = state;
    }
    void CreatePlayer()
    {
        
        if(PlayerPrefs.GetInt("Character",0) == 0)
        {
            var go = Instantiate(warrior,respawnPoint.position, Quaternion.identity);
            go.transform.position = playerPos;
            Character.currentType = CharacterType.Warrior;
        }
        else
        {
            var go = Instantiate(wizard,respawnPoint.position, Quaternion.identity);
            go.transform.position = playerPos;
            Character.currentType = CharacterType.Wizard;
        }
        Hud.instance.ResetAbiltyUI();
        if(TutorialInfo.instance != null)
        {
            TutorialInfo.instance.ResetTutorial();
        }
        StartCoroutine(FadeOut());
    }

    IEnumerator SpawnPlayer()
    {
        yield return new WaitForFixedUpdate();
        CreatePlayer();
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract += Swap;
            StopCoroutine(Down());
            StartCoroutine(Up());
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player")
        {
            PlayerInputs.OnInteract -= Swap;
            StartCoroutine(Down());
        } 
    }
    IEnumerator Up()
    {
        while (goingDown)
        {
            yield return null;
        }
        goingUp = true;

        while (image.color.a < 1)
        {
            image.color = new Color(1,1,1,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,1);
        goingUp = false;

    }

    IEnumerator Down()
    {
        while (goingUp)
        {
            yield return null;
        }
        goingDown = true;

        while (image.color.a > 0)
        {
            image.color = new Color(1,1,1,image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,0);
        goingDown = false;
    }
}

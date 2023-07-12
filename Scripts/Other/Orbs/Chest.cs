using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chest : MonoBehaviour
{
    public GameObject orb;
    public Transform orbSpawnPosition;
    public float orbForce;
    public Animator anim;
    bool goingUp = false;
    public float alphaSpeed;
    public float timeToDropOrb;
    public Image image;
    bool opened;
    [SerializeField] ParticleSystem vfx1;
    [SerializeField] ParticleSystem vfx2;
    [SerializeField] ParticleSystem vfx3;
    AudioSource audioSource;
    public AudioClip sound;
    public GameObject[] platforms;
    public bool inversePlatform = false;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        goingUp = false;
        vfx1.Stop();
        vfx2.Stop();
        vfx3.Stop();
        if (PlayerPrefs.GetInt(orb.GetComponent<Orb>().unlockName, 0) == 1)
        {
            if(inversePlatform)
            {
                ClosePlatforms();
            }
            else
            {
                OpenPlatforms();
            }
            Destroy(this.gameObject);
        }
        else
        {
            if(inversePlatform)
            {
                OpenPlatforms();
            }
            else
            {
                ClosePlatforms();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !opened)
        {
            PlayerInputs.OnInteract += OpenChest;
            StopAllCoroutines();
            StartCoroutine(Up());
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if(other.tag == "Player" && !opened)
        {
            PlayerInputs.OnInteract -= OpenChest;
            StartCoroutine(Down());
        } 
    }

    void OpenPlatforms()
    {
        foreach (var item in platforms)
        {
            item.SetActive(true);
        }
    }

    void ClosePlatforms()
    {
        foreach (var item in platforms)
        {
            item.SetActive(false);
        }
    }
    IEnumerator Up()
    {
        goingUp = true;
        while(image.color.a < 1)
        {
            image.color = new Color(1,1,1,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,1);
        goingUp = false;

    }

    IEnumerator Down()
    {
        while(goingUp)
        {
            yield return null;
        }
        while(image.color.a > 0)
        {
            image.color = new Color(1,1,1,image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,0);
    }

    void OpenChest()
    {
        if(GetComponent<AchievementPopUp>() != null)
        {
            GetComponent<AchievementPopUp>().Activate();
        }
        if(inversePlatform)
        {
            ClosePlatforms();
        }
        else
        {
            OpenPlatforms();
        }
        audioSource.PlayOneShot(sound);
        vfx1.Play();
        vfx2.Play();
        vfx3.Play();
        anim.SetTrigger("Open");
        PlayerInputs.OnInteract -= OpenChest;
        goingUp = false;
        opened = true;
        StopAllCoroutines();
        StartCoroutine(Down());
        StartCoroutine(DropOrb());
    }
    IEnumerator DropOrb()
    {
        yield return new WaitForSeconds(timeToDropOrb);
        //drop orb
        float yForce = Random.Range(1.5f,2f);
        float xForce = Random.Range(-1f,1f);
        var ob = Instantiate(orb,orbSpawnPosition.position,Quaternion.identity);
        ob.GetComponent<Rigidbody2D>().AddForce(new Vector2(xForce,yForce) * orbForce);
    }
}

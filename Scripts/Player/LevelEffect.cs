using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LevelEffect : MonoBehaviour
{


    public static LevelEffect instance;

    [SerializeField] GameObject levelVFX;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    [SerializeField] TextMeshProUGUI textLabel;
    [SerializeField] GameObject transformParent;

    [SerializeField] float textVelocity;
    [SerializeField] float textStayTime;
    public AudioSource audioSource;
    public AudioClip sound;



    private void Start()
    {
        FillLists();
        StopVFX();
        textLabel.gameObject.SetActive(false);
    }
    private void Awake()
    {
        instance = this;
    }
    private void OnEnable()
    {
        Experience.OnLevelUp += PlayVFX;
    }
    private void OnDisable()
    {
        Experience.OnLevelUp -= PlayVFX;
    }

    public void PlayVFX()
    {
        audioSource.PlayOneShot(sound);
        AddText();
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }
    public void StopVFX()
    {
        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

    void FillLists()
    {
        for (int i = 0; i < levelVFX.transform.childCount; i++)
        {
            var ps = levelVFX.transform.GetChild(i).GetComponent<ParticleSystem>();

            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }

    private void Update() 
    {
        if(transform.rotation.y == 1)
        {
            transformParent.transform.localScale = new Vector3(-1,1,1);
        }
        else
        {
            transformParent.transform.localScale = new Vector3(1,1,1);
        }
        
    }
    public void AddText()
    {
        StopAllCoroutines();
        StartCoroutine(AddTextCoroutine());
    }

    IEnumerator AddTextCoroutine()
    {
        textLabel.gameObject.SetActive(true);
        textLabel.color = new Color(0, 1, 0, 0);
        //fade in
        while (textLabel.color.a < 1)
        {
            Color col = new Color(0, 1, 0, textLabel.color.a + textVelocity * Time.deltaTime);
            textLabel.color = col;
            yield return null;
        }
        textLabel.color = new Color(0, 1, 0, 1);
        //stay
        yield return new WaitForSeconds(textStayTime);
        //fade out
        while (textLabel.color.a > 0)
        {
            Color col = new Color(0, 1, 0, textLabel.color.a - textVelocity * Time.deltaTime);
            textLabel.color = col;
            yield return null;
        }
        textLabel.color = new Color(0, 1, 0, 0);
        textLabel.gameObject.SetActive(false);


    }

    

}

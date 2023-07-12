using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuCredits : MonoBehaviour
{
    public GameObject principalMenu;
    public GameObject credits;
    public RectTransform creditsTransform;
    public RectTransform startPosition;
    public float velocity;
    public float velocityPlus;
    public float maxHeight;
    float speed;
    public bool creditsBoss = false;
    AudioSource music;
    public AudioClip musicSound;
    AudioSource audioSource;
    private void OnEnable() {
        if(!creditsBoss)
        {
            music = FindObjectOfType<MusicController>().GetComponent<AudioSource>();
            music.Pause();
        }
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicSound;
        audioSource.Play();
        
        if(creditsBoss)
        {
            Time.timeScale = 1;
        }
        credits.transform.position = new Vector3(creditsTransform.position.x,startPosition.position.y,creditsTransform.position.z);
        PlayerInputs.OnBack += ReturnPrincipal;
        PlayerInputs.OnSpaceBar += SpeedUp;
        PlayerInputs.OnSpaceBarEnd += NormalSpeed;
        speed = velocity;
    }
    void ReturnPrincipal()
    {
        if(creditsBoss)
        {
            ResetGame();
            SceneManager.LoadScene("Menu");
        }
        else
        {
            principalMenu.GetComponent<MenuPrincipal>().firstButton = principalMenu.GetComponent<MenuPrincipal>().creditsButton;
            principalMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    private void OnDisable() {
        PlayerInputs.OnBack -= ReturnPrincipal;
        PlayerInputs.OnSpaceBar -= SpeedUp;
        PlayerInputs.OnSpaceBarEnd -= NormalSpeed;
        if(!creditsBoss)
        {
            music.GetComponent<MusicController>().Start();
        }
        audioSource.Stop();
    }
    void Update()
    {
        if(creditsTransform.position.y < maxHeight)
        {
            creditsTransform.Translate(0,speed*Time.deltaTime,0);
        }
        else
        {
            ReturnPrincipal();
        }
    }

    void SpeedUp()
    {
        speed = velocityPlus;
    }
    void NormalSpeed()
    {
        speed = velocity;
    }

    void ResetGame()
    {
        int full = PlayerPrefs.GetInt("FullScreen",1);
        int vsync = PlayerPrefs.GetInt("VSync",1);
        int shake = PlayerPrefs.GetInt("Shake",1);
        int subt = PlayerPrefs.GetInt("Subtitles",1);
        int curs = PlayerPrefs.GetInt("CursorLock",1);
        int healthbar = PlayerPrefs.GetInt("HealthBars",1);
        int res = PlayerPrefs.GetInt("resolutionOption",PlayerPrefs.GetInt("DefaultResolution"));

        float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float voice = PlayerPrefs.GetFloat("VoiceVolume", 1);

        int [] ach = new int[11];
        for (int i = 1; i <= 10; i++)
        {
            ach[i] = PlayerPrefs.GetInt("Ach" + i, 0);
        }


        PlayerPrefs.DeleteAll();

        //restablecerlos a lo que valian

        PlayerPrefs.SetInt("FullScreen",full);
        PlayerPrefs.SetInt("VSync",vsync);
        PlayerPrefs.SetInt("Shake",shake);
        PlayerPrefs.SetInt("Subtitles",subt);
        PlayerPrefs.SetInt("CursorLock",curs);
        PlayerPrefs.SetInt("HealthBars",healthbar);
        PlayerPrefs.SetInt("resolutionOption",res);

        PlayerPrefs.SetFloat("MasterVolume", master);
        PlayerPrefs.SetFloat("MusicVolume", music);
        PlayerPrefs.SetFloat("SFXVolume", sfx);
        PlayerPrefs.SetFloat("VoiceVolume", voice);

        for (int i = 1; i <= 10; i++)
        {
            PlayerPrefs.SetInt("Ach" + i, ach[i]);
        }
    }
}

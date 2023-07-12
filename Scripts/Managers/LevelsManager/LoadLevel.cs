using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] Animator transition;
    public static LoadLevel instance;
    public bool dead;
    int side;
    string scene;
    AudioSource audioSource;
    public AudioClip deadSound;
    public AudioClip normalSound;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        dead = false;
    }
    private void Awake()
    {
        instance = this;
    }
    IEnumerator StopTime()
    {
        Time.timeScale = 0.25f;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        InterfaceManager.instance.interfaceActive = true;
        if (dead)
        {
            yield return new WaitForSeconds(0.12f);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }
        
        Time.timeScale = 0;
    }
    public void LoadScene()
    {
        PlayerPrefs.SetInt("RespawnSide", side);
        SceneManager.LoadScene(scene);
    }

    public void Respawn()
    {
        audioSource.clip = deadSound;
        audioSource.Play();
        Transition.instance.ChooseDead();
        StartCoroutine(StopTime());
        dead = true;
        transition.Play("end_transition");
    }
    public void RespawnDie()
    {
        
        switch (PlayerPrefs.GetInt("CheckPoint", 0))
        {
            case 1:
                SceneManager.LoadScene("Lobby");
                break;

            case 2:
                SceneManager.LoadScene("Lobby");
                break;

            case 3:
                SceneManager.LoadScene("Vertical");
                break;

            case 4:
                SceneManager.LoadScene("Minas");
                break;

            case 5:
                SceneManager.LoadScene("Surface");
                break;

            case 6:
                SceneManager.LoadScene("Surface");
                break;

            default:
                SceneManager.LoadScene("Tutorial");
                break;
        }
    }

    public void DeleteEnemies()
    {
        MusicController[] music = FindObjectsOfType<MusicController>();
        foreach (var item in music)
        {
            item.GetComponent<AudioSource>().Stop();
        }
        MusicBossController[] musicBoss = FindObjectsOfType<MusicBossController>();
        foreach (var item in musicBoss)
        {
            item.GetComponent<AudioSource>().Stop();
        }

        DialogueManager[] dialogs = FindObjectsOfType<DialogueManager>();
        foreach (var item in dialogs)
        {
            item.GetComponent<AudioSource>().Stop();
        }

        var enemies = FindObjectsOfType<IEnemy>();

        foreach (var enemy in enemies)
        {
            if(enemy != null)
            {
                Destroy(enemy.gameObject,0.2f);
            }
            
        }
    }
    public void NextScene(string scene, int side)
    {
        DeleteEnemies();
        audioSource.clip = normalSound;
        audioSource.Play();
        StartCoroutine(StopTime());
        Transition.instance.ChooseTransition();
        this.scene = scene;
        this.side = side;
        dead = false;
        transition.Play("end_transition");
    }
}

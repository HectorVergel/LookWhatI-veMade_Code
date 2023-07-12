using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BossClone : MonoBehaviour
{
    public AudioClip dieSound;
    public GameObject back;
    public float timeToGoCredits;
    public int dialogNumber;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<MusicBossController>().GetComponent<AudioSource>().Stop();
        GetComponent<AudioSource>().clip = dieSound;
        GetComponent<AudioSource>().Play();
        StartCoroutine(LevelPassed());
    }

    IEnumerator LevelPassed()
    {
        FindObjectOfType<SpearSpawner>().DesactivateSpears();
        while (CheckIfSpearsFalling() && CheckIfWaves() && CheckIfCages() && PlayerAlive())
        {
           yield return null; 
        }
        //confirmamos que nos hemos pasado el boss/el juego
        StartCoroutine(GoToCredits());
    }
    IEnumerator GoToCredits()
    {
        yield return new WaitForSeconds(0.1f);
        if(PlayerPrefs.GetInt("Died",0) == 0)
        {
            if(GetComponent<AchievementPopUp>() != null)
            {
                GetComponent<AchievementPopUp>().Activate();
            }
        }
        yield return new WaitForSeconds(timeToGoCredits);
        SceneManager.LoadScene("CinematicaBoss");
    }
    bool PlayerAlive()
    {
        if(FindObjectOfType<HealthSystemPlayer>() != null)
        {
            return !FindObjectOfType<HealthSystemPlayer>().isAlive;
        }
        else
        {
            return true;
        }
    }

    bool CheckIfSpearsFalling()
    {
        Spear [] spears = FindObjectsOfType<Spear>();
        foreach (var spear in spears)
        {
            if(spear.falling)
            {
                return true;
            }
        }
        return false;
    }
    bool CheckIfWaves()
    {
        Wave [] waves = FindObjectsOfType<Wave>();
        if(waves.Length >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckIfCages()
    {
        Cage [] cages = FindObjectsOfType<Cage>();
        if(cages.Length >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

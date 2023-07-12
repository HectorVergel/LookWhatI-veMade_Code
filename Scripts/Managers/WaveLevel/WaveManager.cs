using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject wave1;
    public GameObject wave2;
    public GameObject wave3;
    public GameObject wave4;
    public GameObject chest;
    public GameObject platform;
    public GameObject dialogueStart;
    public GameObject dialogueEnd;

    public float time1;
    public float time2;
    public float time3;
    public float time4;
    public float timeToStartFalling;
    int currentWave;
    bool triggered;
    bool cameraShake;
    public bool killedLastOne;
    
    private void Start() {
        killedLastOne = false;
        triggered = false;
        cameraShake = false;
    }
    IEnumerator FirstWave()
    {
        platform.SetActive(false);
        if(dialogueStart != null)
        {
            yield return new WaitForSeconds(time1);
        }
        currentWave = 1;
        CallNextWave(currentWave);
    }

    IEnumerator NextWave(GameObject enemies,float cooldown)
    {
        yield return new WaitForSeconds(timeToStartFalling);
        enemies.SetActive(true);
        float time = 0;
        while(time<cooldown && FindObjectOfType<HealthSystemEnemy>() != null)
        {
            time += Time.deltaTime;
            yield return null;
        }
        currentWave++;
        CallNextWave(currentWave);
    }

    IEnumerator LastWave()
    {
        while(!killedLastOne)
        {
            yield return null;
        }
        LevelFinished();
    }

    void LevelFinished()
    {
        if(dialogueEnd != null)
        {
            dialogueEnd.GetComponent<DialogueManager>().StartDialogue();
        }
        chest.SetActive(true);
        platform.SetActive(true);
        
    }

    void CallNextWave(int numberOfWave)
    {
        switch (numberOfWave)
        {
            case 1:
            StartCoroutine(NextWave(wave1,time2));
            break;

            case 2:
            StartCoroutine(NextWave(wave2,time3));
            break;

            case 3:
            StartCoroutine(NextWave(wave3,time4));
            break;

            case 4:
            StartCoroutine(NextWave(wave4,0));
            break;

            default:
            StartCoroutine(LastWave());
            break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !triggered)
        {
            triggered = true;
            StartCoroutine(FirstWave());
        }
        if(other.gameObject.layer == 6 && !cameraShake)
        {
            StartCoroutine(ResetCameraShake());
            Shake.instance.StartCoroutine(Shake.instance.shake());
        }
    }
    IEnumerator ResetCameraShake()
    {
        cameraShake = true;
        yield return new WaitForSeconds(1);
        cameraShake = false;
    }
}

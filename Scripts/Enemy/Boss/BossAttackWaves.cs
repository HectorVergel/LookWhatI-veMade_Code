using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackWaves : MonoBehaviour
{
    public float waveCooldownNormal;
    public float waveCooldownRage;
    public int numberOfWavesNormal;
    public int numberOfWavesRage;
    public float timeToStartNormal;
    public float timeToStartRage;
    public GameObject wavePrefab;
    public Transform waveSpawnR;
    public Transform waveSpawnL;
    public ParticleSystem vfx1;
    public ParticleSystem vfx2;
    float cooldownWave;
    int currentWave;
    float timeToStart;
    public static BossAttackWaves instance;
    AudioSource myAudio;
    public AudioClip attackSound;
    private void Awake() {
        instance = this;
    }
    private void Start()
    {
        vfx1.Stop();
        vfx2.Stop();
        myAudio = GetComponent<AudioSource>();
    }
    public void StartAttack(bool rage)
    {
        if(rage)
        {
            cooldownWave = waveCooldownRage;
            currentWave = numberOfWavesRage;
            timeToStart = timeToStartRage;
        }
        else
        {
            cooldownWave = waveCooldownNormal;
            currentWave = numberOfWavesNormal;
            timeToStart = timeToStartNormal;
        }
        StartCoroutine(StartWaves());
    }
    IEnumerator StartWaves()
    {
        float time = 0;
        while(time < timeToStart)
        {
            time += Time.deltaTime;
            yield return null;
        }
        SpawnWave();
        StartCoroutine(CreateWave());
    }

    IEnumerator CreateWave()
    {
        float time = 0;
        while (time < cooldownWave)
        {
            time += Time.deltaTime;
            yield return null;
        }
        SpawnWave();
        if(currentWave > 0)
        {
            StartCoroutine(CreateWave());
        }
    }

    void SpawnWave()
    {
        vfx1.Play();
        vfx2.Play();
        myAudio.PlayOneShot(attackSound);
        currentWave--;
        var a = Instantiate(wavePrefab,waveSpawnR.position,Quaternion.identity);
        a.GetComponent<Wave>().direction = 1;
        var b = Instantiate(wavePrefab,waveSpawnL.position,Quaternion.identity);
        b.GetComponent<Wave>().direction = - 1;
    }
}

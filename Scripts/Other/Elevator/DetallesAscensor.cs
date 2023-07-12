using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetallesAscensor : MonoBehaviour
{
    public GameObject puerta;
    public GameObject arrow;
    public float tiempoCerrarPuerta;
    AudioSource audioSource;
    float maxY;
    float minY;
    AscensorController ascensor;
    public AudioClip sound;
    public float volumeSpeed;
    private void Start() {
        ascensor = GetComponent<AscensorController>();
        maxY = ascensor.finalPosition.transform.position.y;
        minY = ascensor.startPosition.transform.position.y;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = sound;
        ResetAllDetails();
    }

    public void StartAllDetails()
    {
        StartCoroutine(MoveDoorUp());
        StartCoroutine(MoveArrow());
        StartCoroutine(FadeInMusic());
    }
    public void StopAllDetails()
    {
        StartCoroutine(MoveDoorDown());
        StartCoroutine(FadeOutMusic());
    }
    public void ResetAllDetails()
    {
        StopAllCoroutines();
        puerta.transform.eulerAngles = new Vector3(0,0,-90);
        arrow.transform.eulerAngles = new Vector3(0,0,90);
        audioSource.volume = 0;
        audioSource.Stop();
    }
    IEnumerator MoveDoorUp()
    {
        float timer = 0;
        puerta.transform.eulerAngles = new Vector3(0,0,-90);
        while (puerta.transform.eulerAngles.z >= 0)
        {
            timer+=Time.fixedDeltaTime;
            puerta.transform.eulerAngles = new Vector3(0,0,Mathf.Lerp(-90,0,timer/tiempoCerrarPuerta));
            yield return null;
        }
        puerta.transform.eulerAngles = new Vector3(0,0,0);
    }

    IEnumerator MoveDoorDown()
    {
        float timer = 0;
        puerta.transform.eulerAngles = new Vector3(0,0,0);
        while (puerta.transform.eulerAngles.z > -90)
        {
            timer+=Time.fixedDeltaTime;
            puerta.transform.eulerAngles = new Vector3(0,0,Mathf.Lerp(0,-90,timer/tiempoCerrarPuerta));
            yield return null;
        }
        puerta.transform.eulerAngles = new Vector3(0,0,-90);
    }
    IEnumerator MoveArrow()
    {
        arrow.transform.eulerAngles = new Vector3(0,0,90);
        float distance = maxY-minY;
        while(arrow.transform.eulerAngles.z > -90)
        {
            arrow.transform.eulerAngles = new Vector3(0,0,Mathf.Lerp(90,-90,(transform.position.y - minY)/(distance)));
            yield return null;
        }
        arrow.transform.eulerAngles = new Vector3(0,0,-90);
    }

    IEnumerator FadeInMusic()
    {
        audioSource.volume = 0;
        audioSource.Play();
        while(audioSource.volume < 1)
        {
            audioSource.volume+=volumeSpeed*Time.fixedDeltaTime*2;
            yield return null;
        }
        audioSource.volume = 1;
    }

    IEnumerator FadeOutMusic()
    {
        while(audioSource.volume > 0)
        {
            audioSource.volume-=volumeSpeed*Time.fixedDeltaTime;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Stop();
    }

}

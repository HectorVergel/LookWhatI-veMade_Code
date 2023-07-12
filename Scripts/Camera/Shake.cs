using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shake : MonoBehaviour
{
    [SerializeField] float shakeDuration;
    [SerializeField] float shakeMagnitude;

    public static Shake instance;
    void Awake()
    {
        instance = this;
    }
    public IEnumerator shake()
    {
        if(PlayerPrefs.GetInt("Shake",1) == 0)
        {
            StopShake();
        }
        Vector3 originalPos = transform.localPosition;

        float timer = 0.0f;
        while (timer  < shakeDuration)    
        {
            float x = Random.Range(-0.5f, 0.5f) * shakeMagnitude;
            float y = Random.Range(-0.5f, 0.5f) * shakeMagnitude;

            transform.localPosition = new Vector3(transform.localPosition.x + x,transform.localPosition.y + y, originalPos.z);

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = originalPos;  
    }

    public void StopShake()
    {
        StopAllCoroutines();
    }

}

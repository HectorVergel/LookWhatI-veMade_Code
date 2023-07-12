
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour
{
    // Start is called before the first frame update
    public float secondsToDestroy;
    public float seconds = 0;
    private void Start()
    {
        Destroy(gameObject,secondsToDestroy);
    }

    private void Update()
    {
        seconds += Time.deltaTime;
    }
}

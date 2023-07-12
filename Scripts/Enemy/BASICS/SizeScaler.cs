using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeScaler : MonoBehaviour
{
    [SerializeField] float maxSize;
    [SerializeField] float minSize;


    float randomSize;
    void Start()
    {
        randomSize = GetRandomSize();
        transform.localScale = new Vector3(randomSize, randomSize, randomSize);
    }

    private float GetRandomSize()
    {
        return Random.Range(minSize, maxSize);
    }
}

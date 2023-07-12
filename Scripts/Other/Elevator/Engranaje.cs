using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engranaje : MonoBehaviour
{
    public float speed;
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0,0,speed*Time.fixedDeltaTime),Space.Self);
    }
}

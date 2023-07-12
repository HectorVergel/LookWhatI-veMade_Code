using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensorOut : MonoBehaviour
{
    public AscensorController ascensor;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && !ascensor.goDown)
        {
            ascensor.goDown = true;
        }
    }
}

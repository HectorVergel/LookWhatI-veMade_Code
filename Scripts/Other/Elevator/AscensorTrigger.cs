using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AscensorTrigger : MonoBehaviour
{
    public AscensorController ascensor;
    public bool enable;
    private void Start() {
        enable = true;
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player" && enable)
        {
            enable = false;
            ascensor.StartTravel();
        }
    }
}

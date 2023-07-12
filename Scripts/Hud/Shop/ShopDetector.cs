using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopDetector : MonoBehaviour
{
    public int shopNumber;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            PlayerPrefs.SetInt("ShopNumber",shopNumber);
        }
    }
}

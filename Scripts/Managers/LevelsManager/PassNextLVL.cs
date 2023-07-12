using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PassNextLVL : MonoBehaviour
{
    public string nextSceneName;
    public int sideWhereRespawn;

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag =="Player")
        {
            LoadLevel.instance.NextScene(nextSceneName, sideWhereRespawn);
        }
    }
}

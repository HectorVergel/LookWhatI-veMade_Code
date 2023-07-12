using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSetter : MonoBehaviour
{
    private void Start() {
        PlayerPrefs.SetInt(SceneManager.GetActiveScene().name,1);
    }
}

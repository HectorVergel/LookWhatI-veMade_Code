using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Cinematics : MonoBehaviour
{
    public string nextSceneName;
    private void OnEnable() {
        PlayerInputs.OnPause += SkipCinematic;
    }
    private void OnDisable() {
        PlayerInputs.OnPause -= SkipCinematic;
    }

    void SkipCinematic()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    private void OnEnable() {
        Time.timeScale = 1f;
        PlayerInputs.OnBack += SkipSplash;
        PlayerInputs.OnInventory += SkipSplash;
        PlayerInputs.OnPause += SkipSplash;
        PlayerInputs.OnConfirm += SkipSplash;
    }
    private void OnDisable() {
        PlayerInputs.OnBack -= SkipSplash;
        PlayerInputs.OnInventory -= SkipSplash;
        PlayerInputs.OnPause -= SkipSplash;
        PlayerInputs.OnConfirm -= SkipSplash;
    }

    void SkipSplash()
    {
        SceneManager.LoadScene("Menu");
    }
}

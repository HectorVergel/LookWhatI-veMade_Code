using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuControls : MonoBehaviour
{
    public GameObject pauseMenu;
    private void OnEnable() {
        PlayerInputs.OnBack += ReturnPause;
        PlayerInputs.OnPause += pauseMenu.GetComponent<PauseMenu>().ResumeGame;
        if(GetComponent<AchievementPopUp>() != null)
        {
            GetComponent<AchievementPopUp>().Activate();
        }
    }
    private void OnDisable() {
        PlayerInputs.OnBack -= ReturnPause;
        PlayerInputs.OnPause -= pauseMenu.GetComponent<PauseMenu>().ResumeGame;
    }

    void ReturnPause()
    {
        pauseMenu.GetComponent<PauseMenu>().firstButton = pauseMenu.GetComponent<PauseMenu>().controlsButton;
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

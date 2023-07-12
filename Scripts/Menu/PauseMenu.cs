using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject resumeButton;
    public GameObject optionsButton;
    public GameObject controlsButton;
    public GameObject optionsMenu;
    public GameObject controlsMenu;
    public GameObject panel;

    private void OnEnable() 
    {
        Cursor.lockState = CursorLockMode.None;
        Shake.instance.StopShake();
        PlayerInputs.OnBack += ResumeGame;
        Time.timeScale = 0f;
        InterfaceManager.instance.interfaceActive = true;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        PlayerInputs.OnPause += ResumeGame;
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            firstButton.GetComponent<Button>().Select();
            firstButton.GetComponent<Button>().OnSelect(null);
        }
        panel.SetActive(true);
    }
    private void OnDisable() 
    {
        firstButton = resumeButton;
        PlayerInputs.OnPause -= ResumeGame;
        PlayerInputs.OnBack -= ResumeGame;
    }
    public void ResumeGame()
    {
        if(PlayerPrefs.GetInt("CursorLock",1) == 1)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
        Time.timeScale = 1f;
        InterfaceManager.instance.interfaceActive = false;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        panel.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        gameObject.SetActive(false);
    }

    public void OpenOptions()
    {
        optionsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void OpenControls()
    {
        controlsMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}


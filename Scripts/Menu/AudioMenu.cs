using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;
using UnityEngine.InputSystem;

public class AudioMenu : MonoBehaviour
{
    public OptionsAudio audioManager;
    public GameObject displayMenu;
    public GameObject firstButton;
    public MenuOptions options;
    public RectTransform boton;
    public TextMeshProUGUI title;
    public Color butonSelectedColor;
    public float alpha;
    public float bigScale;
    public float initScale;
    private void OnEnable() 
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            if(Time.timeScale == 0)
            {
                if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
                {
                    firstButton.GetComponent<Slider>().Select();
                    firstButton.GetComponent<Slider>().OnSelect(null);
                }
            }
            else
            {
                StartCoroutine(HighlightFirstButton(firstButton));
            }
            options.SetDescription(firstButton.GetComponent<StopPlayingButtons>().description);
        }
        else
        {
            options.SetDescription("");
        }
        
        PlayerInputs.OnRightTrigger += ChangeOptionsMenu;
        title.color = butonSelectedColor;
        boton.localScale = new Vector3(initScale,initScale,initScale);
    }
    private void OnDisable() 
    {
        PlayerInputs.OnRightTrigger -= ChangeOptionsMenu;
        title.color = new Color(1,1,1,alpha);
        boton.localScale = new Vector3(bigScale,bigScale,bigScale);
    }

    IEnumerator HighlightFirstButton(GameObject button)
    {
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForFixedUpdate();
            EventSystem.current.SetSelectedGameObject(button);
        }
    }

    public void ChangeOptionsMenu()
    {
        audioManager.StopSounds();
        displayMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

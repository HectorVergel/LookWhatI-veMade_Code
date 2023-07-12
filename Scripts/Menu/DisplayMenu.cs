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

public class DisplayMenu : MonoBehaviour
{
    public GameObject audioMenu;
    public MenuOptions options;
    public GameObject firstButton;
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
                    firstButton.GetComponent<TMP_Dropdown>().Select();
                    firstButton.GetComponent<TMP_Dropdown>().OnSelect(null);
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
        
        PlayerInputs.OnLeftTrigger += ChangeOptionsMenu;
        title.color = butonSelectedColor;
        boton.localScale = new Vector3(initScale,initScale,initScale);
    }
    private void OnDisable()
    {
        PlayerInputs.OnLeftTrigger -= ChangeOptionsMenu;
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
        audioMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

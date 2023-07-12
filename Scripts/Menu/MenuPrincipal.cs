using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject firstButton;
    public GameObject playButton;
    public GameObject optionsButton;
    public GameObject achievementsButton;
    public GameObject creditsButton;
    public GameObject exitButton;
    public GameObject questionExit;
    public GameObject optionsMenu;
    public GameObject achievementsMenu;
    public GameObject creditsMenu;
    public GameObject yesExit;
    public float velocityFade;
    public TextMeshProUGUI [] texts;
    public Image [] images;
    public CameraMenu cam;
    public bool goingDown = false;
    private void OnEnable() 
    {
        goingDown = true;
        StartCoroutine(Appear());
        StartCoroutine(HighlightFirstButton(firstButton));
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 1f;
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
    IEnumerator Disapear()
    {
        while(texts[0].color.a > 0)
        {
            foreach (Image im in images)
            {
                RemoveAlphaImage(im,-velocityFade);
            }
            foreach (TextMeshProUGUI tex in texts)
            {
                RemoveAlphaText(tex,-velocityFade);
            }
            yield return null;
        }

        foreach (Image im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b,0);
        }
        foreach (TextMeshProUGUI tex in texts)
        {
            tex.color = new Color(tex.color.r,tex.color.g,tex.color.b,0);
        }
        goingDown = false;
        gameObject.SetActive(false);

    }

    IEnumerator Appear()
    {
        foreach (Image im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b,0);
        }
        foreach (TextMeshProUGUI tex in texts)
        {
            tex.color = new Color(tex.color.r,tex.color.g,tex.color.b,0);
        }
        while(texts[0].color.a < 1)
        {
            foreach (Image im in images)
            {
                RemoveAlphaImage(im,velocityFade);
            }
            foreach (TextMeshProUGUI tex in texts)
            {
                RemoveAlphaText(tex,velocityFade);
            }
            yield return null;
        }

        foreach (Image im in images)
        {
            im.color = new Color(im.color.r,im.color.g,im.color.b,1);
        }
        foreach (TextMeshProUGUI tex in texts)
        {
            tex.color = new Color(tex.color.r,tex.color.g,tex.color.b,1);
        }
        goingDown = false;
    }
    void RemoveAlphaText(TextMeshProUGUI text, float vel)
    {
        text.color = new Color(text.color.r,text.color.g,text.color.b,text.color.a + vel * Time.deltaTime);
    }
    void RemoveAlphaImage(Image image, float vel)
    {
        image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a + vel * Time.deltaTime);
    }

    public void OpenPlayMenu()
    {
        if(!goingDown)
        {
            goingDown = true;
            cam.GoDown();
            StartCoroutine(Disapear());
        }
    }

    public void OpenOptionsMenu()
    {
        if(!goingDown)
        {
            optionsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }
    public void OpenCreditsMenu()
    {
        if(!goingDown)
        {
            creditsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OpenAchievementsMenu()
    {
        if(!goingDown)
        {
            achievementsMenu.SetActive(true);
            gameObject.SetActive(false);
        }
    }

    public void OpenQuestion()
    {
        if(!goingDown)
        {
            exitButton.SetActive(false);
            questionExit.SetActive(true);
            StartCoroutine(HighlightFirstButton(yesExit));
            PlayerInputs.OnBack += ExitQuestion;
        }
    }

    public void ExitQuestion()
    {
        if(!goingDown)
        {
            exitButton.SetActive(true);
            questionExit.SetActive(false);
            StartCoroutine(HighlightFirstButton(exitButton));
            PlayerInputs.OnBack -= ExitQuestion;
        }
    }

    public void ExitGame()
    {
        if(!goingDown)
        {
            Application. Quit();
        }
    }

    public void ResetGameStats()
    {
        PlayerPrefs.DeleteAll();
    }
}

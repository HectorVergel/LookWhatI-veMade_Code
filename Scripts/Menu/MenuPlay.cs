using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;
public class MenuPlay : MonoBehaviour
{
    public GameObject continueButton;
    public GameObject newGameButton;
    public GameObject newGameZenButton;
    public GameObject principalMenu;
    public GameObject arrayCards;
    public GameObject question;
    public GameObject zenQuestion;
    public GameObject noButton;
    public GameObject noZenButton;
    public CameraMenu cam;
    public float velocityFade;
    public TextMeshProUGUI [] texts;
    public Image [] images;
    public bool goingDown = false;
    private void OnEnable() 
    {
        goingDown = true;
        StartCoroutine(Appear());
        PlayerInputs.OnBack += ReturnPrincipal;
        if(PlayerPrefs.GetInt("CheckPoint",0) == 0)
        {
            continueButton.SetActive(false);
            StartCoroutine(HighlightFirstButton(newGameButton));
        }
        else
        {
            continueButton.SetActive(true);
            StartCoroutine(HighlightFirstButton(continueButton));
        }
    }
    private void OnDisable() 
    {
        PlayerInputs.OnBack -= ReturnPrincipal;
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
        while(newGameButton.GetComponent<Image>().color.a < 1)
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

    IEnumerator Disapear()
    {
        while(newGameButton.GetComponent<Image>().color.a > 0)
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
        principalMenu.GetComponent<MenuPrincipal>().firstButton = principalMenu.GetComponent<MenuPrincipal>().playButton;
        gameObject.SetActive(false);
    }
    void RemoveAlphaText(TextMeshProUGUI text, float vel)
    {
        text.color = new Color(text.color.r,text.color.g,text.color.b,text.color.a + vel * Time.deltaTime);
    }
    void RemoveAlphaImage(Image image, float vel)
    {
        image.color = new Color(image.color.r,image.color.g,image.color.b,image.color.a + vel * Time.deltaTime);
    }

    void ReturnPrincipal()
    {
        if(!goingDown)
        {
            cam.GoUp();
            goingDown = true;
            StartCoroutine(Disapear());
        }
    }
    public void ContinueGame()
    {
        if(!goingDown)
        {
            PlayerPrefs.SetInt("RespawnSide",0);
            switch (PlayerPrefs.GetInt("CheckPoint", 0))
            {
                case 1:
                SceneManager.LoadScene("Lobby");
                break;

                case 2:
                SceneManager.LoadScene("Lobby");
                break;

                case 3:
                SceneManager.LoadScene("Vertical");
                break;

                case 4:
                SceneManager.LoadScene("Minas");
                break;

                case 5:
                SceneManager.LoadScene("Surface");
                break;

                case 6:
                SceneManager.LoadScene("Surface");
                break;

                default:
                SceneManager.LoadScene("Tutorial");
                break;
            }
        }
    }

    public void NewGameCheck()
    {
        if(PlayerPrefs.GetInt("CheckPoint",0) == 0)
        {
            NewGame();
        }
        else
        {
            OpenQuestion();
        }
    }
    public void NewGameZenCheck()
    {
        if(PlayerPrefs.GetInt("CheckPoint",0) == 0)
        {
            NewGameZen();
        }
        else
        {
            OpenQuestionZen();
        }
    }

    public void NewGame()
    {
        if(!goingDown)
        {
            //guardar opciones y achievements

            int full = PlayerPrefs.GetInt("FullScreen",1);
            int vsync = PlayerPrefs.GetInt("VSync",1);
            int shake = PlayerPrefs.GetInt("Shake",1);
            int subt = PlayerPrefs.GetInt("Subtitles",1);
            int curs = PlayerPrefs.GetInt("CursorLock",1);
            int healthbar = PlayerPrefs.GetInt("HealthBars",1);
            int res = PlayerPrefs.GetInt("resolutionOption",PlayerPrefs.GetInt("DefaultResolution"));

            float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
            float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
            float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            float voice = PlayerPrefs.GetFloat("VoiceVolume", 1);

            int [] ach = new int[11];
            for (int i = 1; i <= 10; i++)
            {
                ach[i] = PlayerPrefs.GetInt("Ach" + i, 0);
            }


            PlayerPrefs.DeleteAll();

            //restablecerlos a lo que valian

            PlayerPrefs.SetInt("FullScreen",full);
            PlayerPrefs.SetInt("VSync",vsync);
            PlayerPrefs.SetInt("Shake",shake);
            PlayerPrefs.SetInt("Subtitles",subt);
            PlayerPrefs.SetInt("CursorLock",curs);
            PlayerPrefs.SetInt("HealthBars",healthbar);
            PlayerPrefs.SetInt("resolutionOption",res);

            PlayerPrefs.SetFloat("MasterVolume", master);
            PlayerPrefs.SetFloat("MusicVolume", music);
            PlayerPrefs.SetFloat("SFXVolume", sfx);
            PlayerPrefs.SetFloat("VoiceVolume", voice);

            for (int i = 1; i <= 10; i++)
            {
                PlayerPrefs.SetInt("Ach" + i, ach[i]);
            }



            PlayerInputs.OnBack -= ReturnFromQuestion;
            PlayerPrefs.SetInt("CheckPoint",0);
            PlayerPrefs.SetInt("Skill1",1);
            PlayerPrefs.SetInt("Skill2",1);
            PlayerPrefs.SetInt("Dash",1);
            PlayerPrefs.SetInt("Roll",1);

            PlayerPrefs.SetInt("Zen",0);
            SceneManager.LoadScene("CinematicaLore");
        }
    }
    public void NewGameZen()
    {
        if(!goingDown)
        {
            //guardar opciones y achievements

            int full = PlayerPrefs.GetInt("FullScreen",1);
            int vsync = PlayerPrefs.GetInt("VSync",1);
            int shake = PlayerPrefs.GetInt("Shake",1);
            int subt = PlayerPrefs.GetInt("Subtitles",1);
            int curs = PlayerPrefs.GetInt("CursorLock",1);
            int healthbar = PlayerPrefs.GetInt("HealthBars",1);
            int res = PlayerPrefs.GetInt("resolutionOption",PlayerPrefs.GetInt("DefaultResolution"));

            float master = PlayerPrefs.GetFloat("MasterVolume", 1f);
            float music = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
            float sfx = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
            float voice = PlayerPrefs.GetFloat("VoiceVolume", 1);

            int [] ach = new int[11];
            for (int i = 1; i <= 10; i++)
            {
                ach[i] = PlayerPrefs.GetInt("Ach" + i, 0);
            }


            PlayerPrefs.DeleteAll();

            //restablecerlos a lo que valian

            PlayerPrefs.SetInt("FullScreen",full);
            PlayerPrefs.SetInt("VSync",vsync);
            PlayerPrefs.SetInt("Shake",shake);
            PlayerPrefs.SetInt("Subtitles",subt);
            PlayerPrefs.SetInt("CursorLock",curs);
            PlayerPrefs.SetInt("HealthBars",healthbar);
            PlayerPrefs.SetInt("resolutionOption",res);

            PlayerPrefs.SetFloat("MasterVolume", master);
            PlayerPrefs.SetFloat("MusicVolume", music);
            PlayerPrefs.SetFloat("SFXVolume", sfx);
            PlayerPrefs.SetFloat("VoiceVolume", voice);

            for (int i = 1; i <= 10; i++)
            {
                PlayerPrefs.SetInt("Ach" + i, ach[i]);
            }



            PlayerInputs.OnBack -= ReturnFromQuestion;
            PlayerPrefs.SetInt("CheckPoint",0);
            PlayerPrefs.SetInt("Skill1",1);
            PlayerPrefs.SetInt("Skill2",1);
            PlayerPrefs.SetInt("Dash",1);
            PlayerPrefs.SetInt("Roll",1);

            PlayerPrefs.SetInt("Zen",1);
            SceneManager.LoadScene("CinematicaLore");
        }
    }

    public void ReturnFromQuestion()
    {
        question.SetActive(false);
        arrayCards.SetActive(true);
        StartCoroutine(HighlightFirstButton(newGameButton));
        PlayerInputs.OnBack -= ReturnFromQuestion;
        PlayerInputs.OnBack += ReturnPrincipal;
    }
    public void ReturnFromQuestionZen()
    {
        zenQuestion.SetActive(false);
        arrayCards.SetActive(true);
        StartCoroutine(HighlightFirstButton(newGameZenButton));
        PlayerInputs.OnBack -= ReturnFromQuestionZen;
        PlayerInputs.OnBack += ReturnPrincipal;
    }

    public void OpenQuestion()
    {
        arrayCards.SetActive(false);
        question.SetActive(true);
        StartCoroutine(HighlightFirstButton(noButton));
        PlayerInputs.OnBack -= ReturnPrincipal;
        PlayerInputs.OnBack += ReturnFromQuestion;
    }
    public void OpenQuestionZen()
    {
        arrayCards.SetActive(false);
        zenQuestion.SetActive(true);
        StartCoroutine(HighlightFirstButton(noZenButton));
        PlayerInputs.OnBack -= ReturnPrincipal;
        PlayerInputs.OnBack += ReturnFromQuestionZen;
    }
}

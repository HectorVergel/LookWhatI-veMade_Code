using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.Events;
using TMPro;

public class MenuOptions : MonoBehaviour
{
    public bool menus;
    public GameObject principalMenu;
    public GameObject audioMenu;
    public GameObject displayMenu;
    public GameObject firstButton;

    //opciones
    public AudioMixer volMixer; 
    public float multiplier;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;
    public Slider voiceSlider;
    public TMP_Dropdown resolutionDropdown;
    private int screenInt;
    const string resName = "resolutionOption";
    Resolution[] resolutions;
    //nuevas opciones
    public Toggle fullScreenToggle;
    public Toggle vSyncToggle;
    public Toggle subtitlesToggle;
    public Toggle shakeToggle;
    public Toggle cursorLockToggle;
    public Toggle healthBarsToggle;
    public Slider brightnessSlider;
    int initialChildrens;
    int lastFrameChildrens;
    public TextMeshProUGUI descriptionLabel;
    public GameObject pauseMenu;
    public OptionsAudio audioManager;

    public void OnEnable()
    {
        Start();
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, PlayerPrefs.GetInt("DefaultResolution"));
        resolutionDropdown.RefreshShownValue();
        SetToggle(PlayerPrefs.GetInt("FullScreen",1),fullScreenToggle);
        SetToggle(PlayerPrefs.GetInt("VSync",1),vSyncToggle);
        SetToggle(PlayerPrefs.GetInt("Subtitles",1),subtitlesToggle);
        SetToggle(PlayerPrefs.GetInt("Shake",1),shakeToggle);
        SetToggle(PlayerPrefs.GetInt("CursorLock",1),cursorLockToggle);
        SetToggle(PlayerPrefs.GetInt("HealthBars",1),healthBarsToggle);
        //audio
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1);
        float vol1 = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume"))*multiplier;
        volMixer.SetFloat("Master", vol1);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("Music", vol2);

        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFX", vol3);

        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1);
        float vol4 = Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume"))*multiplier;
        volMixer.SetFloat("Voice", vol4);

        if(menus)
        {
            PlayerInputs.OnBack += ReturnPrincipal;
        }
        else
        {
            PlayerInputs.OnBack += ReturnPause;
            PlayerInputs.OnPause += pauseMenu.GetComponent<PauseMenu>().ResumeGame;
        }
        audioManager.StopSounds();
        audioMenu.SetActive(true);
        initialChildrens = resolutionDropdown.GetComponentsInChildren<RectTransform>().Length;
        lastFrameChildrens = initialChildrens;
    }
    private void OnDisable() 
    {
        if(menus)
        {
            PlayerInputs.OnBack -= ReturnPrincipal;
        }
        else
        {
            PlayerInputs.OnBack -= ReturnPause;
            PlayerInputs.OnPause -= pauseMenu.GetComponent<PauseMenu>().ResumeGame;
        }
        displayMenu.SetActive(false);
    }
    public void Awake() {
        SetToggle(PlayerPrefs.GetInt("FullScreen",1),fullScreenToggle);
        SetToggle(PlayerPrefs.GetInt("VSync",1),vSyncToggle);
        SetToggle(PlayerPrefs.GetInt("Subtitles",1),subtitlesToggle);
        SetToggle(PlayerPrefs.GetInt("Shake",1),shakeToggle);
        SetToggle(PlayerPrefs.GetInt("CursorLock",1),cursorLockToggle);
        SetToggle(PlayerPrefs.GetInt("HealthBars",1),healthBarsToggle);

        resolutionDropdown.onValueChanged.AddListener(new UnityAction<int>(index => 
        { 
            PlayerPrefs.SetInt(resName, resolutionDropdown.value); 
            PlayerPrefs.Save(); 
        }));
    }

    public void Start()
    {
        masterSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
        float vol1 = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume"))*multiplier;
        volMixer.SetFloat("Master", vol1);

        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.6f);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("Music", vol2); 

        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.75f);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFX", vol3);

        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume", 1);
        float vol4 = Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume"))*multiplier;
        volMixer.SetFloat("Voice", vol4);

        brightnessSlider.value = PlayerPrefs.GetFloat("Brightness", 1);

        resolutions = GetResolutions().ToArray();

        resolutionDropdown.ClearOptions();


        List<string> options = new List<string>();

        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRate + "HZ";
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

            if (resolutions[i].width == 1920 && resolutions[i].height == 1080)
            {
                PlayerPrefs.SetInt("DefaultResolution", i);
            }


        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, currentResolutionIndex);
        resolutionDropdown.RefreshShownValue();

        audioManager.StopSounds();
    }

    public void SetResolution(int index) 
    {
        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen(bool isfullscreen)
    {
        Screen.fullScreen = isfullscreen;

        if (isfullscreen == false)
        {
            PlayerPrefs.SetInt("FullScreen", 0);
        }
        else
        {
            isfullscreen = true;
            PlayerPrefs.SetInt("FullScreen", 1);
        }
    }
    public void SetVSync(bool isVSync)
    {
        if (isVSync)
        {
            QualitySettings.vSyncCount = 1;
            PlayerPrefs.SetInt("VSync", 1);
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            PlayerPrefs.SetInt("VSync", 0);
        }
    }

    public void SetSubtitles(bool isSubtitles)
    {
        if (isSubtitles)
        {
            PlayerPrefs.SetInt("Subtitles", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Subtitles", 0);
        }
    }
    public void SetMouseLock(bool isLock)
    {
        if (isLock)
        {
            PlayerPrefs.SetInt("CursorLock", 1);
        }
        else
        {
            PlayerPrefs.SetInt("CursorLock", 0);
        }
    }

    public void SetCamaraShake(bool isShake)
    {
        if (isShake)
        {
            PlayerPrefs.SetInt("Shake", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Shake", 0);
        }
    }
    public void SetHealthBars(bool isHealthBar)
    {
        if (isHealthBar)
        {
            PlayerPrefs.SetInt("HealthBars", 1);
        }
        else
        {
            PlayerPrefs.SetInt("HealthBars", 0);
        }
    }

    public void SetVolumeMaster(float volume) 
    {
        PlayerPrefs.SetFloat("MasterVolume", volume);
        float vol1 = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume"))*multiplier;
        volMixer.SetFloat("Master", vol1);
        audioManager.PlayMaster();
    }
    public void SetVolumeMusic(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("Music", vol2);
    }
    public void SetVolumeSFX(float volume)
    {
        PlayerPrefs.SetFloat("SFXVolume", volume);
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFX", vol3);
        audioManager.PlaySFX();
    }
    public void SetVolumeVoice(float volume)
    {
        PlayerPrefs.SetFloat("VoiceVolume", volume);
        float vol4 = Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume"))*multiplier;
        volMixer.SetFloat("Voice", vol4);
        audioManager.PlayVoice();
    }

    public void SetBrightness(float bright)
    {
        PlayerPrefs.SetFloat("Brightness", bright);
        Screen.brightness = bright;
    }

    List<Resolution> GetResolutions()
    {
        //Filters out all resolutions with low refresh rate:
        Resolution[] resolutions = Screen.resolutions;
        HashSet<System.ValueTuple<int, int>> uniqResolutions = new HashSet<System.ValueTuple<int, int>>();
        Dictionary<System.ValueTuple<int, int>, int> maxRefreshRates = new Dictionary<System.ValueTuple<int, int>, int>();
        for (int i = 0; i < resolutions.GetLength(0); i++)
        {
            //Add resolutions (if they are not already contained)
            System.ValueTuple<int, int> resolution = new System.ValueTuple<int, int>(resolutions[i].width, resolutions[i].height);
            uniqResolutions.Add(resolution);
            //Get highest framerate:
            if (!maxRefreshRates.ContainsKey(resolution))
            {
                maxRefreshRates.Add(resolution, resolutions[i].refreshRate);
            }
            else
            {
                maxRefreshRates[resolution] = resolutions[i].refreshRate;
            }
        }
        //Build resolution list:
        List<Resolution> uniqResolutionsList = new List<Resolution>(uniqResolutions.Count);
        foreach (System.ValueTuple<int, int> resolution in uniqResolutions)
        {
            Resolution newResolution = new Resolution();
            newResolution.width = resolution.Item1;
            newResolution.height = resolution.Item2;
            if (maxRefreshRates.TryGetValue(resolution, out int refreshRate))
            {
                newResolution.refreshRate = refreshRate;
            }
            uniqResolutionsList.Add(newResolution);
        }
        return uniqResolutionsList;
    }
    void SetToggle(int num, Toggle toggle)
    {
        if(num == 0)
        {
            toggle.isOn = false;
        }
        else
        {
            toggle.isOn = true;
        }
    }
    void ReturnPrincipal()
    {
        principalMenu.GetComponent<MenuPrincipal>().firstButton = principalMenu.GetComponent<MenuPrincipal>().optionsButton;
        principalMenu.SetActive(true);
        gameObject.SetActive(false);
    }
    public void ResetDisplay()
    {
        //reset variables
        PlayerPrefs.SetInt("FullScreen",1);
        PlayerPrefs.SetInt("VSync",1);
        PlayerPrefs.SetInt("Shake",1);
        PlayerPrefs.SetInt("Subtitles",1);
        PlayerPrefs.SetInt("CursorLock",1);
        PlayerPrefs.SetInt("HealthBars",1);
        PlayerPrefs.SetInt(resName,PlayerPrefs.GetInt("DefaultResolution"));

        //refresh UI
        resolutionDropdown.value = PlayerPrefs.GetInt(resName, PlayerPrefs.GetInt("DefaultResolution"));
        resolutionDropdown.RefreshShownValue();
        SetToggle(PlayerPrefs.GetInt("FullScreen",1),fullScreenToggle);
        SetToggle(PlayerPrefs.GetInt("VSync",1),vSyncToggle);
        SetToggle(PlayerPrefs.GetInt("Subtitles",1),subtitlesToggle);
        SetToggle(PlayerPrefs.GetInt("Shake",1),shakeToggle);
        SetToggle(PlayerPrefs.GetInt("CursorLock",1),cursorLockToggle);
        SetToggle(PlayerPrefs.GetInt("HealthBars",1),healthBarsToggle);

    }
    public void ResetAudio()
    {
        PlayerPrefs.SetFloat("MasterVolume", 1);
        masterSlider.value = 1;
        float vol1 = Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume"))*multiplier;
        volMixer.SetFloat("Master", vol1);

        PlayerPrefs.SetFloat("MusicVolume", 0.6f);
        musicSlider.value = 0.6f;
        float vol2 = Mathf.Log10(PlayerPrefs.GetFloat("MusicVolume"))*multiplier;
        volMixer.SetFloat("Music", vol2);

        PlayerPrefs.SetFloat("SFXVolume", 0.75f);
        sfxSlider.value = 0.75f;
        float vol3 = Mathf.Log10(PlayerPrefs.GetFloat("SFXVolume"))*multiplier;
        volMixer.SetFloat("SFX", vol3);

        PlayerPrefs.SetFloat("VoiceVolume", 1);
        voiceSlider.value = 1f;
        float vol4 = Mathf.Log10(PlayerPrefs.GetFloat("VoiceVolume"))*multiplier;
        volMixer.SetFloat("Voice", vol4);

        audioManager.StopSounds();
    }

    private void Update()
    {
        CheckIfDropdownChanged();
    }

    void CheckIfDropdownChanged()
    {
        int actualChildrens = resolutionDropdown.GetComponentsInChildren<RectTransform>().Length;
        if(actualChildrens != lastFrameChildrens)
        {
            if(actualChildrens == initialChildrens)
            {
                PlayerInputs.OnBack -= CloseDropdown;
                if(menus)
                {
                    PlayerInputs.OnBack += ReturnPrincipal;
                }
                else
                {
                    PlayerInputs.OnBack += ReturnPause;
                }
                
            }
            else
            {
                if(menus)
                {
                    PlayerInputs.OnBack -= ReturnPrincipal;
                }
                else
                {
                    PlayerInputs.OnBack -= ReturnPause;
                }
                PlayerInputs.OnBack += CloseDropdown;
            }
        }
        lastFrameChildrens = actualChildrens;
    }

    void CloseDropdown()
    {
        resolutionDropdown.Hide();
    }

    public void SetDescription(string desc)
    {
        descriptionLabel.text = desc;
    }

    void ReturnPause()
    {
        pauseMenu.GetComponent<PauseMenu>().firstButton = pauseMenu.GetComponent<PauseMenu>().optionsButton;
        pauseMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    public MapLevelSetter[] levelIcons = new MapLevelSetter[6];
    public Image playerIcon;
    public Transform[] iconPositions = new Transform[6];
    public Sprite notDiscoveredSprite;
    public Sprite[] playerIconSprites = new Sprite[2];
    public AudioSource audioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    private void OnEnable() {
        PlayerInputs.OnBack += CloseMap;
        PlayerInputs.OnCloseMap += CloseMap;
        Shake.instance.StopShake();
        InterfaceManager.instance.interfaceActive = true;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        MapTutorial();
        SetMap();
        audioSource.pitch = 1;
        audioSource.PlayOneShot(openSound);
        Time.timeScale = 0f;
        
    }
    private void OnDisable() {
        PlayerInputs.OnBack -= CloseMap;
        PlayerInputs.OnCloseMap -= CloseMap;
        InterfaceManager.instance.interfaceActive = false;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Time.timeScale = 1f;
        audioSource.pitch = 0.8f;
        audioSource.PlayOneShot(closeSound);
    }
    void MapTutorial()
    {
        if(FindObjectOfType<TutorialMap>() != null)
        {
            FindObjectOfType<TutorialMap>().Done();
        }
    }
    void CloseMap()
    {
        gameObject.SetActive(false);
    }

    void SetMap()
    {
        SetLevelIcons();
        SetPlayerPosition();
    }
    void SetPlayerPosition()
    {
        switch (SceneManager.GetActiveScene().name)
        {
            case "Lobby":
            SetCharacterPos(iconPositions[0]);
            break;
            
            case "Plane":
            SetCharacterPos(iconPositions[1]);
            break;

            case "Vertical":
            SetCharacterPos(iconPositions[2]);
            break;

            case "Minas":
            SetCharacterPos(iconPositions[3]);
            break;

            case "Surface":
            SetCharacterPos(iconPositions[4]);
            break;

            case "FinalBoss":
            SetCharacterPos(iconPositions[5]);
            break;

            default:
            SetCharacterPos(iconPositions[0]);
            break;
        }
        CheckCharacter();
    }
    void CheckCharacter()
    {
        if(PlayerPrefs.GetInt("Character",0) == 0)
        {
            playerIcon.sprite = playerIconSprites[0];
        }
        else
        {
            playerIcon.sprite = playerIconSprites[1];
        }
    }
    void SetLevelIcons()
    {
        foreach (var level in levelIcons)
        {
            level.SetSprite(notDiscoveredSprite);
        }
    }
    void SetCharacterPos(Transform iconPosition)
    {
        playerIcon.GetComponent<RectTransform>().position = iconPosition.position;
    }
}

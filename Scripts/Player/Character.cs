using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    public static Character instance;
    [SerializeField] GameObject warrior;
    [SerializeField] GameObject wizard;
    public Transform left;
    public Transform right;
    public Transform right2;
    [SerializeField] Transform checkpointRespawn1;
    [SerializeField] Transform checkpointRespawn2;

    
    public static CharacterType currentType;


    private void Awake() {
        instance = this;
    }
    private void Start() {
        Time.timeScale = 1;
        Respawn();
        
        if(PlayerPrefs.GetInt("CursorLock",1) == 1)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
    void CreatePlayer(Transform trans)
    {
        if(PlayerPrefs.GetInt("Character",0) == 0)
        {
            Instantiate(warrior,trans.position, Quaternion.identity);
            currentType = CharacterType.Warrior;
        }
        else
        {
            Instantiate(wizard,trans.position, Quaternion.identity);
            currentType = CharacterType.Wizard;
        }
        Hud.instance.ResetAbiltyUI();
        if(TutorialInfo.instance != null)
        {
            TutorialInfo.instance.ResetTutorial();
        }
    }

    public void Respawn()
    {
        switch (PlayerPrefs.GetInt("RespawnSide", 0))
        {
            case -1:
            CreatePlayer(left);
            break;

            case 1:
            CreatePlayer(right);
            break;

            case 2:
            CreatePlayer(right2);
            break;

            default:
            if(PlayerPrefs.GetInt("CheckPoint", 0) == 2 || PlayerPrefs.GetInt("CheckPoint", 0) == 6)
            {
                CreatePlayer(checkpointRespawn2);
                PlayerPrefs.SetInt("CurrentHealth",(int)StatsSystem.instance.currentHP);
            }
            else
            {
                CreatePlayer(checkpointRespawn1);
                PlayerPrefs.SetInt("CurrentHealth",(int)StatsSystem.instance.currentHP);
            }
            break;
        }
        PlayerPrefs.SetInt("RespawnSide", 0);
    }
}
public enum CharacterType
{
    Wizard,
    Warrior
}

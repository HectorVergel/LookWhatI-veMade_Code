using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.InputSystem;

public class AchievementsMenu : MonoBehaviour
{
    public GameObject principalMenu;

    private void OnEnable() 
    {
        PlayerInputs.OnBack += ReturnPrincipal;
        if(CheckIfLastAchievement())
        {
            StartCoroutine(LastAchievement());
        }
    }
    private void OnDisable() 
    {
        PlayerInputs.OnBack -= ReturnPrincipal;
    }

    void ReturnPrincipal()
    {
        principalMenu.GetComponent<MenuPrincipal>().firstButton = principalMenu.GetComponent<MenuPrincipal>().achievementsButton;
        principalMenu.SetActive(true);
        gameObject.SetActive(false);
    }

    bool CheckIfLastAchievement()
    {
        for (int i = 1; i < 10; i++)
        {
            if(PlayerPrefs.GetInt("Ach" + i.ToString(),0) == 0)
            {
                return false;
            }
        }
        return true;
    }
    IEnumerator LastAchievement()
    {
        yield return new WaitForSeconds(0.2f);
        GetComponent<AchievementPopUp>().Activate();
        var logros = FindObjectsOfType<Achievement>();
        foreach (var item in logros)
        {
            item.SetState();
        }
    }
}

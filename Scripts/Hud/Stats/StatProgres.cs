using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StatProgres : MonoBehaviour
{
    public Image imageHealth;
    public Image imageStrength;
    public Image imageHumor;
    public Image imageMana;

    public float barSpeed;
    public bool goingDown;
    public StatsSystem stats;
    
    private void OnEnable()
    {
        Time.timeScale = 0;
        PlayerInputs.OnBack += CloseStats;
        PlayerInputs.OnInventory += CloseStats;
        if(FindObjectOfType<TutorialConsumables>() != null)
        {
            FindObjectOfType<TutorialConsumables>().Done3();
        }
        imageHealth.fillAmount = 0;
        imageStrength.fillAmount = 0;
        imageMana.fillAmount = 0;
        imageHumor.fillAmount = 0;
        goingDown = false;
        stats.goingDown = false;
        StartCoroutine(GoUpFast(imageHealth, "Health"));
        StartCoroutine(GoUpFast(imageStrength, "Strength"));
        StartCoroutine(GoUpFast(imageMana, "Mana"));
        StartCoroutine(GoUpFast(imageHumor, "Luck"));
        stats.SetVisuals();
        stats.SetNumbersLabels();
        stats.redPoint = false;
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            
            stats.healthButton.gameObject.GetComponent<Button>().Select();
            StartCoroutine(CheckFirstGamepad());
            stats.healthButton.gameObject.GetComponent<Button>().OnSelect(null);
            
        }
        Cursor.visible = true;
        Shake.instance.StopShake();
        InterfaceManager.instance.interfaceActive = true;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        var nextNumbers = FindObjectsOfType<ButonsSelected>();
        foreach (var item in nextNumbers)
        {
            item.DesactivateLabel();
        }
        ResetButton reset = FindObjectOfType<ResetButton>();
        reset.DesactivateAll();

    }

    private void OnDisable() {
        Time.timeScale = 1;
        Cursor.visible = false;
        InterfaceManager.instance.interfaceActive = false;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        PlayerInputs.OnBack -= CloseStats;
        PlayerInputs.OnInventory -= CloseStats;
    }


   

    void CloseStats()
    {
        gameObject.SetActive(false);
    }

    IEnumerator CheckFirstGamepad()
    {
        yield return new WaitForSecondsRealtime(0.01f);
        stats.healthButton.gameObject.GetComponent<ButonsSelected>().Check();

    }

    IEnumerator GoDown(Image image, string name)
    {
        goingDown = true;

        while (image.fillAmount > 0f)
        {
          
            image.fillAmount -= barSpeed * 2 * Time.unscaledDeltaTime;
            yield return null;
        }
        
        image.fillAmount = 0f;
        CheckAllDown();
    }
    IEnumerator GoUp(Image image, string name)
    {
        while (goingDown)
        {
            yield return null;
        }
        while (image.fillAmount < (float)PlayerPrefs.GetInt(name, 0) / stats.maxLevels)
        {
            
            image.fillAmount += barSpeed * Time.unscaledDeltaTime;
            yield return null;
        }
        image.fillAmount = (float)PlayerPrefs.GetInt(name, 0) / stats.maxLevels;
    }
    IEnumerator GoUpFast(Image image, string name)
    {
        while (goingDown)
        {
            yield return null;
        }
        while (image.fillAmount < (float)PlayerPrefs.GetInt(name, 0) / stats.maxLevels)
        {
            
            image.fillAmount += barSpeed * 4 * Time.unscaledDeltaTime;
            yield return null;
        }
        image.fillAmount = (float)PlayerPrefs.GetInt(name, 0) / stats.maxLevels;
    }

    void CheckAllDown()
    {
        if (imageHealth.fillAmount == 0 && imageHumor.fillAmount == 0 && imageMana.fillAmount == 0 && imageStrength.fillAmount == 0)
        {
            goingDown = false;
        }
    }
    public void StartGoingDown()
    {
        StopAllCoroutines();
        StartCoroutine(GoDown(imageHealth,"Health"));
        StartCoroutine(GoDown(imageHumor, "Luck"));
        StartCoroutine(GoDown(imageStrength, "Strength"));
        StartCoroutine(GoDown(imageMana, "Mana"));
    }

    public void UpHealth()
    {
        StopCoroutine(GoUp(imageHealth, "Health"));

        StartCoroutine(GoUp(imageHealth, "Health"));
       
        
    }
    public void UpMana()
    {

        StopCoroutine(GoUp(imageMana, "Mana"));

        StartCoroutine(GoUp(imageMana, "Mana"));
    }
    public void UpHumor()
    {

        StopCoroutine(GoUp(imageHumor, "Luck"));
        StartCoroutine(GoUp(imageHumor, "Luck"));
        
    }
    public void UpStrength()
    {
       
        StopCoroutine(GoUp(imageStrength, "Strength"));
        StartCoroutine(GoUp(imageStrength, "Strength"));

    }
}

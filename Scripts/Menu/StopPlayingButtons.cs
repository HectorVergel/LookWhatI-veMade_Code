using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class StopPlayingButtons : MonoBehaviour,ISelectHandler,IPointerExitHandler,IPointerEnterHandler
{
    [TextArea(5,10)]
    public string description;
    public OptionsAudio audioManager;
    public MenuOptions options;
    void ISelectHandler.OnSelect(UnityEngine.EventSystems.BaseEventData eventData)
    {
        audioManager.StopSounds();
        options.SetDescription(description);

        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            audioManager.StopSounds();
            options.SetDescription(description);
        }
        else
        {
            StartCoroutine(Unselect());
        }
    }
    IEnumerator Unselect()
    {
        while(EventSystem.current.alreadySelecting)
        {
            yield return null;
        }
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        audioManager.StopSounds();
        options.SetDescription(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        audioManager.StopSounds();
        options.SetDescription("");
    }
}

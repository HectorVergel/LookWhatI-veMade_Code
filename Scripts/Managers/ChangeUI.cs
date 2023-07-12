using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class ChangeUI : MonoBehaviour
{
    public Sprite controller;
    public Sprite keyboardMouse;
    Image image;

    
    private void OnEnable() {
        image = GetComponent<Image>();
        if(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() == "gamepad")
        {
            image.sprite = controller;
        }
        else
        {
            image.sprite = keyboardMouse;
        }
    }

    public void ChangeUIScheme(bool keyboard)
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        if(keyboard)
        {
            image.sprite = keyboardMouse;
        }
        else
        {
            
            image.sprite = controller;
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using System;

public class PlayerInputs : MonoBehaviour
{
    public static Action OnJump;
    public static Action OnEndJump;
    public static Action <float> OnMove;
    public static Action OnDash;
    public static Action OnRoll;
    public static Action OnAttack;
    public static Action OnInteract;
    public static Action OnBack;
    public static Action OnAbility1;
    public static Action OnAbility2;
    public static Action OnInventory;
    public static Action OnUseConsumable;
    public static Action OnLeft;
    public static Action OnRight;
    public static Action OnLeftTrigger;
    public static Action OnRightTrigger;
    public static Action OnPause;
    public static Action OnSpaceBar;
    public static Action OnSpaceBarEnd;
    public static Action OnDown;
    public static Action OnOpenMap;
    public static Action OnCloseMap;
    public static Action OnConfirm;

    void OnEnable()
    {
        InputUser.onChange += onInputDeviceChange;
    }
    private void OnDestroy()
    {
        OnJump = null;
        OnEndJump=null;
        OnMove = null;
        OnDash = null;
        OnRoll = null;
        OnAttack = null;
        OnInteract = null;
        OnBack = null;
        OnAbility1 = null;
        OnAbility2 = null;
        OnInventory = null;
        OnUseConsumable = null;
        OnLeft = null;
        OnRight = null;
        OnLeftTrigger = null;
        OnRightTrigger = null;
        OnPause = null;
        OnSpaceBar = null;
        OnSpaceBarEnd = null;
        OnDown = null;
        OnOpenMap = null;
        OnCloseMap = null;
        OnConfirm = null;
    }
    void onInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
    {
        if (change == InputUserChange.ControlSchemeChanged)
        {
            if(user.controlScheme.ToString().ToLower().Contains("gamepad"))
            {
                var listOfSelectables = Selectable.allSelectablesArray;
                if(listOfSelectables.Length > 0)
                {
                    Selectable targetButton = listOfSelectables[0];

                    foreach (Selectable selectableUI in listOfSelectables)
                    {
                        if(selectableUI.GetComponent<RectTransform>().position.y > targetButton.GetComponent<RectTransform>().position.y)
                        {
                            targetButton = selectableUI;
                        }
                        else if(selectableUI.GetComponent<RectTransform>().position.y == targetButton.GetComponent<RectTransform>().position.y)
                        {
                            if(selectableUI.GetComponent<RectTransform>().position.x < targetButton.GetComponent<RectTransform>().position.x)
                            {
                                targetButton = selectableUI;
                            }
                        }
                    }
                    targetButton.Select();
                    targetButton.OnSelect(null);
                }
                //cambiar toda la UI de controles a las de gamepad
                var uiToChange = FindObjectsOfType<ChangeUI>();
                foreach (var item in uiToChange)
                {
                    item.ChangeUIScheme(false);
                }
            }
            else if(user.controlScheme.ToString().ToLower().Contains("keyboard"))
            {
                EventSystem.current.SetSelectedGameObject(null);
                //cambiar toda la UI de controles a las de keyboard
                var uiToChange = FindObjectsOfType<ChangeUI>();
                foreach (var item in uiToChange)
                {
                    item.ChangeUIScheme(true);
                }
                var descriptions = FindObjectsOfType<ButonsSelected>();
                foreach (var item in descriptions)
                {
                    item.DesactivateLabel();
                }
                var resetButons = FindObjectsOfType<ResetButton>();
                foreach (var item in resetButons)
                {
                    item.DesactivateAll();
                }
                var descriptionsOptions = FindObjectsOfType<MenuOptions>();
                foreach (var item in descriptionsOptions)
                {
                    item.SetDescription("");
                }
            }
        }
    }
    public void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>().x);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        
        if (context.performed)
        {
            
            OnJump?.Invoke();
        }

        if (context.canceled)
        {
            
            OnEndJump?.Invoke();
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            OnRoll?.Invoke();
            OnDash?.Invoke();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttack?.Invoke();
        }

    }

    public void Interacted(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteract?.Invoke();
        }
    }

    public void Ability1(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            OnAbility1?.Invoke();
        }
    }

    public void Ability2(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAbility2?.Invoke();
        }
    }

    public void Inventory(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInventory?.Invoke();
        }
    }

    public void UseCons(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnUseConsumable?.Invoke();
        }
           
    }

    public void LeftCons(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            OnLeft?.Invoke();
        }
           
    }

    public void RightCons(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            OnRight?.Invoke();
        }  
    }

    public void Back(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnBack?.Invoke();
        }  
    }
    public void RightTrigger(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            OnRightTrigger?.Invoke();
        }  
    }
    public void LeftTrigger(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            OnLeftTrigger?.Invoke();
        }
    }
    public void Pause(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnPause?.Invoke();
        }
    }
    public void Down(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0.7f)
        {
            OnDown?.Invoke();
        }
    }
    public void SpaceBar(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            
            OnSpaceBar?.Invoke();
        }

        if (context.canceled)
        {
            
            OnSpaceBarEnd?.Invoke();
        }
    }

    public void OpenMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnOpenMap?.Invoke();
        }  
    }

    public void CloseMap(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnCloseMap?.Invoke();
        }  
    }
    public void Confirm(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnConfirm?.Invoke();
        }
    }
}


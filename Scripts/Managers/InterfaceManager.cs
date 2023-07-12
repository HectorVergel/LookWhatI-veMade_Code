using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InterfaceManager : MonoBehaviour
{
    public static InterfaceManager instance;
    [SerializeField] GameObject stats;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject map;
    public bool interfaceActive = false;

    private void Awake() {
        instance = this;
    }
    private void OnEnable()
    {
        PlayerInputs.OnInventory += OpenStats;
        PlayerInputs.OnPause += Pause;
        PlayerInputs.OnOpenMap += OpenMap;
    }
    private void OnDisable()
    {
        PlayerInputs.OnInventory -= OpenStats;
        PlayerInputs.OnPause -= Pause;
        PlayerInputs.OnOpenMap -= OpenMap;
    }

    public void OpenShop()
    {
        if (!interfaceActive && Time.timeScale == 1)
        {
            shop.SetActive(true);
        }
    }


    void OpenStats()
    {
        if (!interfaceActive && Time.timeScale == 1)
        {
            stats.SetActive(true);
        }
    }
    void OpenMap()
    {
        if (!interfaceActive && Time.timeScale == 1 && PlayerPrefs.GetInt("Map",0) == 1)
        {
            map.SetActive(true);
        }
    }

    void Pause()
    {
        if (!interfaceActive && Time.timeScale == 1)
        {
            pause.SetActive(true);
        }
    }

}

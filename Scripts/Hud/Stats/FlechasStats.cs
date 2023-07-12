using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlechasStats : MonoBehaviour
{
    public GameObject[] warrior;
    public GameObject[] wizard;
    private void OnEnable() {
        if(Character.currentType == CharacterType.Warrior)
        {
            foreach (var item in warrior)
            {
                item.SetActive(true);
            }
            foreach (var item in wizard)
            {
                item.SetActive(false);
            }
        }
        else
        {
            foreach (var item in warrior)
            {
                item.SetActive(false);
            }
            foreach (var item in wizard)
            {
                item.SetActive(true);
            }
        }
    }
}

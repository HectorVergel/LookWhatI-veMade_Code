using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsSetter : MonoBehaviour
{
    public MenuOptions options;
    private void OnEnable()
    {
        options.gameObject.SetActive(true);
        options.gameObject.SetActive(false);
    }
}

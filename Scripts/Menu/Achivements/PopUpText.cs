using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopUpText : MonoBehaviour
{
    public string descriptionBefore;
    [System.NonSerialized]
    public string achievementName;
    public string descriptionAfter;
    public TextMeshProUGUI description;

    public void SetAll() {
        description.text = descriptionBefore +" "+ achievementName + descriptionAfter;
        GetComponent<AudioSource>().Play();
    }
}

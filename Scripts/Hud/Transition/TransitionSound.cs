using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TransitionSound : MonoBehaviour
{
  public TextMeshProUGUI deadText;

    private void OnEnable()
    {
        if (LoadLevel.instance.dead)
        {
            deadText.color = new Color (deadText.color.r,deadText.color.g,deadText.color.b,1);
        }
        else
        {
            deadText.color = new Color (deadText.color.r,deadText.color.g,deadText.color.b,0);
        }
    }

}

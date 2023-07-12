using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicDialogueTrigger : MonoBehaviour
{
    private void Start() {
        Time.timeScale = 1f;
        StartCoroutine(StartDialogScene());
    }
    IEnumerator StartDialogScene()
    {
        yield return new WaitForFixedUpdate();
        GetComponent<DialogueManager>().StartDialogue();
    }
}

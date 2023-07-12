using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public DialogueNode startNode;
    public float endTimeOffset;
    public int dialogNumber;
    public bool dontDestroy = false;
    public bool codeCall = false;

    AudioSource audioSource;
    DialogueNode currentNode;
    float maxTime;
    string text;
    AudioClip sound;
    BoxCollider2D boxCollider;
    [System.NonSerialized]
    public bool playing = false;

    [System.NonSerialized]
    public bool fadeIn = false;
    [System.NonSerialized]
    public bool fadeOut = false;

    private void Start() 
    {
        boxCollider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        if(PlayerPrefs.GetInt("Dialog" + dialogNumber.ToString(), 1) == 0 && !dontDestroy)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        if(other.tag == "Player" && CheckIfOtherDialogIsPlaying() && !codeCall && !playing)
        {
            StartDialogue();
        }
    }
    private void OnTriggerStay2D(Collider2D other) {

        if(other.tag == "Player" && CheckIfOtherDialogIsPlaying() && !codeCall && !playing)
        {
            StartDialogue();
        }
    }

    public void StartDialogue()
    {
        if(!playing)
        {
            StartCoroutine(NewNode(startNode));
            Destroy(boxCollider);
            DestroyOtherDialogsLikeMe();
            playing = true;
            PlayerPrefs.SetInt("Dialog" + dialogNumber.ToString(), 0);
        }
    }

    IEnumerator NewNode(DialogueNode node)
    {
        float time = 0;
        currentNode = node;
        SetVariables();
        SetVisuals();
        while(fadeIn)
        {
            yield return null;
        }
        while(time<maxTime)
        {
            time += Time.deltaTime;
            yield return null;
        }
        fadeOut = true;
        TextDialogue.instance.StartFadeOut(this);
        while(fadeOut)
        {
            yield return null;
        }
        if(currentNode.TargetNode != null)
        {
            StartCoroutine(NewNode(currentNode.TargetNode));
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    void SetVariables()
    {
        text = currentNode.Text;
        sound = currentNode.audio;
        maxTime = sound.length + endTimeOffset + currentNode.timeToStartNextDialogue;
    }
    void SetVisuals()
    {
        fadeIn = true;
        TextDialogue.instance.StartFadeIn(text,this);
        audioSource.clip = currentNode.audio;
        audioSource.Play();
    }

    void DestroyOtherDialogsLikeMe()
    {
        DialogueManager[] dialogs = FindObjectsOfType<DialogueManager>();
        foreach (var dialog in dialogs)
        {
            if(dialog.startNode == startNode && dialog != this)
            {
                Destroy(dialog.gameObject);
            }
        }
    }
    bool CheckIfOtherDialogIsPlaying()
    {
        DialogueManager[] dialogs = FindObjectsOfType<DialogueManager>();
        foreach (var dialog in dialogs)
        {
            if(dialog.playing && dialog != this)
            {
                return false;
            }
        }

        return true;
    }
}

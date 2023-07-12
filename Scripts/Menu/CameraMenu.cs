using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMenu : MonoBehaviour
{
    public static CameraMenu instance;
    public Transform pointA;
    public GameObject playMenu;
    public GameObject principalMenu;
    public bool goingDown = false;
    Animator animator;
    private void Awake() {
        instance = this;
    }
    private void Start() 
    {
        animator = GetComponent<Animator>();
        transform.position = pointA.position;
    }
    public void GoDown()
    {
        if(!goingDown)
        {
            goingDown = true;
            animator.Play("down");
        }
    }

    public void GoUp()
    {
        if(!goingDown)
        {
            goingDown = true;
            animator.Play("up");
        }
    }
    public void OpenPlayMenu()
    {
        playMenu.SetActive(true);
    }
    public void OpenPrincipalMenu()
    {
        principalMenu.SetActive(true);
    }
}

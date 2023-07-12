using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class OrbPopUp : MonoBehaviour
{

    public string skill1Name;
    public string skill2Name;
    public string dashName;
    public string rollName;
    public Sprite[] skill1Sprite = new Sprite[2];
    public Sprite[] skill2Sprite = new Sprite[2];
    public Sprite dashSprite;
    public Sprite rollSprite;

    public Sprite[] skill1Control = new Sprite[2];
    public Sprite[] skill2Control = new Sprite[2];
    public Sprite[] rollDashControl = new Sprite[2];

    public Sprite[] ilustrationsSkill1 = new Sprite[2];
    public Sprite[] ilustrationsSkill2 = new Sprite[2];
    public Sprite ilustrationDash;
    public Sprite ilustrationRoll;


    public TextMeshProUGUI title;
    public GameObject icon1;
    public GameObject icon2;
    public Transform iconCenterPosition;
    public float iconCenterScale;
    public Image[] spriteIcon = new Image[2];
    public ChangeUI[] controlIcon = new ChangeUI[2];
    public GameObject ilustrationsParent;
    public Image ilustration1;
    public Image ilustration2;
    public Transform ilustrationTopPos;
    public float ilustrationTopScale;

    [System.NonSerialized]
    public string orbName;


    private void Start() {
        Time.timeScale = 0f;
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("UI");
        ChooseOrb();
    }

    private void OnEnable() {
        PlayerInputs.OnBack += ClosePopUp;
    }
    private void OnDisable() {
        PlayerInputs.OnBack -= ClosePopUp;
    }

    void ClosePopUp()
    {
        Time.timeScale = 1f;
        if(FindObjectOfType<TutorialMap>() != null)
        {
            FindObjectOfType<TutorialMap>().StartTutorial();
        }
        InterfaceManager.instance.GetComponent<PlayerInput>().SwitchCurrentActionMap("Player");
        Destroy(GetComponentInParent<Transform>().gameObject);
    }
    void ChooseOrb()
    {
        switch (orbName)
        {
            case "Skill1":
            SetSkill1();
            break;
            
            case "Skill2":
            SetSkill2();
            break;

            case "Dash":
            SetDash();
            break;

            case "Roll":
            SetRoll();
            break;

            default:
            break;
        }
    }

    void SetSkill1()
    {
        title.text = skill1Name + " Unlocked!";
        spriteIcon[0].sprite = skill1Sprite[0];
        spriteIcon[1].sprite = skill1Sprite[1];
        foreach (ChangeUI change in controlIcon)
        {
            change.controller = skill1Control[0];
            change.keyboardMouse = skill1Control[1];
            change.ChangeUIScheme(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() != "gamepad");
        }
        ilustration1.sprite = ilustrationsSkill1[0];
        ilustration2.sprite = ilustrationsSkill1[1];
    }
    void SetSkill2()
    {
        title.text = skill2Name + " Unlocked!";
        spriteIcon[0].sprite = skill2Sprite[0];
        spriteIcon[1].sprite = skill2Sprite[1];
        foreach (ChangeUI change in controlIcon)
        {
            change.controller = skill2Control[0];
            change.keyboardMouse = skill2Control[1];
            change.ChangeUIScheme(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() != "gamepad");
        }
        ilustration1.sprite = ilustrationsSkill2[0];
        ilustration2.sprite = ilustrationsSkill2[1];
    }
    void SetDash()
    {
        title.text = dashName + " Unlocked!";
        Destroy(icon2);
        icon1.transform.position = new Vector3(iconCenterPosition.position.x,iconCenterPosition.position.y,iconCenterPosition.position.z);
        icon1.transform.localScale = new Vector3(iconCenterScale,iconCenterScale,iconCenterScale);
        spriteIcon[0].sprite = dashSprite;
        controlIcon[0].controller = rollDashControl[0];
        controlIcon[0].keyboardMouse = rollDashControl[1];
        controlIcon[0].ChangeUIScheme(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() != "gamepad");
        Destroy(ilustration2.gameObject);
        ilustration1.sprite = ilustrationDash;
        ilustrationsParent.transform.position = ilustrationTopPos.position;
        ilustrationsParent.transform.localScale = new Vector3(ilustrationTopScale,ilustrationTopScale,ilustrationTopScale);
    }
    void SetRoll()
    {
        title.text = rollName + " Unlocked!";
        Destroy(icon2);
        icon1.transform.position = new Vector3(iconCenterPosition.position.x,iconCenterPosition.position.y,iconCenterPosition.position.z);
        icon1.transform.localScale = new Vector3(iconCenterScale,iconCenterScale,iconCenterScale);
        spriteIcon[0].sprite = rollSprite;
        controlIcon[0].controller = rollDashControl[0];
        controlIcon[0].keyboardMouse = rollDashControl[1];
        controlIcon[0].ChangeUIScheme(FindObjectOfType<PlayerInput>().currentControlScheme.ToLower() != "gamepad");
        Destroy(ilustration2.gameObject);
        ilustration1.sprite = ilustrationRoll;
        ilustrationsParent.transform.position = ilustrationTopPos.position;
        ilustrationsParent.transform.localScale = new Vector3(ilustrationTopScale,ilustrationTopScale,ilustrationTopScale);
    }
}

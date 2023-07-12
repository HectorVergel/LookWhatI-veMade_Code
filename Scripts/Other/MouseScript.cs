using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    public float cooldownToDisappear;
    float timer;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
        {
            timer = 0;
            if(!Cursor.visible)
            {
                Cursor.visible = true;
            }
        }
        else if(timer<cooldownToDisappear)
        {
            timer+= Time.deltaTime;
        }
        else if(Cursor.visible)
        {
            Cursor.visible = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal; 

public class NIghtLight : MonoBehaviour
{
    public Light2D globalLight;
    public Color nightColor;
    public float nightIntensity = 1f;
    private void OnEnable() {
        if(PlayerPrefs.GetInt("BossMusic",0) == 1)
        {
            globalLight.color = nightColor;
        }
        //ojo, las particulas de viento se ven un poco feas en la oscuridad
    }
}

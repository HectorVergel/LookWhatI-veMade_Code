using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPlayerIcon : MonoBehaviour
{
    Image image;
    public float alphaSpeed;
    public float stayTime;
    private void OnEnable() {
        image = GetComponent<Image>();
        image.color = new Color(1,1,1,1);
        StartCoroutine(Fade());
    }

    IEnumerator Fade()
    {
        while (image.color.a < 1)
        {
            image.color = new Color(1,1,1,image.color.a + alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,1);

        //yield return new WaitForSeconds(stayTime);
        
        while (image.color.a > 0)
        {
            image.color = new Color(1,1,1,image.color.a - alphaSpeed * Time.deltaTime);
            yield return null;
        }
        image.color = new Color(1,1,1,0);
        StartCoroutine(Fade());
    }
}

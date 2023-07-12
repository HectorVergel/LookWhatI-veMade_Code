using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DamageText : MonoBehaviour
{
    public Color normal;
    public Color critic;
    [SerializeField] float speed;
    Vector2 direction;
    [SerializeField] float desiredScale;
    float initScale;
    [SerializeField] float angle;
    RectTransform rect;
    public GameObject critImage;
    public float initScaleNormal;
    public float initScaleCritic;
    
    
    public void SetColor(bool crit)
    {
        if(crit)
        {
            GetComponent<TextMeshProUGUI>().color = critic;
            critImage.SetActive(true);
            initScale = initScaleCritic;
            
        }
        else
        {
            GetComponent<TextMeshProUGUI>().color = normal;
            critImage.SetActive(false);
            initScale = initScaleNormal;
        }
    }

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        rect.localScale = new Vector3(initScale,initScale,initScale);
        direction = GetRandomDirection();

    }

    private void Update()
    {
        Move();
        ScaleObject();
    }

    private void Move()
    {
        rect.Translate(direction * speed * Time.deltaTime);
    } 

    private Vector3 GetRandomDirection()
    {
        float dir = Random.Range(-angle, angle);
        return new Vector3(dir, 1, 0).normalized; 
    }

    private void ScaleObject()
    {
        if(initScale == initScaleNormal)
        {
            float scale = Mathf.Lerp(initScale, desiredScale, GetComponent<DestroyInSeconds>().seconds);
            if(transform.localScale.x < scale)
            {
                rect.localScale = new Vector3(scale,scale,scale);
            }
        }
        else
        {
            float scale = Mathf.Lerp(initScale, desiredScale + initScaleCritic - initScaleNormal, GetComponent<DestroyInSeconds>().seconds);
            if(transform.localScale.x < scale)
            {
                rect.localScale = new Vector3(scale,scale,scale);
            }
        }
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PrintDamage : MonoBehaviour
{
    [SerializeField] GameObject floatingText;
    [SerializeField] RectTransform startingPoint;

    public void ShowText(float damage, bool crit)
    {
        GameObject prefab = Instantiate(floatingText, GetComponent<Collider2D>().bounds.center, Quaternion.identity);
        prefab.transform.SetParent(GetComponentInChildren<Canvas>().transform);
        prefab.GetComponent<TextMeshProUGUI>().text = damage.ToString();
        prefab.GetComponent<RectTransform>().position = startingPoint.position;
        prefab.GetComponent<DamageText>().SetColor(crit);
    }
}

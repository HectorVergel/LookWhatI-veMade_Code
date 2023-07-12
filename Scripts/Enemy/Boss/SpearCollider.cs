using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCollider : MonoBehaviour,IHaveHealth
{
    public Spear spear;

    public void TakeDamage(float amount)
    {
        spear.TakeDamage();
    }
}

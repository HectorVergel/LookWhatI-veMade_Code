using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteTriggerCinematic : MonoBehaviour
{
    public int indexNumber;
    
    private void OnEnable() {
        var spritesChanges = FindObjectsOfType<ChangeSpriteCinematic>();
        foreach (var item in spritesChanges)
        {
            item.ChangeSprite(indexNumber);
        }
    }
}

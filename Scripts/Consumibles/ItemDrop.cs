using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public string consumableName;
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            Consumables.instance.GiveConsumable(consumableName);

            bool otherItemsLeft = false;
            var items = FindObjectsOfType<ItemDrop>();
            foreach (var item in items)
            {
                if(item != this)
                {
                    otherItemsLeft = true;
                }
            }
            if(!otherItemsLeft)
            {
                if(FindObjectOfType<TutorialConsumables>() != null)
                {
                    FindObjectOfType<TutorialConsumables>().AskStart();
                }
            }
            Destroy(this.gameObject);
        }
    }
}

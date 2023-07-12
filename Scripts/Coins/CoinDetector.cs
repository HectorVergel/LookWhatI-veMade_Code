using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDetector : MonoBehaviour
{
    List<int> coin_IDs = new List<int>();
    public GameObject sound;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            bool _in = false;
            int _coinID = collision.GetComponent<CoinsDropped>().coin_ID;
            foreach (int ID in coin_IDs)
            {
                if(_coinID == ID)
                {
                    _in = true;
                }
            }
            if (!_in)
            {
                Coins.instance.AddCoins(collision.GetComponent<CoinsDropped>().gold);
                coin_IDs.Add(_coinID);
               
            }
            Instantiate(sound,transform.position,Quaternion.identity);
            Destroy(collision.gameObject);
        }
    }

}

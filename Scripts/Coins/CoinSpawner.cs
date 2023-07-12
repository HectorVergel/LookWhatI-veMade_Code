using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public int ID;
    public int gold;
    public int goldFraction;
    public float timeToSpawn;
    public int coinsToSpawn;
    public GameObject coinPref;


    RectTransform canvasRect;
    private void Start()
    {
        canvasRect = GameObject.FindGameObjectWithTag("Canvas").GetComponent<RectTransform>();
        CalculateNumberOfCoins();
        StartCoroutine(SpawnCoin());
        
    }

    IEnumerator SpawnCoin()
    {

        
        coinsToSpawn--;
        var go = Instantiate(coinPref, transform.position, Quaternion.identity);
        go.GetComponent<CoinsDropped>().gold = gold;
        go.GetComponent<CoinsDropped>().coin_ID = ID;
        go.transform.SetParent(canvasRect);
        go.GetComponent<WorldToUI>().spawnPoint = this.transform.position;
        if (coinsToSpawn > 0)
        {
            yield return new WaitForSeconds(timeToSpawn);
            StartCoroutine(SpawnCoin());
        }
        else
        {
            Destroy(this.gameObject);
        }


    }


    void CalculateNumberOfCoins()
    {
        coinsToSpawn = (int)(gold / goldFraction);

    }
}

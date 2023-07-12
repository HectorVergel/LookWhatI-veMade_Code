using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinsDropped : MonoBehaviour
{
    public int coin_ID;
    public int gold;
    float speed;
    public float maxSpeed;
    public float minSpeed;
    RectTransform _target;
    RectTransform rectTrans;

    private void Start()
    {

        rectTrans = GetComponent<RectTransform>();
        _target = FindObjectOfType<CoinDetector>().GetComponent<RectTransform>();
        speed = Random.Range(minSpeed, maxSpeed);
    }

    private void Update()
    {
        Move();
    }

    void Move()
    {
        Vector3 direction = _target.position - rectTrans.position;
        rectTrans.position += direction.normalized * speed * Time.deltaTime;
    }




}

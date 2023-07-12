using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearController : MonoBehaviour
{
    public float maxScale;
    public float minScale;
    public float cooldownSpear;
    public float spearSpawnDistance;
    public GameObject spear;
    float scaleGrow;

    private void Start() {
        StartCoroutine(ChangeSize());
        scaleGrow = maxScale - minScale;
    }
    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position,transform.position + new Vector3 (0,spearSpawnDistance,0));
    }

    IEnumerator ChangeSize()
    {
        float time = 0;
        transform.localScale = new Vector3(minScale,minScale,minScale);
        while(time < cooldownSpear/2)
        {
            ScaleCircleFirstHalf(time);
            time+=Time.deltaTime;
            yield return null;
        }
        time = 0;
        transform.localScale = new Vector3(maxScale,maxScale,maxScale);
        while(time < cooldownSpear/2)
        {
            ScaleCircleSecondHalf(time);
            time+=Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(minScale,minScale,minScale);
        Vector3 spawnPoint = transform.position + new Vector3 (0,spearSpawnDistance,0);
        Instantiate(spear,spawnPoint,Quaternion.identity);
        Destroy(this.gameObject);
    }

    void ScaleCircleFirstHalf(float time)
    {
        float percent = time/cooldownSpear*2;
        float desiredScale = minScale + scaleGrow*percent;
        transform.localScale =  new Vector3(desiredScale,desiredScale,desiredScale);
    }

    void ScaleCircleSecondHalf(float time)
    {
        float percent = time/cooldownSpear*2;
        float desiredScale = maxScale - scaleGrow*percent;
        transform.localScale =  new Vector3(desiredScale,desiredScale,desiredScale);
    }
}

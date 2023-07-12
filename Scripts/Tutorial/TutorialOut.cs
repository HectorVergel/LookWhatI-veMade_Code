using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialOut : MonoBehaviour
{
    public GameObject spikes;

    public Transform finalPosSpikes;
    public float velocity;
    public float timeWaitSpikes;

    IEnumerator MoveSpikes()
    {
        yield return new WaitForSeconds(timeWaitSpikes);
        while(spikes.transform.position.y > finalPosSpikes.position.y)
        {
            spikes.transform.position -= new Vector3(0, velocity * Time.deltaTime, 0);
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.tag == "Player")
        {
            HealthSystemPlayer.instance.TakeDamage(Consumables.instance.health1HP);
            StartCoroutine(MoveSpikes());
            Destroy(GetComponent<BoxCollider2D>());
        }
    }
}

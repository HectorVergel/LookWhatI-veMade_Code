using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	Transform target;
	Transform room;

	public static CameraFollow instance;

	[Range(5,-5)]
	public float minModX, maxModX, minModY, maxModY;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
		SetTarget();
		if(target != null)
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}

   
    void FixedUpdate()
	{

		if (target == null || room == null)
		{
			SetTarget();
		}
		else 
		{
			var minPosY = room.GetComponent<BoxCollider2D>().bounds.min.y + minModY;
			var maxPosY = room.GetComponent<BoxCollider2D>().bounds.max.y + maxModY;
			var minPosX = room.GetComponent<BoxCollider2D>().bounds.min.x + minModX;
			var maxPosX = room.GetComponent<BoxCollider2D>().bounds.max.x + maxModX;

			Vector3 clampedPos = new Vector3(Mathf.Clamp(target.position.x, minPosX, maxPosX), Mathf.Clamp(target.position.y, minPosY, maxPosY), Mathf.Clamp(target.position.z, -3.724273f, -3.724273f));

			transform.position = new Vector3(clampedPos.x, clampedPos.y, transform.position.z);
		}




		
	}

	void SetTarget()
    {
		if(target == null && FindObjectOfType<HealthSystemPlayer>() != null)
        {
			
			target = FindObjectOfType<HealthSystemPlayer>().transform;
			
		}
		if(room == null && GameObject.FindGameObjectWithTag("Room") != null)
        {
			room = GameObject.FindGameObjectWithTag("Room").transform;

		}
    }

	

}
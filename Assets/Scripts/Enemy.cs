using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	[SerializeField,Range(0,10)] float speed;
	
	private void Update()
	{
		transform.Translate(Vector3.forward*speed*Time.deltaTime);
	}
}

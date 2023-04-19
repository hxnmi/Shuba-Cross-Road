using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kobo : MonoBehaviour
{
	[SerializeField,Range(0,50)] float speed = 10;
	
	private void Update()
	{
		transform.Translate(Vector3.forward*speed*Time.deltaTime);
	}
}

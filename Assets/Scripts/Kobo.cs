using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Kobo : MonoBehaviour
{
	public UnityEvent OnKoboSpawn;
	[SerializeField,Range(0,50)] float speed = 10;
	
	private void Start() 
	{
		OnKoboSpawn.Invoke();
	}
	
	private void Update()
	{
		transform.Translate(Vector3.forward*speed*Time.deltaTime);
	}
}

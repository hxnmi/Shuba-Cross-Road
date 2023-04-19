using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboSpawner : MonoBehaviour
{
	[SerializeField] Kobo kobo;
	[SerializeField] Shuba shuba;
	[SerializeField] float initialTimer;
	
	float timer;
	void Start()
	{
		timer = initialTimer;
		kobo.gameObject.SetActive(false);
	}
	
	void Update()
	{
		if(timer<=0 && kobo.gameObject.activeInHierarchy == false)
		{
			kobo.gameObject.SetActive(true);
			kobo.transform.position = shuba.transform.position + new Vector3(0,1,13);
			shuba.SetMoveable(false);
		}
			
		timer -= Time.deltaTime;
	}
	
	public void ResetTimer()
	{
		timer = initialTimer;
	}
}

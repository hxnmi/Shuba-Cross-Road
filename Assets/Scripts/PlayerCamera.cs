using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckCamera : MonoBehaviour
{
	[SerializeField] Vector3 offset;
	[SerializeField] Vector3 targetPosition;
	
	private void start()
	{
		offset = this.transform.position;
	}
	public void UpdatePosition(Vector3.targetPosition)
	{
		DOTween.Kill(this.transform);
		transform.DOMove(offset+targetPosition)	;
	}
	
}

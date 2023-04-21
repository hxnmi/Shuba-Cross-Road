 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class Shuba : MonoBehaviour
{
	private Vector3 startTouchPosition;
	private Vector3 endTouchPosition;
	private float dragDistance;
	
	[SerializeField,Range(0,1)] float moveDuration = 0.1f;
	[SerializeField,Range(0, 1)] float jumpHeight = 0.5f;
	[SerializeField] int leftMoveLimit;
	[SerializeField] int rightMoveLimit;
	[SerializeField] int backMoveLimit;
	
	public UnityEvent<Vector3> OnJumpEnd;
	public UnityEvent<int> OnGetCoin;
	public UnityEvent OnDie;
	public UnityEvent OnCarCollision;
	public UnityEvent OnLive;
	
	private bool isMoveable = false;
	
	void Start()
	{
		dragDistance = Screen.height * 15 /100;
		
		InvokeRepeating("ShubaSay",0.5f,8f);
	}
	
	void Update()
	{	
		if(isMoveable == false)
			return;

		if (DOTween.IsTweening(transform))
			return;
			
		Vector3 direction = Vector3.zero;
		
		//input mobile
		if(Input.touchCount>0)
		{
			Touch touch = Input.GetTouch(0);
			if(touch.phase == TouchPhase.Began)
			{
				startTouchPosition = touch.position;
				endTouchPosition = touch.position;
			}
			else if(touch.phase == TouchPhase.Moved)
			{
				endTouchPosition = touch.position;
			}	
			else if(touch.phase == TouchPhase.Ended)
			{
				endTouchPosition = touch.position;
				if(Mathf.Abs(startTouchPosition.x-endTouchPosition.x)>dragDistance || Mathf.Abs(startTouchPosition.y-endTouchPosition.y)>dragDistance)
				{
					if(Mathf.Abs(startTouchPosition.x-endTouchPosition.x)>Mathf.Abs(startTouchPosition.y-endTouchPosition.y))
					{
						if (endTouchPosition.x > startTouchPosition.x)
						{
							direction += Vector3.right;
						}
						else if (endTouchPosition.x < startTouchPosition.x)
						{
							direction += Vector3.left;
						}
					}
					else
					{
						if (endTouchPosition.y > startTouchPosition.y)
						{
							direction += Vector3.forward;
						}
						else if (endTouchPosition.y < startTouchPosition.y)
						{
							direction += Vector3.back;
						}
					}
				}
			}
		}
		
		//input standalone
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
		{
			direction += Vector3.forward;
		}
		else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
		{
			direction += Vector3.back;
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			direction += Vector3.right;
		}
		else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			direction += Vector3.left;
		}

		if (direction == Vector3.zero)
			return;

		Move(direction);
	}

	public void Move(Vector3 direction)
	{
		var targetPosition = transform.position + direction;
		
		if(targetPosition.x < leftMoveLimit ||
		 targetPosition.x > rightMoveLimit ||
		  targetPosition.z < backMoveLimit ||
		  Tree.PositionSet.Contains(targetPosition))
		{
			targetPosition = transform.position;
		}
		
		transform.DOJump(
			targetPosition,
			jumpHeight, 
			1, 
			moveDuration)
			.onComplete = BroadCastPositionOnJumpEnd;
		
		transform.forward = direction;
	}
	
	public void SetMoveable(bool value)
	{
		isMoveable=value;
	}
	
	public void UpdateMoveLimit(int horizontalSize, int backLimit)
	{
		leftMoveLimit = -horizontalSize/2;
		rightMoveLimit = horizontalSize/2;
		backMoveLimit = backLimit;
	}
	
	private void BroadCastPositionOnJumpEnd()
	{
		OnJumpEnd.Invoke(transform.position);
	}
	
	private void OnTriggerEnter(Collider other) 
	{
		if(other.CompareTag("Car"))
		{
			if(transform.localScale.y == 0.2f)
				return;
			transform.DOScaleY(0.2f,0.2f);
			
			isMoveable = false;
			OnCarCollision.Invoke();
			Invoke("Die",3);
		}
		else if(other.CompareTag("Coin"))
		{
			var coin = other.GetComponent<Coin>();
			OnGetCoin.Invoke(coin.Value);
			coin.Collected();
		}
		else if(other.CompareTag("Kobo"))
		{
			if(this.transform != other.transform)
			{
				this.transform.SetParent(other.transform);
				Invoke("Die",3);
				this.transform.position = this.transform.parent.position + new Vector3(0,1,-0.5f);
				this.transform.rotation = Quaternion.Euler(0, 180, 0);
			}
		}
	}
	
	private void Die()
	{
		OnDie.Invoke();
	}
	
	void ShubaSay()
	{
		OnLive.Invoke();
	}
}

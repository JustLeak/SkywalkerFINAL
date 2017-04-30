using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drag : MonoBehaviour
{
	Vector3 dist;
	float posX;
	float posY;

	private void OnMouseDown()
	{
		dist = Camera.main.WorldToScreenPoint(transform.position);
		posX = Input.mousePosition.x - dist.x;
		posY = Input.mousePosition.y - dist.y;
	}
	private void OnMouseDrag()
	{
		Vector3 curPos = new Vector3(Input.mousePosition.x - posX, Input.mousePosition.y - posY, dist.z);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
		transform.position = worldPos;
	}
	private void Update()
	{
		if (Input.touchCount == 1) { 
			dist = Camera.main.WorldToScreenPoint(transform.position);
		posX = Input.GetTouch(0).position.x - dist.x;
		posY = Input.GetTouch(0).position.y - dist.y;

		Vector3 curPos = new Vector3(Input.GetTouch(0).position.x - posX, Input.GetTouch(0).position.y - posY, dist.z);
		Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
		transform.position = worldPos;
		}
	}
}
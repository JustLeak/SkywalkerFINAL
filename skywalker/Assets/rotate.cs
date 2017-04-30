using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotate : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		//this.transform.Rotate(Vector3.right, Time.deltaTime * 15);

		this.transform.Translate(Vector3.forward * Time.deltaTime);
		Debug.Log("asd");
	}
}

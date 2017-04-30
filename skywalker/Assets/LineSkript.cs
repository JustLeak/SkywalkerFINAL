using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineSkript : MonoBehaviour {
	[SerializeField] public Transform pos1;
	[SerializeField] public Transform pos2;

    // Use this for initialization
    private LineRenderer line;
    
    void Start () {
		Vector3 _pos1 = pos1.position;
		Vector3 _pos2 = pos2.position;
      line = transform.GetComponent<LineRenderer>();
      line.SetWidth(1000f, 1000f);
      line.SetPosition(0, _pos1);
      line.SetPosition(1, _pos2);
    }
	
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : MonoBehaviour {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                Application.LoadLevel(3);
        }
}


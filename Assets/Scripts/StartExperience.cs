using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartExperience : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(1))
        {
            GetComponent<ImageTrackingController>().enabled = true;
            this.enabled = false;
        }
	}
}

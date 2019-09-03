using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartExperience : MonoBehaviour
{


    // Use this for unit testing
    void Start () {
        //Invoke("UpdateTextTest", 2f);
	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetMouseButtonDown(1))
        {
            GetComponent<TrackingController>().enabled = true;
            this.enabled = false;
        }
	}

    private void UpdateTextTest()
    {
        GetComponent<View>().UpdateText("This is a test", true);
    }
}

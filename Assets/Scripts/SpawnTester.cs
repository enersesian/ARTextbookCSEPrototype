using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTester : MonoBehaviour
{
    public GameObject spawned;
	// Use this for initialization
	void Start () {
        Instantiate(spawned);
        Instantiate(spawned);
        Instantiate(spawned);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

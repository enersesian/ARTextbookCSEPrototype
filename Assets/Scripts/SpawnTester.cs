using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTester : MonoBehaviour
{
    public GameObject spawned;

	//initialization testing of prefabs
	void Start () {
        Instantiate(spawned);
        Instantiate(spawned);
        Instantiate(spawned);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggyInteractive : MonoBehaviour
{
    private AppManager appmng;

    private void Awake()
    {
        appmng = GameObject.FindGameObjectWithTag("GameController").GetComponent<AppManager>();
    }

    public void NotifyAppManagerOfLostTracking()
    {
        appmng.InputDetected(9); //8 for lost tracking on eggy interactive which is success for InactiveTrackingLesson
    }
}

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

public class TrackerBase : MonoBehaviour
{
    public bool[] interfaceBits;
    private TrackedImage thisTrackedImage;
    private bool coolDownTime = true;
    private int stationCounter;

    // Use this for initialization
    void Start()
    {
        thisTrackedImage = GetComponent<TrackedImage>();
        if (gameObject.name == "Task Machine(Clone)") interfaceBits = new bool[2];
        if (gameObject.name == "Number machine(Clone)") interfaceBits = new bool[4];
        if (gameObject.name == "Shape machine(Clone)") interfaceBits = new bool[3];
        if (gameObject.name == "Color machine(Clone)") interfaceBits = new bool[4];
        for (int i = 0; i < interfaceBits.Length; i++)
        {
            //interfaceBits[i] = true;
        }
    }

    public void SetInterface(int interfaceBit)
    {
        if(coolDownTime)
        {
            coolDownTime = false;
            interfaceBits[interfaceBit] = !interfaceBits[interfaceBit];
            //transform.GetChild(0).GetChild(interfaceBit).gameObject.SetActive(interfaceBits[interfaceBit]);
            Invoke("ResetCoolDownTimer", 2f);
        }
    }

    private void ResetCoolDownTimer()
    {
        coolDownTime = true;
    }

    public bool TrackingNotice(string trackerName, bool isTracking)
    {
        if (trackerName == "eggy") interfaceBits[0] = isTracking;
        if (trackerName == "TutorialInteractive(Clone)") interfaceBits[1] = isTracking;

        if (interfaceBits[0]) return true;
        else return false;
    }
}

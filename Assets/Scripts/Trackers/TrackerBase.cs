using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

public class TrackerBase : MonoBehaviour
{
    private bool[] interfaceBits = new bool[3];
    private TrackedImage thisTrackedImage;
    private bool coolDownTime = true;
    private int stationCounter;

    // Use this for initialization
    void Start()
    {
        thisTrackedImage = GetComponent<TrackedImage>();
        for(int i = 0; i < 3; i++)
        {
            interfaceBits[i] = true;
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
}

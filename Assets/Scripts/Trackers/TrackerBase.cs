﻿using System;
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
        if (gameObject.name == "TaskStation(Clone)") interfaceBits = new bool[2];
        if (gameObject.name == "NumberStation(Clone)") interfaceBits = new bool[4];
        if (gameObject.name == "ShapeStation(Clone)") interfaceBits = new bool[3];
        if (gameObject.name == "ColorStation(Clone)") interfaceBits = new bool[4];
        if (gameObject.name == "OutputStation(Clone)") interfaceBits = new bool[4];

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
        if (gameObject.name == "TaskStation(Clone)" || gameObject.name == "OutputStation(Clone)")
        {
            if (trackerName == "CookieBit(Clone)") interfaceBits[1] = isTracking;
        }
        if (gameObject.name == "NumberStation(Clone)")
        {
            if (trackerName == "CandyBit(Clone)") interfaceBits[1] = isTracking;
            if (trackerName == "CookieBit(Clone)") interfaceBits[2] = isTracking;
            if (trackerName == "CoffeeBit(Clone)") interfaceBits[3] = isTracking;
        }
        if (gameObject.name == "ShapeStation(Clone)")
        {
            if (trackerName == "CandyBit(Clone)") interfaceBits[1] = isTracking;
            if (trackerName == "CookieBit(Clone)") interfaceBits[2] = isTracking;
        }
        if (gameObject.name == "ColorStation(Clone)")
        {
            if (trackerName == "CandyBit(Clone)") interfaceBits[1] = isTracking;
            if (trackerName == "CookieBit(Clone)") interfaceBits[2] = isTracking;
            if (trackerName == "CoffeeBit(Clone)") interfaceBits[3] = isTracking;
        }

        if (interfaceBits[0] && trackerName == "CookieBit(Clone)")
        {
            if (isTracking)
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(13);
            }
            else
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(12);
            }
            return true;
        }
        else if (interfaceBits[0] && trackerName == "CandyBit(Clone)")
        {
            if (isTracking)
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(11);
            }
            else
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(10);
            }
            return true;
        }
        else if (interfaceBits[0] && trackerName == "CoffeeBit(Clone)")
        {
            if (isTracking)
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(15);
            }
            else
            {
                transform.parent.parent.GetComponent<AppManager>().InputDetected(14);
            }
            return true;
        }
        else return false;
    }
}

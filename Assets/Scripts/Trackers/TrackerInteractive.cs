﻿using System;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class TrackerInteractive : MonoBehaviour
{
    private TrackedImage thisTrackedImage;
    private TrackerBase mainTracker;
    private float timeSinceFullTrackingMethod;
    private bool isFullTracked;
    private bool currentElementIsBitSetToOne = true;
    private float birthTime;

    private void Start()
    {
        thisTrackedImage = transform.parent.GetComponent<TrackedImage>();
        mainTracker = transform.parent.parent.GetChild(0).GetChild(0).GetComponent<TrackerBase>();
        if (transform.GetChild(1).GetComponent<Text>()) transform.GetChild(1).GetComponent<Text>().text = "";
        birthTime = Time.time;
    }

    public void Update()
    {
        if (thisTrackedImage.image == null || thisTrackedImage.image.TrackingState != TrackingState.Tracking)
        {
            //foreach (var element in thisTrackedImage.ARBookPageElements) element.SetActive(false);
            return;
        }
        //give a 2 second window before it starts pinging away
        if (Time.time < birthTime + 2f) return; 

        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
        {
            isFullTracked = true;
            InteractionNotice(true);
            if (mainTracker.TrackingNotice(gameObject.name, true)) SetBit(true);
            else ResetBit();
        }

        //user placed down the peppermint token and covered the tracked image
        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && isFullTracked)
        {
            timeSinceFullTrackingMethod += Time.deltaTime;
            if (timeSinceFullTrackingMethod > 1.3f)
            {
                isFullTracked = false;
                InteractionNotice(false);
                if (mainTracker.TrackingNotice(gameObject.name, false)) SetBit(false);
                else ResetBit();
                timeSinceFullTrackingMethod = 0f;
            }
        }
        else timeSinceFullTrackingMethod = 0f;
    }

    private void SetBit(bool temp)
    {
        if (temp)
        {
            if(mainTracker.gameObject.name == "TaskStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "1";
            if (mainTracker.gameObject.name == "NumberStation(Clone)")
            {
                if (gameObject.name == "CoffeeBit(Clone)") transform.GetChild(1).GetComponent<Text>().text = "1";
                if (gameObject.name == "CookieBit(Clone)") transform.GetChild(1).GetComponent<Text>().text = "2";
                if (gameObject.name == "CandyBit(Clone)") transform.GetChild(1).GetComponent<Text>().text = "4";
            }
            if (mainTracker.gameObject.name == "ShapeStation(Clone)")
            {
                if (gameObject.name == "CookieBit(Clone)") transform.GetChild(1).GetComponent<Text>().text = "1";
                if (gameObject.name == "CandyBit(Clone)") transform.GetChild(1).GetComponent<Text>().text = "1";
            }
            currentElementIsBitSetToOne = false;
        }
        else
        {
            if (mainTracker.gameObject.name == "TaskStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "0";
            if (mainTracker.gameObject.name == "NumberStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "0";
            if (mainTracker.gameObject.name == "ShapeStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "0";
            currentElementIsBitSetToOne = true;
        }
    }

    private void ResetBit()
    {
        if (mainTracker.gameObject.name == "TaskStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "";
        if (mainTracker.gameObject.name == "NumberStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "";
        if (mainTracker.gameObject.name == "ShapeStation(Clone)") transform.GetChild(1).GetComponent<Text>().text = "";
    }

    private void InteractionNotice(bool isReady)
    {
        if (isReady)
        {
            if(transform.GetChild(0).GetComponent<Image>()) transform.GetChild(0).GetComponent<Image>().color = new Color(0f, 1f, 0f, transform.GetChild(0).GetComponent<Image>().color.a);
        }
        else
        {
            if(transform.GetChild(0).GetComponent<Image>()) transform.GetChild(0).GetComponent<Image>().color = new Color(1f, 0f, 0f, transform.GetChild(0).GetComponent<Image>().color.a);
            //was used for a removed part of eggy page tutorial
            if (GetComponent<EggyInteractive>()) GetComponent<EggyInteractive>().NotifyAppManagerOfLostTracking(); 
        }
    }
}

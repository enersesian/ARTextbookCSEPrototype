﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
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
        if (transform.GetChild(0).GetComponent<Text>()) transform.GetChild(0).GetComponent<Text>().text = "";
        if (transform.GetChild(2).GetComponent<Text>()) transform.GetChild(2).GetComponent<Text>().text = "";
        if (transform.GetChild(3).GetComponent<Text>()) transform.GetChild(3).GetComponent<Text>().text = "";
        birthTime = Time.time;
    }

    public void Update()
    {
        if (thisTrackedImage.image == null || thisTrackedImage.image.TrackingState != TrackingState.Tracking)
        {
            //foreach (var element in thisTrackedImage.ARBookPageElements) element.SetActive(false);
            return;
        }
        if (Time.time < birthTime + 2f) return; //give a 2 second window before it starts pinging away

        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
        {
            isFullTracked = true;
            InteractionNotice(true);
            if(mainTracker.TrackingNotice(gameObject.name, true)) SetBit(true);
            //transform.GetChild(0).GetComponent<Text>().text = mainTracker.TrackingNotice(gameObject.name, true).ToString();
        }

        //user placed down the peppermint token and covered the tracked image
        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && isFullTracked)
        {
            timeSinceFullTrackingMethod += Time.deltaTime;
            if (timeSinceFullTrackingMethod > 1.3f)
            {
                //SetBit(false);
                isFullTracked = false;
                InteractionNotice(false);
                if (mainTracker.TrackingNotice(gameObject.name, false)) SetBit(false);
                //transform.GetChild(0).GetComponent<Text>().text = mainTracker.TrackingNotice(gameObject.name, false).ToString();
                timeSinceFullTrackingMethod = 0f;
            }
        }
        else
        {
            //thisTrackedImage.ARBookPageElements[thisTrackedImage.thisImageDatabaseElement].SetActive(true);
            timeSinceFullTrackingMethod = 0f;
        }
    }

    private void SetBit(bool temp)
    {
        if (temp)//currentElementIsBitSetToOne)
        {
            if(transform.GetChild(2).GetComponent<Text>()) transform.GetChild(2).GetComponent<Text>().text = "";
            if (transform.GetChild(3).GetComponent<Text>()) transform.GetChild(3).GetComponent<Text>().text = "1";
            if (transform.GetChild(0).GetComponent<Text>()) transform.GetChild(0).GetComponent<Text>().text = "";
            currentElementIsBitSetToOne = false;
        }
        else
        {
            if(transform.GetChild(2).GetComponent<Text>()) transform.GetChild(2).GetComponent<Text>().text = "";
            if (transform.GetChild(3).GetComponent<Text>()) transform.GetChild(3).GetComponent<Text>().text = "0";
            if (transform.GetChild(0).GetComponent<Text>()) transform.GetChild(0).GetComponent<Text>().text = "";
            currentElementIsBitSetToOne = true;
        }
    }

    private void InteractionNotice(bool isReady)
    {
        if (isReady)
        {
            if(transform.GetChild(1).GetComponent<Image>()) transform.GetChild(1).GetComponent<Image>().color = new Color(0f, 1f, 0f, transform.GetChild(1).GetComponent<Image>().color.a);
        }
        else
        {
            if(transform.GetChild(1).GetComponent<Image>()) transform.GetChild(1).GetComponent<Image>().color = new Color(1f, 0f, 0f, transform.GetChild(1).GetComponent<Image>().color.a);
            if (GetComponent<EggyInteractive>()) GetComponent<EggyInteractive>().NotifyAppManagerOfLostTracking(); //was used for a removed part of eggy page tutorial
        }
    }
}

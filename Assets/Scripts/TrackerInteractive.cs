using System;
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

    private void Start()
    {
        thisTrackedImage = transform.parent.GetComponent<TrackedImage>();
    }

    public void Update()
    {
        if (thisTrackedImage.image == null || thisTrackedImage.image.TrackingState != TrackingState.Tracking)
        {
            //foreach (var element in thisTrackedImage.ARBookPageElements) element.SetActive(false);
            return;
        }

        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
        {
            isFullTracked = true;
            InteractionNotice(true);
            SetBit(true);
        }

        //user placed down the peppermint token and covered the tracked image
        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && isFullTracked)
        {
            timeSinceFullTrackingMethod += Time.deltaTime;
            if (timeSinceFullTrackingMethod > 1f)
            {
                SetBit(false);
                isFullTracked = false;
                timeSinceFullTrackingMethod = 0f;
                InteractionNotice(false);
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
            transform.GetChild(0).GetComponent<Text>().text = "0";
            transform.GetChild(1).GetComponent<Text>().text = "Off";
            currentElementIsBitSetToOne = false;
        }
        else
        {
            transform.GetChild(0).GetComponent<Text>().text = "1";
            transform.GetChild(1).GetComponent<Text>().text = "On";
            currentElementIsBitSetToOne = true;
        }
    }

    private void InteractionNotice(bool isReady)
    {
        if (isReady)
        {
            transform.GetChild(2).GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.3f);
        }
        else
        {
            transform.GetChild(2).GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.3f);
        }
    }
}

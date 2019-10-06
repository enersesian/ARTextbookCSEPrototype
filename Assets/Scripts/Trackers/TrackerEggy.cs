using System;
using System.Collections.Generic;
using GoogleARCore;
using UnityEngine;
using UnityEngine.UI;

public class TrackerEggy : MonoBehaviour
{
    private TrackedImage thisTrackedImage;
    private TrackerBase mainTracker;
    private float timeSinceFullTrackingMethod;
    private bool isFullTracked;
    private bool currentElementIsBitSetToOne = true;

    private void Start()
    {
        thisTrackedImage = transform.parent.GetComponent<TrackedImage>();
        mainTracker = GetComponent<TrackerBase>();
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
            if(mainTracker != null) mainTracker.TrackingNotice("eggy", true);
        }

        //user placed down the peppermint token and covered the tracked image
        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && isFullTracked)
        {
            timeSinceFullTrackingMethod += Time.deltaTime;
            if (timeSinceFullTrackingMethod > 1f)
            {
                isFullTracked = false;
                timeSinceFullTrackingMethod = 0f;
                InteractionNotice(false);
                if (mainTracker != null) mainTracker.TrackingNotice("eggy", false);
            }
        }
        else timeSinceFullTrackingMethod = 0f;
    }

    private void SetBit(bool temp)
    {
        if (temp)
        {
            if(transform.GetChild(0).GetComponent<Text>()) transform.GetChild(0).GetComponent<Text>().text = "0";
            if(transform.GetChild(1).GetComponent<Text>()) transform.GetChild(1).GetComponent<Text>().text = "Off";
            currentElementIsBitSetToOne = false;
        }
        else
        {
            if(transform.GetChild(0).GetComponent<Text>()) transform.GetChild(0).GetComponent<Text>().text = "1";
            if(transform.GetChild(1).GetComponent<Text>()) transform.GetChild(1).GetComponent<Text>().text = "On";
            currentElementIsBitSetToOne = true;
        }
    }

    private void InteractionNotice(bool isReady)
    {
        if (isReady) transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.green);
        else transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.red);
    }
}

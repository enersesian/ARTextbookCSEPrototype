using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GoogleARCore;
using GoogleARCoreInternal;
using UnityEngine;

public class TrackerInteractive : MonoBehaviour
{
    private TrackedImage thisTrackedImage;
    private TrackerBase mainTracker;
    private float timeSinceFullTrackingMethod;
    private bool isFullTracked;

    private void Start()
    {
        thisTrackedImage = GetComponent<TrackedImage>();
        mainTracker = thisTrackedImage.GetMainTracker();
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
            thisTrackedImage.InteractionNotice(true);
            thisTrackedImage.SetBit(true);
        }

        //user placed down the peppermint token and covered the tracked image
        if (thisTrackedImage.image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && isFullTracked)
        {
            timeSinceFullTrackingMethod += Time.deltaTime;
            if (timeSinceFullTrackingMethod > 1f)
            {
                //thisTrackedImage.ARBookPageElements[thisTrackedImage.thisImageDatabaseElement].SetActive(false);
                //mainTracker = thisTrackedImage.GetMainTracker(); //secondary tracker may be created before main tracker and cant set this in start method
                //mainTracker.SetInterface(thisTrackedImage.thisImageDatabaseElement - 1);

                thisTrackedImage.SetBit(false);
                isFullTracked = false;
                timeSinceFullTrackingMethod = 0f;
                thisTrackedImage.InteractionNotice(false);
            }
        }
        else
        {
            //thisTrackedImage.ARBookPageElements[thisTrackedImage.thisImageDatabaseElement].SetActive(true);
            timeSinceFullTrackingMethod = 0f;
        }
    }
}

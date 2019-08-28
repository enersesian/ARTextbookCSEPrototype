namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    public class TrackerInteractive : MonoBehaviour
    {
        private FoundImageManager visualizer;
        private TrackerBase mainTracker;
        private float timeSinceFullTrackingMethod;
        private bool hasBeenFullTracked;

        private void Start()
        {
            visualizer = GetComponent<FoundImageManager>();
            mainTracker = visualizer.GetMainTracker();
        }

        public void Update()
        {
            if (visualizer.Image == null || visualizer.Image.TrackingState != TrackingState.Tracking)
            {
                foreach (var element in visualizer.ARBookPageElements) element.SetActive(false);
                return;
            }

            if (visualizer.Image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking)
            {
                hasBeenFullTracked = true;
            }

            if (visualizer.Image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && hasBeenFullTracked)
            {
                timeSinceFullTrackingMethod += Time.deltaTime;
                if (timeSinceFullTrackingMethod > 1f)
                {
                    visualizer.ARBookPageElements[visualizer.thisInterfaceElement].SetActive(false);
                    mainTracker = visualizer.GetMainTracker(); //secondary tracker may be created before main tracker and cant set this in start method
                    mainTracker.SetInterface(visualizer.thisInterfaceElement - 1);
                    timeSinceFullTrackingMethod = 0f;
                }
            }
            else
            {
                visualizer.ARBookPageElements[visualizer.thisInterfaceElement].SetActive(true);
                timeSinceFullTrackingMethod = 0f;
            }
        }
    }
}

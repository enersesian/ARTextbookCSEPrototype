//-----------------------------------------------------------------------
// <copyright file="AugmentedImageVisualizer.cs" company="Google">
//
// Copyright 2018 Google Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    /// <summary>
    /// Used to track elements on an AR book page
    /// </summary>
    public class ARBookPageVisualizer : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage Image;

        /// <summary>
        /// A world space UI to act as a three bit counter for number station.
        /// </summary>
        public GameObject[] ARBookPageElements;

        public static ARBookPageVisualizer[] ARBookPageInterface = new ARBookPageVisualizer[4];
        public int thisInterfaceElement;
        //private float timeSinceFullTrackingMethod;

        public void Start()
        {
            ARBookPageInterface[Image.DatabaseIndex] = this; //Record which of the four interface elements this object is
            thisInterfaceElement = Image.DatabaseIndex; //Have this object remember which interface element it is
            ARBookPageElements[thisInterfaceElement].SetActive(true);
            if (thisInterfaceElement == 0)
            {
                GetComponent<ARBookPageMainTracker>().enabled = true;
                GetComponent<ARBookPageSecondaryTracker>().enabled = false;
            }
            else
            {
                GetComponent<ARBookPageMainTracker>().enabled = false;
                GetComponent<ARBookPageSecondaryTracker>().enabled = true;
            }
        }

        public ARBookPageMainTracker GetMainTracker()
        {
            return ARBookPageInterface[0].GetComponent<ARBookPageMainTracker>();
        }

        /*
        public void Update()
        {
            if (Image == null || Image.TrackingState != TrackingState.Tracking)
            {
                foreach (var element in ARBookPageElements) element.SetActive(false);
                return;
            }

            if (Image.TrackingMethod == AugmentedImageTrackingMethod.LastKnownPose && thisInterfaceElement > 0)
            {
                timeSinceFullTrackingMethod += Time.deltaTime;
                if (timeSinceFullTrackingMethod > 1f)
                {
                    ARBookPageElements[thisInterfaceElement].SetActive(false);
                    ARBookPageInterface[0].GetComponent<ARBookPageMainTracker>().SetInterface(thisInterfaceElement);
                    timeSinceFullTrackingMethod = 0f;
                }
            }
            else
            {
                ARBookPageElements[thisInterfaceElement].SetActive(true);
                timeSinceFullTrackingMethod = 0f;
            }
        }
        */
    }
}

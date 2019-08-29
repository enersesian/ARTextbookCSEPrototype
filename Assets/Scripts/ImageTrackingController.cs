//-----------------------------------------------------------------------
// <copyright file="AugmentedImageExampleController.cs" company="Google">
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
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using UnityEngine;
    using UnityEngine.UI;

    /// <summary>
    /// Controller for AugmentedImage example.
    /// </summary>
    /// <remarks>
    /// In this sample, we assume all images are static or moving slowly with
    /// a large occupation of the screen. If the target is actively moving,
    /// we recommend to check <see cref="AugmentedImage.TrackingMethod"/> and
    /// render only when the tracking method equals to
    /// <see cref="AugmentedImageTrackingMethod"/>.<c>FullTracking</c>.
    /// See details in <a href="https://developers.google.com/ar/develop/c/augmented-images/">
    /// Recognize and Augment Images</a>
    /// </remarks>
    public class ImageTrackingController : MonoBehaviour
    {
        /// <summary>
        /// A prefab for visualizing an AugmentedImage.
        /// </summary>
        public TrackedImage TrackedImagePrefab;

        /// <summary>
        /// The overlay containing the fit to scan user guide.
        /// </summary>
        public GameObject FitToScanOverlay;

        private Dictionary<int, TrackedImage> m_TrackedImages
            = new Dictionary<int, TrackedImage>();

        private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

        private TrackedImage currentlyTracked;

        public Text printToScreen;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
        }

        /// <summary>
        /// The Unity Update method.
        /// </summary>
        public void Update()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            // Get updated augmented images for this frame.
            Session.GetTrackables<AugmentedImage>(
                m_TempAugmentedImages, TrackableQueryFilter.Updated);

            // Create visualizers and anchors for updated augmented images that are tracking and do
            // not previously have a visualizer. Remove visualizers for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                currentlyTracked = null;
                m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
                if (image.TrackingState == TrackingState.Tracking && image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && currentlyTracked == null)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    currentlyTracked = (TrackedImage)Instantiate(
                        TrackedImagePrefab, anchor.transform);
                    currentlyTracked.image = image;
                    currentlyTracked.transform.rotation = Quaternion.identity;
                    currentlyTracked.transform.parent = this.transform;
                    m_TrackedImages.Add(image.DatabaseIndex, currentlyTracked);
                }
                else if (image.TrackingState == TrackingState.Stopped && currentlyTracked != null)
                {
                    m_TrackedImages.Remove(image.DatabaseIndex);
                    GameObject.Destroy(currentlyTracked.gameObject);
                }
            }

            //User touched screen to reset tracking if issues or turning page in book
            if (Input.GetMouseButtonDown(1))
            {
                foreach (var image in m_TempAugmentedImages)
                {
                    currentlyTracked = null;
                    m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
                    if (currentlyTracked != null)
                    {
                        m_TrackedImages.Remove(image.DatabaseIndex);
                        GameObject.Destroy(currentlyTracked.gameObject);
                    }
                }
            }

            if(Input.GetMouseButton(0))
            {
                printToScreen.text = Input.GetTouch(0).deltaPosition.x.ToString();
                for(int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).rotation = Quaternion.Euler(0f, transform.GetChild(i).rotation.eulerAngles.y + Input.GetTouch(0).deltaPosition.x, 0f);
                }
            }

            // Show the fit-to-scan overlay if there are no images that are Tracking.
            foreach (var potentallyTrackedImages in m_TrackedImages.Values)
            {
                if (potentallyTrackedImages.image.TrackingState == TrackingState.Tracking)
                {
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            }

            FitToScanOverlay.SetActive(true);
        }
    }
}

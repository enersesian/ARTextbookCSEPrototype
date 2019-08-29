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
    using UnityEngine.UI;

    /// <summary>
    /// Used to track elements on an AR book page
    /// </summary>
    public class TrackedImage : MonoBehaviour
    {
        /// <summary>
        /// The AugmentedImage to visualize.
        /// </summary>
        public AugmentedImage image;

        /// <summary>
        /// A world space UI to act as a three bit counter for number station.
        /// </summary>
        public GameObject[] ARBookPageElements;
        public GameObject tutorialStation, numberStation, shapeStation, colorStation, leverStatus;

        public static TrackedImage[] imageDatabaseElement = new TrackedImage[7];
        public int thisImageDatabaseElement;

        private GameObject currentElement;
        private bool currentElementIsBitSetToOne;
        private ImageTrackingController imageTrackingController;

        public void Start()
        {
            imageTrackingController = transform.parent.GetComponent<ImageTrackingController>();
            //Record which of the four interface elements this object is
            imageDatabaseElement[image.DatabaseIndex] = this;
            //Have this object remember which interface element it is
            thisImageDatabaseElement = image.DatabaseIndex;
            //imageTrackingController.printToScreen.text += " " + thisImageDatabaseElement.ToString();
            //ARBookPageElements[thisImageDatabaseElement].SetActive(true);
            switch(thisImageDatabaseElement)
            {
                case 0: //color station page
                    break;

                case 1: //number station page
                    break;

                case 2: //shape station page
                    break;

                case 3: //tutorial station page
                    currentElement = Instantiate(tutorialStation, transform.position, Quaternion.identity);
                    currentElement.transform.parent = transform;
                    break;

                case 4: //lever left
                    break;

                case 5: //lever middle
                    currentElement = Instantiate(leverStatus, transform.position, Quaternion.identity);
                    currentElement.transform.parent = transform;
                    break;

                case 6: //lever right
                    break;

                case 7: //UI, not used right now
                    break;

                default:
                    break;
            }
            
            if (thisImageDatabaseElement < 4)
            {
                GetComponent<TrackerBase>().enabled = true;
                GetComponent<TrackerInteractive>().enabled = false;
            }
            else
            {
                GetComponent<TrackerBase>().enabled = false;
                GetComponent<TrackerInteractive>().enabled = true;
            }
            
        }

        public TrackerBase GetMainTracker()
        {
            return imageDatabaseElement[0].GetComponent<TrackerBase>();
        }

        public void SetBit()
        {
            if(currentElementIsBitSetToOne)
            {
                currentElement.transform.GetChild(0).GetComponent<Text>().text = "0";
                currentElementIsBitSetToOne = false;
            }
            else
            {
                currentElement.transform.GetChild(0).GetComponent<Text>().text = "1";
                currentElementIsBitSetToOne = true;
            }
        }
    }
}

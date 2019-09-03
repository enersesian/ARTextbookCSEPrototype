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
    private ImageTrackingController imageTrackingController;

    public void Start()
    {
        imageTrackingController = transform.parent.GetComponent<ImageTrackingController>();
        //Record which of the four interface elements this object is
        imageDatabaseElement[image.DatabaseIndex] = this;
        //Have this object remember which interface element it is
        thisImageDatabaseElement = image.DatabaseIndex;
        switch(thisImageDatabaseElement)
        {
            case 0:
                currentElement = Instantiate(tutorialStation, transform.position, Quaternion.identity);
                currentElement.GetComponent<Transition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            case 5:
                currentElement = Instantiate(leverStatus, transform.position, Quaternion.identity);
                currentElement.GetComponent<Transition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
        } 
    }

    public void Remove()
    {
        currentElement.GetComponent<Transition>().TurnOff();
        Destroy(gameObject, 2f);
    }
}

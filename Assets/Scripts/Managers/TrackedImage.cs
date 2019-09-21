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
public class TrackedImage : Listener
{
    /// <summary>
    /// The AugmentedImage to visualize.
    /// </summary>
    public AugmentedImage image;
    public Anchor anchor;

    /// <summary>
    /// A world space UI to act as a three bit counter for number station.
    /// </summary>
    public GameObject[] ARBookPageElements;
    public GameObject eggy, eggyInteractive, tutorialInteractive, centerInteractive, tutorialStation, taskStation, numberStation, shapeStation, colorStation;

    public static TrackedImage[] imageDatabaseElement = new TrackedImage[9];
    public int thisImageDatabaseElement;

    private GameObject currentElement;
    private TrackingController imageTrackingController;

    public void Start()
    {
        imageTrackingController = transform.parent.GetComponent<TrackingController>();
        //Record which of the four interface elements this object is
        imageDatabaseElement[image.DatabaseIndex] = this;
        //Have this object remember which interface element it is
        thisImageDatabaseElement = image.DatabaseIndex;
        switch(thisImageDatabaseElement)
        {
            case 0:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "Eggy";
                currentElement = Instantiate(eggy, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                //currentElement.transform.GetChild(0).GetComponent<Animator>().SetTrigger("handsUp");
                break;
            case 1:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "taskStation";
                currentElement = Instantiate(taskStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 2:
                //Tracker is confusing Eggy with Output Station, long term fix is change Eggy tracker
                if (imageTrackingController.gameObject.GetComponent<AppManager>().currentAppState == AppManager.AppState.Eggy04RotatingLesson)
                {
                    imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "Eggy";
                    currentElement = Instantiate(eggy, transform.position, transform.rotation);
                    currentElement.GetComponent<TrackerEggy>().enabled = false;
                    currentElement.GetComponent<ITransition>().TurnOn();
                    currentElement.transform.parent = transform;
                }
                break;
            case 3:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "numberStation";
                currentElement = Instantiate(numberStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 4:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "shapeStation";
                currentElement = Instantiate(shapeStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 5:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "colorStation";
                currentElement = Instantiate(colorStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            case 6:
                if (imageTrackingController.gameObject.GetComponent<AppManager>().currentAppState == AppManager.AppState.Tutorial04GoblinAdd)
                {
                    imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "tutorialBit";
                    currentElement = Instantiate(tutorialInteractive, transform.position, transform.rotation);
                }
                else
                {
                    imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "leftBit";
                    currentElement = Instantiate(centerInteractive, transform.position, transform.rotation);
                }
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            case 8:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "rightBit";
                currentElement = Instantiate(centerInteractive, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            case 7:
                if (imageTrackingController.gameObject.GetComponent<AppManager>().currentAppState == AppManager.AppState.Eggy06InactiveTrackingLesson)
                {
                    currentElement = Instantiate(eggyInteractive, transform.position, transform.rotation);
                    imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "eggyBit";
                }
                else
                {
                    currentElement = Instantiate(centerInteractive, transform.position, transform.rotation);
                    imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "centerBit";
                }
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            default:
                imageTrackingController.gameObject.GetComponent<AppManager>().textLastSpawned.text = "colorStation";
                break;
        } 
    }

    public void Remove()
    {
        currentElement.GetComponent<ITransition>().TurnOff();
        Destroy(gameObject, 2f);
    }
}

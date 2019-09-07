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

    /// <summary>
    /// A world space UI to act as a three bit counter for number station.
    /// </summary>
    public GameObject[] ARBookPageElements;
    public GameObject eggy, eggyInteractive, tutorialInteractive, centerInteractive, tutorialStation, taskStation, numberStation, shapeStation, colorStation;

    public static TrackedImage[] imageDatabaseElement = new TrackedImage[7];
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
                currentElement = Instantiate(eggy, transform.position, transform.rotation);
                currentElement.GetComponent<TrackerEggy>().enabled = false;
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 1:
                currentElement = Instantiate(taskStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 2:
                currentElement = Instantiate(taskStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 3:
                currentElement = Instantiate(numberStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 4:
                if(imageTrackingController.gameObject.GetComponent<AppManager>().currentAppState == AppManager.AppState.InactiveTrackingLesson)
                    currentElement = Instantiate(eggyInteractive, transform.position, transform.rotation); //shapeStation
                else if(imageTrackingController.gameObject.GetComponent<AppManager>().currentAppState == AppManager.AppState.TutorialExercise01)
                    currentElement = Instantiate(tutorialInteractive, transform.position, transform.rotation); //shapeStation
                else currentElement = Instantiate(centerInteractive, transform.position, transform.rotation); //shapeStation
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
            case 5:
                currentElement = Instantiate(colorStation, transform.position, transform.rotation);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;

            case 7:
                currentElement = Instantiate(centerInteractive, transform.position, Quaternion.identity);
                currentElement.GetComponent<ITransition>().TurnOn();
                currentElement.transform.parent = transform;
                break;
        } 
    }

    public void Remove()
    {
        currentElement.GetComponent<ITransition>().TurnOff();
        Destroy(gameObject, 2f);
    }
}

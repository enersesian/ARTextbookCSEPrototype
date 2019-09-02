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
    private bool currentElementIsBitSetToOne = true;
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
            case 0:
                currentElement = Instantiate(tutorialStation, transform.position, Quaternion.identity);
                currentElement.transform.parent = transform;
                GetComponent<TrackerBase>().enabled = true;
                break;

            case 5:
                currentElement = Instantiate(leverStatus, transform.position, Quaternion.identity);
                currentElement.transform.parent = transform;
                GetComponent<TrackerInteractive>().enabled = true;
                break;
        }
        /*
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
        */    
    }

    public TrackerBase GetMainTracker()
    {
        return imageDatabaseElement[0].GetComponent<TrackerBase>();
    }

    public void SetBit(bool temp)
    {
        if(temp)//currentElementIsBitSetToOne)
        {
            currentElement.transform.GetChild(0).GetComponent<Text>().text = "0";
            currentElement.transform.GetChild(1).GetComponent<Text>().text = "Off";
            currentElementIsBitSetToOne = false;
        }
        else
        {
            currentElement.transform.GetChild(0).GetComponent<Text>().text = "1";
            currentElement.transform.GetChild(1).GetComponent<Text>().text = "On";
            currentElementIsBitSetToOne = true;
        }
    }

    public void InteractionNotice(bool isReady)
    {
        if(isReady)
        {
            currentElement.transform.GetChild(2).GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.3f);
        }
        else
        {
            currentElement.transform.GetChild(2).GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.3f);
        }
    }
}

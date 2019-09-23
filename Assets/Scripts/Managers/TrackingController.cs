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
public class TrackingController : Listener
{
    /// <summary>
    /// A prefab for visualizing an AugmentedImage.
    /// </summary>
    public TrackedImage TrackedImagePrefab;

    private Dictionary<int, TrackedImage> m_TrackedImages
        = new Dictionary<int, TrackedImage>();

    private List<AugmentedImage> m_TempAugmentedImages = new List<AugmentedImage>();

    private TrackedImage currentlyTracked;

    private bool[] isImageTracked = new bool[7];
    private bool shouldImageBeTracked, shouldControllerBeTracking, removeExistingContent, remove3DContent;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public new void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }

    public void ResetTracking()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<TrackedImage>().anchor = transform.GetChild(i).GetComponent<TrackedImage>().image.CreateAnchor(transform.GetChild(i).GetComponent<TrackedImage>().image.CenterPose);
            transform.GetChild(i).position = transform.GetChild(i).GetComponent<TrackedImage>().anchor.transform.position;
            transform.GetChild(i).rotation = transform.GetChild(i).GetComponent<TrackedImage>().anchor.transform.rotation;
        }
    }

    /// <summary>
    /// The Unity Update method.
    /// </summary>
    public void Update()
    {
        //rotate objects with finger swipe feature
        if (Input.GetMouseButton(0))
        {
            //printToScreen.text = Input.GetTouch(0).deltaPosition.x.ToString();
            for (int i = 0; i < transform.childCount; i++)
            {//make this check is its actively tracked first
                //its a model, not a UI, so go rotate it
                if (transform.GetChild(i).GetChild(0).gameObject.tag == "3DModel")
                {
                    transform.GetChild(i).rotation = Quaternion.Euler(0f, transform.GetChild(i).rotation.eulerAngles.y + (Input.GetTouch(0).deltaPosition.x/2f), 0f);
                }
            }
        }

        //two finger touch is the default touch interaction with the app
        //if (Input.GetMouseButtonDown(1)) gameManager.UserInputDetected();
        //got replaced with the continue button on the UI
        /*
        //Reset app to welcome screen
        if (Input.GetMouseButtonDown(2))
        {
            
            //shouldControllerBeTracking = false;
            //removeExistingContent = true;
            //remove3DContent = true;
            //gameManager.StartApp();
            //return;
            
            for(int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<TrackedImage>().anchor = transform.GetChild(i).GetComponent<TrackedImage>().image.CreateAnchor(transform.GetChild(i).GetComponent<TrackedImage>().image.CenterPose);
                transform.GetChild(i).position = transform.GetChild(i).GetComponent<TrackedImage>().anchor.transform.position;
                transform.GetChild(i).rotation = transform.GetChild(i).GetComponent<TrackedImage>().anchor.transform.rotation;
            }
        }
        */
        // Exit the app when the 'back' button is pressed.
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        if (Session.Status != SessionStatus.Tracking) Screen.sleepTimeout = SleepTimeout.SystemSetting;
        else Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.All);

        if(removeExistingContent)
        {
            foreach (var image in m_TempAugmentedImages)
            {
                currentlyTracked = null;
                m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
                if (currentlyTracked != null)
                {
                    //testing if we should remove 3D models at this point and if its a 3D model
                    if(currentlyTracked.transform.GetChild(0).gameObject.tag == "3DModel")
                    {
                        if(remove3DContent)
                        {
                            m_TrackedImages.Remove(image.DatabaseIndex);
                            currentlyTracked.Remove();
                        }
                    }
                    else
                    { //its a UI element and remove it
                        m_TrackedImages.Remove(image.DatabaseIndex);
                        currentlyTracked.Remove();
                    }
                }
            }
            removeExistingContent = false;
            remove3DContent = false;
        }
        else if (shouldControllerBeTracking)
        {
            if(Input.GetKeyDown(KeyCode.Alpha0))
            {
                shouldImageBeTracked = gameManager.InputDetected(0);
                if (shouldImageBeTracked)
                {
                    currentlyTracked = (TrackedImage)Instantiate(
                        TrackedImagePrefab, this.transform);
                    currentlyTracked.transform.parent = this.transform;
                    m_TrackedImages.Add(0, currentlyTracked);
                    shouldImageBeTracked = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                shouldImageBeTracked = gameManager.InputDetected(7);
                if (shouldImageBeTracked)
                {
                    currentlyTracked = (TrackedImage)Instantiate(
                        TrackedImagePrefab, this.transform);
                    currentlyTracked.transform.parent = this.transform;
                    m_TrackedImages.Add(7, currentlyTracked);
                    shouldImageBeTracked = false;
                }
            }

            // Create an anchor and a model for updated augmented images that are tracking and do
            // not previously have a model. Remove models for stopped images.
            foreach (var image in m_TempAugmentedImages)
            {
                currentlyTracked = null;
                m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
                if (image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && currentlyTracked == null) //image.TrackingState == TrackingState.Tracking && 
                {
                    //need to switch this to know the databaseElement right away if thats the one that should be spawned
                    shouldImageBeTracked = gameManager.InputDetected(image.DatabaseIndex);
                    if (shouldImageBeTracked)
                    {
                        // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                        Anchor anchor = image.CreateAnchor(image.CenterPose);
                        currentlyTracked = (TrackedImage)Instantiate(
                            TrackedImagePrefab, anchor.transform);
                        currentlyTracked.image = image;
                        currentlyTracked.anchor = anchor;
                        //currentlyTracked.transform.rotation = Quaternion.identity;
                        currentlyTracked.transform.parent = this.transform;
                        m_TrackedImages.Add(image.DatabaseIndex, currentlyTracked);
                        shouldImageBeTracked = false;
                    }
                }
                else if (image.TrackingState == TrackingState.Stopped && currentlyTracked != null)
                {
                    m_TrackedImages.Remove(image.DatabaseIndex);
                    GameObject.Destroy(currentlyTracked.gameObject);
                }
            }
        }
    }

    private void RemoveExistingContent()
    {
        foreach (var image in m_TempAugmentedImages)
        {
            currentlyTracked = null;
            m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
            if (currentlyTracked != null)
            {
                m_TrackedImages.Remove(image.DatabaseIndex);
                currentlyTracked.Remove();
            }
        }
    }

    public override void SetListenerState()
    {
        switch (gameManager.currentAppState)
        {
            //add all shape station cases

            case AppManager.AppState.Eggy01Welcome: //lesson on using two finger tap to move through app states
            case AppManager.AppState.Eggy02ResetInstructions: //lesson on using three finger tap to reset app due to poor tracking
            case AppManager.AppState.Eggy04RotatingLesson: //lesson on how to rotate trackers to align them with the images
            case AppManager.AppState.Number02FirstBitExplaination:
            case AppManager.AppState.Number03SecondBitExplaination:
            case AppManager.AppState.Number04ThirdBitExplaination:
            case AppManager.AppState.Number05SugarGoblinIntro:
            case AppManager.AppState.Number06FirstExercise:
            case AppManager.AppState.Number07SecondExercise:
            case AppManager.AppState.Number08ThirdExercise:
            case AppManager.AppState.Number09FourthExercise:
            default:
                shouldControllerBeTracking = false;
                break;

            case AppManager.AppState.Tutorial02BitScanning: //lesson on how to find the tutorial interaction
                ResetTracking(); //can scan tutorial station while user is flipping page and it jumps all over the place so reset its tracking
                shouldControllerBeTracking = false;
                break;

            case AppManager.AppState.Number02FirstBitScanning:
            case AppManager.AppState.Number03SecondBitScanning:
            case AppManager.AppState.Number04ThirdBitScanning:
                ResetTracking(); 
                Invoke("TurnOnTracking", 5f);
                break;

            case AppManager.AppState.Eggy07TrackingExercise: //lesson on how finding and tracking interactives
                transform.GetChild(0).GetChild(0).GetComponent<TrackerEggy>().enabled = true;
                shouldControllerBeTracking = false;
                break;

            case AppManager.AppState.Eggy03ScanningLesson: //lesson on how to recognize scannable images
                Invoke("TurnOnTracking", 1f); //Since eggy is a poor tracker give more time to finding him
                break;

            case AppManager.AppState.Eggy05ActiveTrackingLesson: //lesson on how finding and tracking interactives
            case AppManager.AppState.Tutorial03BitExplanation:
                Invoke("TurnOnTracking", 5f);
                break;

            case AppManager.AppState.Tutorial01StationScanning: //lesson on how to find the tutorial station
            case AppManager.AppState.Number01StationScanning:
            case AppManager.AppState.Shape01StationScanning:
                //removeExistingContent = true;
                //remove3DContent = true;

                shouldControllerBeTracking = false;
                Invoke("DelayRemoveContent", 3f);
                Invoke("TurnOnTracking", 6f);
                break;

            //dont need to be scanning for new targets
            case AppManager.AppState.Eggy06InactiveTrackingLesson: //lesson on how finding and tracking interactives
                ResetTracking();
                break;
            case AppManager.AppState.Tutorial04GoblinAdd: //lesson on how to turn a bit on with a piece of candy
            case AppManager.AppState.Tutorial05CurrentStateExplanation:
            case AppManager.AppState.Tutorial06GoblinRemove:
            case AppManager.AppState.Tutorial07GoblinPractice: //confirm moving from tutorial to task station
                break;
        }
    }

    private void TurnOnTracking()
    {
        shouldControllerBeTracking = true;
    }

    private void DelayRemoveContent()
    {
        removeExistingContent = true;
        remove3DContent = true;
    }
}

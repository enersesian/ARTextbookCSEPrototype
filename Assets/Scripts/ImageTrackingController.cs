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

    public Text printToScreenTop, printToScreenBottom;

    private bool[] isImageTracked = new bool[7];
    private bool shouldImageBeTracked;
    private int resetCounter;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public void Awake()
    {
        // Enable ARCore to target 60fps camera capture frame rate on supported devices.
        // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
        Application.targetFrameRate = 60;
    }

    private void OnEnable()
    {
        IntroText();
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
                //printToScreenTop.text = "Entered tracking loop";
                if (!isImageTracked[0] && !isImageTracked[1] && !isImageTracked[2] && !isImageTracked[3]) //Haven't found a station yet to start AR page experience
                {
                    //printToScreenBottom.text = "no stations found yet " + image.DatabaseIndex.ToString();
                    if (image.DatabaseIndex < 4) //Found a station icon to start an AR page experience
                    {
                        switch (image.DatabaseIndex)
                        {
                            case 0:
                                printToScreenTop.text = "Welcome to the Task Station Page";
                                break;
                            case 1:
                                printToScreenTop.text = "Welcome to the Number Station Page";
                                break;
                            case 2:
                                printToScreenTop.text = "Welcome to the Shape Station Page";
                                break;
                            case 3:
                                printToScreenTop.text = "Welcome to the Color Station Page";
                                break;
                        }
                        printToScreenTop.text += ", swipe finger to rotate station\n";
                        printToScreenBottom.text = "When done viewing station model, please scan right page to start AR lesson";
                        isImageTracked[image.DatabaseIndex] = true;
                        shouldImageBeTracked = true;
                    }
                }
                else if (image.DatabaseIndex >= 4) //Station selected and found a cart icon
                {
                    if (isImageTracked[0]) //Task Station icon already found so ready to start lesson, need center cart
                    {
                        if (image.DatabaseIndex == 5) //Center cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                            printToScreenTop.text = "Welcome to the tutorial on binary math. Our candy cart is empty and providing no power to the station.\n";
                            printToScreenBottom.text = "Please place a candy in the cart to turn it on and give power to the tutorial station.";
                            //start tutorial lesson
                        }
                    }
                    if (isImageTracked[1]) //Number Station icon already found so ready to start lesson, need all three carts
                    {
                        if (image.DatabaseIndex == 4) //Left cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }
                        if (image.DatabaseIndex == 5) //Center cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }
                        if (image.DatabaseIndex == 6) //Right cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }

                        if (isImageTracked[4] && isImageTracked[5] && isImageTracked[6])
                        {
                            //start number lesson
                        }
                        else
                        {
                            //print out that they need to scan more carts to start the lesson
                        }
                    }
                    if (isImageTracked[2]) //Shape Station icon already found so ready to start lesson, need left and right carts
                    {
                        if (image.DatabaseIndex == 4) //Left cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }
                        if (image.DatabaseIndex == 6) //Right cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }

                        if (isImageTracked[4] && isImageTracked[6])
                        {
                            //start shape lesson
                        }
                        else
                        {
                            //print out that they need to scan more carts to start the lesson
                        }
                    }
                    if (isImageTracked[3]) //Color Station icon already found so ready to start lesson, need all three carts
                    {
                        if (image.DatabaseIndex == 4) //Left cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }
                        if (image.DatabaseIndex == 5) //Center cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }
                        if (image.DatabaseIndex == 6) //Right cart found
                        {
                            isImageTracked[image.DatabaseIndex] = true;
                            shouldImageBeTracked = true;
                        }

                        if (isImageTracked[4] && isImageTracked[5] && isImageTracked[6])
                        {
                            //start color lesson
                        }
                        else
                        {
                            //print out that they need to scan more carts to start the lesson
                        }
                    }
                }

                if(shouldImageBeTracked)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    currentlyTracked = (TrackedImage)Instantiate(
                        TrackedImagePrefab, anchor.transform);
                    currentlyTracked.image = image;
                    currentlyTracked.transform.rotation = Quaternion.identity;
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

        //User touched screen to reset tracking if issues or turning page in book
        if (Input.GetMouseButtonDown(1))
        {
            for (int i = 0; i < isImageTracked.Length; i++)
            {
                if (isImageTracked[i]) resetCounter++; //if nothing is tracking then resetCounter == 0 and we reset app
            }
            if(resetCounter > 0) //we are tracking something so go to app intro not reset app
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
                IntroText();
                for (int i = 0; i < isImageTracked.Length; i++) isImageTracked[i] = false;
                resetCounter = 0;
            }
            else //we are not tracking anything so reset app
            {
                GetComponent<StartExperience>().enabled = true;
                printToScreenTop.text = "Welcome to C-Spresso, an augmented reality textbook";
                printToScreenBottom.text = "Please tap two fingers on the screen to begin image scanning";
                FitToScanOverlay.SetActive(false);
                this.enabled = false;
                return;
            }
            
        }

        //rotate objects with finger swipe feature
        if(Input.GetMouseButton(0))
        {
            //printToScreen.text = Input.GetTouch(0).deltaPosition.x.ToString();
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
            /*
            if (potentallyTrackedImages.image.DatabaseIndex < 4) //found a station, present 3d model of station
            {
                if (potentallyTrackedImages.image.TrackingState == TrackingState.Tracking)
                {
                    if (potentallyTrackedImages.image.DatabaseIndex == 0) printToScreen.text = "Welcome to the Color Station Page\n";
                    if (potentallyTrackedImages.image.DatabaseIndex == 1) printToScreen.text = "Welcome to the Number Station Page\n";
                    if (potentallyTrackedImages.image.DatabaseIndex == 2) printToScreen.text = "Welcome to the Shape Station Page\n";
                    if (potentallyTrackedImages.image.DatabaseIndex == 3) printToScreen.text = "Welcome to the Tutorial Station Page\n";
                    FitToScanOverlay.SetActive(false);
                    return;
                }
            }
            */
        }

        FitToScanOverlay.SetActive(true);
    }

    private void IntroText()
    {
        printToScreenTop.text = "Welcome to the AR experience of the Binary Math Lesson";
        printToScreenBottom.text = "Please scan the station icon on the left page to start";
    }
}

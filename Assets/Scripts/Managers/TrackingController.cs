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
    private bool shouldImageBeTracked, shouldControllerBeTracking;

    /// <summary>
    /// The Unity Awake() method.
    /// </summary>
    public new void Awake()
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
        if (Input.GetKey(KeyCode.Escape)) Application.Quit();

        //Reset app to welcome screen
        if (Input.GetMouseButtonDown(2))
        {
            shouldControllerBeTracking = false;
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
            gameManager.StartApp();
            return;
        }

        //rotate objects with finger swipe feature
        if (Input.GetMouseButton(0))
        {
            //printToScreen.text = Input.GetTouch(0).deltaPosition.x.ToString();
            for (int i = 0; i < transform.childCount; i++)
            {//make this check is its actively tracked first
                transform.GetChild(i).rotation = Quaternion.Euler(0f, transform.GetChild(i).rotation.eulerAngles.y + Input.GetTouch(0).deltaPosition.x, 0f);
            }
        }

        if (Input.GetMouseButtonDown(1)) gameManager.UserInputDetected();
        if (Session.Status != SessionStatus.Tracking) Screen.sleepTimeout = SleepTimeout.SystemSetting;
        else Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // Get updated augmented images for this frame.
        Session.GetTrackables<AugmentedImage>(m_TempAugmentedImages, TrackableQueryFilter.All);

        // Create an anchor and a model for updated augmented images that are tracking and do
        // not previously have a model. Remove models for stopped images.
        foreach (var image in m_TempAugmentedImages)
        {
            currentlyTracked = null;
            m_TrackedImages.TryGetValue(image.DatabaseIndex, out currentlyTracked);
            if (image.TrackingMethod == AugmentedImageTrackingMethod.FullTracking && currentlyTracked == null) //image.TrackingState == TrackingState.Tracking && 
            {
                shouldImageBeTracked = gameManager.InputDetected(image.DatabaseIndex);
                if (shouldImageBeTracked)
                {
                    // Create an anchor to ensure that ARCore keeps tracking this augmented image.
                    Anchor anchor = image.CreateAnchor(image.CenterPose);
                    currentlyTracked = (TrackedImage)Instantiate(
                        TrackedImagePrefab, anchor.transform);
                    currentlyTracked.image = image;
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

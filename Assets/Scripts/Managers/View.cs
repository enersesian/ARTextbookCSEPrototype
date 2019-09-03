using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : Listener
{
    public Text printToScreenTop, printToScreenBottom;

    /// <summary>
    /// The overlay containing the fit to scan user guide.
    /// </summary>
    public Transition_UI FitToScanOverlay;

    public override void SetListenerState()
    {
        switch(gameManager.currentAppState)
        {
            case AppManager.AppState.Welcome: //lesson on using two finger tap to move through app states
                FitToScanOverlay.TurnOff();
                UpdateText("Welcome to C-Spresso, an augmented reality textbook on computer science.", true);
                UpdateText("Please tap two fingers on screen to proceed and start your binary math lesson.", false);
                break;

            case AppManager.AppState.ScanningLesson: //lesson on how to recognize scannable images
                UpdateText("Find the Eggy image that says trackable and hold your phone close to the image.", true);
                UpdateText("Please tap two fingers on the screen to begin scanning for image targets.", false);
                break;

            case AppManager.AppState.ScanningExercise: //exercise on scanning images
                //UpdateText("Find page that has an image that says trackable", true);
                //UpdateText("Please tap two fingers on the screen to begin scanning for image targets", false);
                FitToScanOverlay.TurnOn();
                break;

            case AppManager.AppState.RotatingLesson: //lesson on how to rotate trackers to align them with the images
                FitToScanOverlay.TurnOff();
                UpdateText("Hi, Im Eggy. Its fun being spawned into your world, but I can get turned around in the process.", true);
                UpdateText("Please swipe with one finger to rotate Eggy toward you. When done, please tap with two fingers.", false);
                break;

            case AppManager.AppState.ActiveTrackingLesson: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOn();
                UpdateText("Ah, that's better. Make sure to always rotate new objects to align them with you and the page.", true);
                UpdateText("Notice the scanner is active again. Please scan the image on the right page to activate it.", false);
                break;

            case AppManager.AppState.InactiveTrackingLesson: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOff();
                UpdateText("Great! Notice that the image turned green. That means it can be interacted with.", true);
                UpdateText("Now move away from the image until it turns red. That means you are too far to interact with it.", false);
                break;

            case AppManager.AppState.TrackingExercise: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOff();
                UpdateText("Great! Now practice moving back and forth to make the image interactable and not interactable.", true);
                UpdateText("You will need a good feel for these distances. When done, please tap with two fingers.", false);
                break;

            case AppManager.AppState.TutorialStationScanning: //lesson on how to find the tutorial station
                FitToScanOverlay.TurnOn();
                UpdateText("Great! Now let's go find your first station, the tutorial station and learn about a bit.", true);
                UpdateText("Please go find the tutorial station and scan it to start its lesson.", false);
                break;

            case AppManager.AppState.TutorialInteractiveScanning: //lesson on how to find the tutorial interaction
                FitToScanOverlay.TurnOff();
                UpdateText("Great! Remember to rotate it toward you, then move to the right page.", true);
                UpdateText("Please scan the interactive image on the right page to begin exercise.", false);
                break;

            case AppManager.AppState.TutorialExercise01: //lesson on how to turn a bit on with a piece of candy
                UpdateText("Great! Remember to rotate it so it aligns with the page. A bit is the basic unit for a computer.", true);
                UpdateText("It can be either 0 or 1. Let's get close enough to interact with it and place a candy on the image.", false);
                break;

            case AppManager.AppState.TutorialExercise02: //lesson on how to turn a bit off with a piece of candy
                UpdateText("Great! You just changed the bit from 0 to 1. The two states of a bit can be used to represent many things.", true);
                UpdateText("Initially we use a bit to indicate on and off. Remove the candy to put the bit back to 0.", false);
                break;

            case AppManager.AppState.TutorialExercise03: //confirm moving from tutorial to task station
                UpdateText("Great! You successfully flipped one bit from its zero position to its one position and back again.", true);
                UpdateText("Now that you know how to interact with a bit, tap your two fingers to move on to the next lesson.", false);
                break;

            case AppManager.AppState.TaskStationScanning: //lesson on how to find the task station
                FitToScanOverlay.TurnOn();
                UpdateText("Let's turn the page and move to the task station. There you will learn to interact with two bits.", true);
                UpdateText("One bit will turn the station on and the next bit will give you a task.", false);
                break;

            /*
                        case AppManager.AppState.InteractiveScanning:
                            switch (gameManager.currentStation)
                            {
                                case AppManager.ActiveStation.None:
                                    break;

                                case AppManager.ActiveStation.Task:
                                    UpdateText("Welcome to the Task Station Page, swipe finger to rotate station", true);
                                    break;

                                case AppManager.ActiveStation.Number:
                                    UpdateText("Welcome to the Number Station Page, swipe finger to rotate station", true);
                                    break;

                                case AppManager.ActiveStation.Shape:
                                    UpdateText("Welcome to the Shape Station Page, swipe finger to rotate station", true);
                                    break;

                                case AppManager.ActiveStation.Color:
                                    UpdateText("Welcome to the Color Station Page, swipe finger to rotate station", true);
                                    break;
                            }
                            UpdateText("When done viewing station model, please scan right page to start AR lesson", false);
                            break;

                        case AppManager.AppState.Lesson:
                            break;
            */
            default:
                break;
        }
    }

    public void UpdateText(string newText, bool isTop)
    {
        StartCoroutine(TextTransition(newText, isTop));
    }

    private IEnumerator TextTransition(string newText, bool isTop)
    {
        float elapsedTime = 0f, waitTime = 1f;
        Text activeText;
        if (isTop) activeText = printToScreenTop;
        else activeText = printToScreenBottom;

        while (elapsedTime < waitTime)
        {
            activeText.color = new Color(activeText.color.r, activeText.color.g, activeText.color.b, Mathf.Lerp(1f, 0f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        activeText.text = newText;
        elapsedTime = 0f;

        while (elapsedTime < waitTime)
        {
            activeText.color = new Color(activeText.color.r, activeText.color.g, activeText.color.b, Mathf.Lerp(0f, 1f, (elapsedTime / waitTime)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return null;
    }
}

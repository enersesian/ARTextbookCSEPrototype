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
                UpdateText("Please touch two fingers to the screen to begin learning how to use the app.", false);
                break;

            case AppManager.AppState.ResetInstructions: //lesson on using two finger tap to move through app states
                FitToScanOverlay.TurnOff();
                UpdateText("Please note if tracking gets bad, you can reset the app with a three finger touch.", true);
                UpdateText("When ready, touch two fingers to the screen to start scanning your first image!", false);
                break;

            case AppManager.AppState.ScanningLesson: //lesson on how to recognize scannable images
                UpdateText("Notice the scanning frame has appeared and it tells you that the phone is actively scanning.", true);
                UpdateText("Now find the Eggy image and align his image's frame with your scanning frame.", false);
                FitToScanOverlay.TurnOn(4f);
                break;

            case AppManager.AppState.RotatingLesson: //lesson on how to rotate trackers to align them with the images
                FitToScanOverlay.TurnOff();
                UpdateText("Hi, Im Eggy! Its fun being spawned into your world, and it's even funnier to spin me around!", true);
                UpdateText("Please swipe with one finger to rotate me! When done, please tap with two fingers to continue.", false);
                break;

            case AppManager.AppState.ActiveTrackingLesson: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOn(4f);
                UpdateText("Ah, that was fun! Remember, you can rotate any 3D model to enjoy it from any angle.", true);
                UpdateText("Now notice the scanner is active again. Please scan the image below me to activate it.", false);
                break;

            case AppManager.AppState.InactiveTrackingLesson: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOff();
                UpdateText("Great! Notice that the image is glowing green. That means you are tracking it and can interact with it.", true);
                UpdateText("Now pull back from the image until it grows red. That means you are too far to interact with it.", false);
                break;

            case AppManager.AppState.TrackingExercise: //lesson on how finding and tracking interactives
                FitToScanOverlay.TurnOff();
                UpdateText("Now try tracking both of us. I will glow green and red now to let you know when you are tracking me.", true);
                UpdateText("Now practice moving back and forth to get both of us green. When done, please tap with two fingers.", false);
                break;

            case AppManager.AppState.TutorialStationScanning: //lesson on how to find the tutorial station
                FitToScanOverlay.TurnOn(4f);
                UpdateText("That image is a bit, which is a counting unit for computers, like digits are counting units for humans.", true);
                UpdateText("When you are ready to learn more about bits, go scan the tutorial station on the right page.", false);
                break;

            case AppManager.AppState.TutorialInteractiveScanning: //lesson on how to find the tutorial interaction
                FitToScanOverlay.TurnOff();
                UpdateText("This is the tutorial station. Remember you can rotate it for viewing. I am also here to show you if its tracking.", true);
                UpdateText("Remember to make sure I am green when trying to interact with a bit. Tap two fingers to continue.", false);
                break;

            case AppManager.AppState.TutorialExplanation: //lesson on how to find the tutorial interaction
                FitToScanOverlay.TurnOn(4f);
                UpdateText("When a bit is green it is turned on, and when a bit is red it is turned off. Lets turn turning a bit on and off.", true);
                UpdateText("When ready, scan the image below it to begin the exercise. You will use a cookie to turn the bit on and off.", false);
                break;

            case AppManager.AppState.TutorialExercise01: //lesson on how to turn a bit on with a piece of candy
                FitToScanOverlay.TurnOff();
                UpdateText("Notice above the colored interactive area now sits a zero. That is a bit, and it is currently set to 0.", true);
                UpdateText("Bits can be set to 0 or 1, which is only two states. Let's change the bit's state by placing a cookie on it.", false); // //Digits can be set to one of ten states: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9.
                break;

            case AppManager.AppState.TutorialExercise02: //lesson on how to turn a bit off with a piece of candy
                UpdateText("Great! You just changed the bit from 0 to 1. The two states of a bit can be used to represent many things.", true);
                UpdateText("Here we are turning the station on or off. Watch the station turn off as you remove the candy to put the bit back to 0.", false);
                break;

            case AppManager.AppState.TutorialExercise03: //confirm moving from tutorial to task station
                UpdateText("Great! You successfully flipped a bit from 0 to 1 and back again. Try turning the station back on.", true);
                UpdateText("When you are comfortable with interacting with a bit, tap two fingers to move on to the next lesson.", false);
                break;
                //Get to here for tonight, build the on/off into the tutorial station
            case AppManager.AppState.TaskStationScanning: //lesson on how to find the task station
                FitToScanOverlay.TurnOn(4f);
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

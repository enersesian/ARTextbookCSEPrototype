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

    public Transition_UI trackingStatusIconTransition, trackingStatusGreenHighlightTransition;
    public Button trackingResetButton, continueInstructionButton, submitAnswerButton;
    private enum TrackingStatusState { NotSearchingAndNotTracking, SearchingForATrackableNew, SearchingForATrackableContinued, TrackedAtLeastOneTrackable };
    private TrackingStatusState currentTrackingStatusState;
    public Text objectiveStatusText, currentStatusText;
    private bool canSetCurrentStatusText, canSetObjectiveStatusText;

    private void Start()
    {
        currentTrackingStatusState = TrackingStatusState.NotSearchingAndNotTracking;
        SetRightSideUIToDefault();
    }

    private void TrackingStatusUpdate(TrackingStatusState nextTrackingStatusIconState)
    {
        //Turn off tracking from any state
        if(nextTrackingStatusIconState == TrackingStatusState.NotSearchingAndNotTracking)
        {
            currentTrackingStatusState = TrackingStatusState.NotSearchingAndNotTracking;
            trackingStatusIconTransition.repeaterForTrackingStatus = false;
            trackingStatusIconTransition.TurnOff();
            trackingStatusGreenHighlightTransition.TurnOff();
            trackingResetButton.interactable = false;
        }

        //Turn on searching from nothing state
        if(nextTrackingStatusIconState == TrackingStatusState.SearchingForATrackableNew)
        {
            currentTrackingStatusState = TrackingStatusState.SearchingForATrackableNew;
            trackingStatusIconTransition.repeaterForTrackingStatus = true;
            trackingStatusIconTransition.TurnOn();
            trackingStatusGreenHighlightTransition.TurnOff();
            trackingResetButton.interactable = false;

            //invoke turn on tracking reset button after x seconds
        }

        //Turn on searching again while tracking
        if (nextTrackingStatusIconState == TrackingStatusState.SearchingForATrackableContinued)
        {
            currentTrackingStatusState = TrackingStatusState.SearchingForATrackableContinued;
            trackingStatusIconTransition.repeaterForTrackingStatus = true;
            trackingStatusIconTransition.TurnOn();
            //trackingStatusGreenHighlightTransition.TurnOn();

            //invoke turn on tracking reset button after x seconds
        }

        //Tracking at least one image
        if (nextTrackingStatusIconState == TrackingStatusState.TrackedAtLeastOneTrackable)
        {
            currentTrackingStatusState = TrackingStatusState.TrackedAtLeastOneTrackable;
            trackingStatusIconTransition.repeaterForTrackingStatus = false;
            trackingStatusIconTransition.TurnOff();
            trackingStatusGreenHighlightTransition.TurnOn();
            trackingResetButton.interactable = true;
        }
    }

    public override void SetListenerState()
    {
        switch(gameManager.currentAppState)
        {
            case AppManager.AppState.Eggy01Welcome: //lesson on using two finger tap to move through app states
                UpdateText("Welcome to C-Spresso, an augmented reality textbook on computer science.", true);
                UpdateText("Please tap the continue button on the bottom left to how to use the app.", false);
                continueInstructionButton.interactable = true;
                break;

            case AppManager.AppState.Eggy02ResetInstructions: //lesson on using two finger tap to move through app states
                UpdateText("Please note if tracking gets bad, tap the reset tracking button on the middle left.", true);
                UpdateText("When ready, tap continue to start scanning your first image!", false);
                break;

            case AppManager.AppState.Eggy03ScanningLesson: //lesson on how to recognize scannable images
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew);
                continueInstructionButton.interactable = false;
                UpdateText("Notice the scanning icon on top left is blinking, that tells you that the phone is actively scanning.", true);
                UpdateText("Move phone close to Eggy to scan. If Eggy is not scanning, try moving the phone back and forth.", false);
                break;

            case AppManager.AppState.Eggy04RotatingLesson: //lesson on how to rotate trackers to align them with the images
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Hi, Im Eggy! Its fun being spawned into your world, and it's even funnier to spin me around!", true);
                UpdateText("Swipe with one finger to rotate me! When done, tap continue.", false);
                break;

            case AppManager.AppState.Eggy05ActiveTrackingLesson: //lesson on how finding and tracking interactives
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("Ah, that was fun! Remember, you can rotate any 3D model to enjoy it from any angle.", true);
                UpdateText("Scanner will activate now. Try scanning the candy icon below me to activate it.", false);
                break;

            case AppManager.AppState.Eggy06InactiveTrackingLesson: //lesson on how finding and tracking interactives
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable); //2 trackables
                continueInstructionButton.interactable = true;
                UpdateText("Notice that the candy is glowing green. That means its tracked and soon you can interact with it.", true);
                UpdateText("If its red, wait for it to turn green or try moving the phone around. When ready, tap continue.", false);
                break;

            case AppManager.AppState.Eggy07TrackingExercise: //lesson on how finding and tracking interactives
                continueInstructionButton.interactable = true;
                UpdateText("Frame up my image so that you can track me along with the candy. My belt will glow green when I'm tracked.", true);
                UpdateText("Now practice getting both of the candy and me tracked well and glowing green. When ready, please tap continue.", false);
                break;

            case AppManager.AppState.Tutorial01StationScanning: //lesson on how to find the tutorial station
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew); //maybe invoke in 5 seconds to line up with removing 3d content
                continueInstructionButton.interactable = false;
                UpdateText("The candy represents a bit, a counting unit for computers, like fingers and digits for humans.", true);
                UpdateText("When you are ready to learn about bits, turn the page and scan the tutorial station. I'll meet you there.", false);
                break;

            case AppManager.AppState.Tutorial02BitScanning: //lesson on how to find the tutorial interaction
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable); //1 trackable
                continueInstructionButton.interactable = true;
                UpdateText("This is the tutorial station. Remember you can rotate it for viewing. If I am green, it is tracking.", true);
                UpdateText("Always make sure my belt is glowing green when trying to interact with the bits. Tap continue.", false);
                break;

            case AppManager.AppState.Tutorial03BitExplanation: //lesson on how to find the tutorial interaction
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("When the bit is green it is on, and when the bit is red it is off. Lets try turning a bit on and off.", true);
                UpdateText("Try scanning the candy below to turn it on, then I will show you how to use a sugar goblin to turn it off.", false);
                break;

            case AppManager.AppState.Tutorial04GoblinAdd: //lesson on how to turn a bit off with a piece of candy
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable); //2 trackables
                UpdateText("Notice in the colored cookie area now sits a 1. That is a bit, and it is currently set to state 1.", true);
                UpdateText("Bits can be set to only two states, 0 or 1. Let's change the bit by putting a sugar goblin on it.", false); // //Digits can be set to one of ten states: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9.
                break;

            case AppManager.AppState.Tutorial05CurrentStateExplanation:
                continueInstructionButton.interactable = true;
                UpdateText("Great! You just changed the bit from 1 to 0. The two states of a bit can be used to represent many things.", true);
                UpdateText("Here it's turning this station on and off. I'm going to track your progress over to the right. Tap continue.", false);
                currentStatusText.text = "Off";
                canSetCurrentStatusText = true;
                objectiveStatusText.text = "";
                break;

            case AppManager.AppState.Tutorial06GoblinRemove: //lesson on how to turn a bit on with a piece of candy
                continueInstructionButton.interactable = false;
                UpdateText("I'm going to give you a goal in the top right. You just turned the station off and I want you to turn it on.", true);
                UpdateText("Remove the goblin to turn station on. When your current matches goal, click submit button in bottom right.", false);
                ActivateGoalStatusText("On");
                break;
                
            case AppManager.AppState.Tutorial07GoblinPractice: //confirm moving from tutorial to task station
                submitAnswerButton.interactable = false;
                continueInstructionButton.interactable = true;
                UpdateText("Great! You successfully set bits to their 0 and 1 states, and can turn the station on and off in the process.", true);
                UpdateText("Try turning the station on and off again to get a feel for interacting with a bit. When ready tap continue.", false);
                objectiveStatusText.text = "None";
                break;

            case AppManager.AppState.Number01StationScanning: //lesson on how to find the task station
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew);
                continueInstructionButton.interactable = false;
                SetRightSideUIToDefault();
                UpdateText("Let's turn the page and move to the number station. There you will learn to interact with a series of bits.", true);
                UpdateText("Bits can be put together to represent larger numbers. The more bits, the more numbers you can represent.", false);
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

    private void SetRightSideUIToDefault()
    {
        canSetCurrentStatusText = false;
        canSetObjectiveStatusText = false;
        submitAnswerButton.interactable = false;
        objectiveStatusText.text = "";
        objectiveStatusText.fontSize = 100;
        currentStatusText.text = "";
        currentStatusText.fontSize = 100;
    }

    private void ActivateCurrentStatusText(string currentStatus)
    {
        currentStatusText.text = currentStatus;
        canSetCurrentStatusText = true;
    }

    private void ActivateGoalStatusText(string currentObjective)
    {
        objectiveStatusText.text = currentObjective;
        canSetObjectiveStatusText = true;
    }

    public void SetCurrentStatusText(string text)
    {
        if(canSetCurrentStatusText)
        {
            currentStatusText.text = text;
            SetSubmitButtonStatus();
        }
    }

    private void SetSubmitButtonStatus()
    {
        if (currentStatusText.text == objectiveStatusText.text) submitAnswerButton.interactable = true;
        else submitAnswerButton.interactable = false;
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

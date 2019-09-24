﻿using System.Collections;
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
                UpdateText("Frame up on my image so you can track me with the candy. My belt will glow green when I'm tracked.", true);
                UpdateText("Now practice getting both the candy and me tracked and glowing green. When ready, please tap continue.", false);
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
                objectiveStatusText.text = "";
                break;

            case AppManager.AppState.Number01StationScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew);
                continueInstructionButton.interactable = false;
                SetRightSideUIToDefault();
                UpdateText("Let's turn the page and move to the number station. There you will learn to interact with a series of bits.", true);
                UpdateText("Bits can be put together to represent larger numbers. More bits, the larger numbers you can represent.", false);
                break;

            case AppManager.AppState.Number02FirstBitExplaination: 
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                canSetCurrentStatusText = true;
                UpdateText("Here we have three bits. Each bit represents a power of two, which added together can represent 0-7.", true);
                UpdateText("The coffee bit on right represents 2^0, which is 1, when it is turned on. Press continue to scan bit.", false);
                break;

            case AppManager.AppState.Number02FirstBitScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("Scanning for coffee bit on right...please zoom in to the coffee bit if not scanning.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Number03SecondBitExplaination:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Now the 1 bit is on, so our equation is 0+0+1 = 1. With one bit active, we can represent the numbers 0-1.", true);
                UpdateText("Try using the goblin to turn the 1 bit on and off to see the results. Press continue to scan the 2 bit.", false);
                break;

            case AppManager.AppState.Number03SecondBitScanning: 
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("The cookie bit represents 2^1, which is 2, when it is turned on. It can represent 0 or 2.", true);
                UpdateText("Scanning for cookie bit in middle...please zoom in to the cookie bit if not scanning.", false);
                break;

            case AppManager.AppState.Number04ThirdBitExplaination: 
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Now the 2 bit is on, so our equation is 0+2+1 = 3. With two bits active, we can represent the numbers 0-3.", true);
                UpdateText("Try using the goblin to turn the 2 bit on and off to see the results. Press continue to scan the 4 bit.", false);
                break;

            case AppManager.AppState.Number04ThirdBitScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("The candy bit represents 2^2, which is 4, when it is turned on. It can represent 0 or 4. ", true);
                UpdateText("Scanning for candy bit on left...please zoom in to the candy bit if not scanning.", false);
                break;

            case AppManager.AppState.Number05SugarGoblinIntro:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Now the 4 bit is on, so our equation is 4+2+1 = 7. With three bits active, we can represent the numbers 0-7.", true);
                UpdateText("Try turning some of the bits off with the sugar goblins to see how the equation changes. Tap continue.", false);
                break;

            case AppManager.AppState.Number06FirstExercise: 
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = false;
                objectiveStatusText.text = "2";
                UpdateText("I need you to solve a series of exercises to fix the number station. Use the recipe book if you need help.", true);
                UpdateText("The first goal is give me a 2. When you have the correct bits turned on, click the submit button.", false);
                break;

            case AppManager.AppState.Number07SecondExercise:
                submitAnswerButton.interactable = false;
                objectiveStatusText.text = "5";
                UpdateText("Great job! Now give me a 5. When you have the correct bits turned on, click the submit button.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Number08ThirdExercise:
                submitAnswerButton.interactable = false;
                objectiveStatusText.text = "3";
                UpdateText("Great job! Now give me a 3. When you have the correct bits turned on, click the submit button.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Number09FourthExercise:
                submitAnswerButton.interactable = false;
                objectiveStatusText.text = "6";
                UpdateText("Great job! Now give me a 6. When you have the correct bits turned on, click the submit button.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Shape01StationScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew);
                continueInstructionButton.interactable = false;
                submitAnswerButton.interactable = false;
                SetRightSideUIToDefault();
                UpdateText("You fixed it! Let's turn the page and move to the shape station. You will learn how to use bits as lists.", true);
                UpdateText("Bits can be put together to represent lists of objects you want the computer to remember for you.", false);
                break;

            case AppManager.AppState.Shape02StationExplaination:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Here we have two bits. We learned that two bits can represent 0-3, so we can make a list of four objects.", true);
                UpdateText("Notice on the shape station we have a list of four shapes. Let's learn how to use the bits. Press continue.", false);
                break;

            case AppManager.AppState.Shape03FirstBitExplaination:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("The list is a combination of two bits in their different states, 0 and 1. We will use the candy and cookie bit.", true);
                UpdateText("The cookie bit represents the right bit in the list. Press continue to scan the cookie bit on the right.", false);
                break;

            case AppManager.AppState.Shape04FirstBitScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("Scanning for cookie bit on right...please zoom in to the cookie bit if not scanning.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Shape05SecondBitExplaination:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                canSetCurrentStatusText = true;
                objectiveStatusText.fontSize = 70;
                currentStatusText.fontSize = 70;
                UpdateText("Try turning the cookie bit on and off with the goblin and see how it changes your current results.", true);
                UpdateText("The candy bit represents the left bit in the list. Press continue to scan the candy bit on the left.", false);
                break;

            case AppManager.AppState.Shape06SecondBitScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableContinued);
                continueInstructionButton.interactable = false;
                UpdateText("Scanning for candy bit on left...please zoom in to the candy bit if not scanning.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Shape07FinalExplaination:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = true;
                UpdateText("Try turning the candy bit on and off with the goblin and see how it changes your current results.", true);
                UpdateText("Now that you can represent all shapes in the list, lets start the exercises. Press continue.", false);
                break;

            case AppManager.AppState.Shape08FirstExercise:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                continueInstructionButton.interactable = false;
                objectiveStatusText.text = "sphere";
                UpdateText("I need you to solve a series of exercises to fix the shape station. Use the list if you need help.", true);
                UpdateText("The first goal is give me a sphere. When you have the correct bits turned on, click the submit button.", false);
                break;

            case AppManager.AppState.Shape09SecondExercise:
                submitAnswerButton.interactable = false;
                objectiveStatusText.text = "ring";
                UpdateText("Great job! Now give me a ring. When you have the correct bits turned on, click the submit button.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Shape10ThirdExercise:
                submitAnswerButton.interactable = false;
                objectiveStatusText.text = "cone";
                UpdateText("Great job! Now give me a cone. When you have the correct bits turned on, click the submit button.", true);
                UpdateText("", false);
                break;

            case AppManager.AppState.Color01StationScanning:
                TrackingStatusUpdate(TrackingStatusState.SearchingForATrackableNew);
                continueInstructionButton.interactable = false;
                submitAnswerButton.interactable = false;
                SetRightSideUIToDefault();
                UpdateText("You fixed it! Let's turn the page and move to the color station. You will learn to combine numbers and lists.", true);
                UpdateText("Bits can represent a range of numbers and a list of objects. This is useful for colors. Click continue.", false);
                break;

            case AppManager.AppState.Color02StationExplanation:
                TrackingStatusUpdate(TrackingStatusState.TrackedAtLeastOneTrackable);
                UpdateText("This is as far as we got for the OC6 demo. Thank you for trying our AR version of C-Spresso.", true);
                UpdateText("Please try out VR version of C-Spresso on Oculus Quest which is a complete binary math level.", false);
                break;

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

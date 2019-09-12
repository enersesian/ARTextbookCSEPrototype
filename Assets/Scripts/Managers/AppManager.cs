using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public enum AppState { Welcome, ResetInstructions, ScanningLesson, RotatingLesson, ActiveTrackingLesson, ActiveTrackingLesson01, ActiveTrackingLesson02, ActiveTrackingLesson03,
        InactiveTrackingLesson, TrackingExercise, TutorialStationScanning, TutorialInteractiveScanning, TutorialExplanation,
        TutorialExercise01, TutorialExercise02, TutorialExercise03, NumberStationScanning };
    public AppState currentAppState;

    public enum ActiveStation { Task, Number, Shape, Color, None };
    public ActiveStation currentStation;

    public enum InteractiveState { NotFound, NotActive, Active};
    public InteractiveState[] currentInteractiveState = new InteractiveState[3]; //up to 3 markers in an exercise

    private List<Listener> listeners = new List<Listener>();

    private bool shouldRespondToUserInput = true, shouldImagebeTracked;

    public Text textAppState, textTrackingState;

    private void Start() //called after awake to let listeners register first
    {
        StartApp();
    }

    public void StartApp() //also used with reset input
    {
        SetAppState(AppState.Welcome, ActiveStation.None);
        //SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
    }

    private void SetAppState(AppState tempState, ActiveStation tempStation)
    {
        currentAppState = tempState;
        currentStation = tempStation;
        textAppState.text = currentAppState.ToString();
        foreach (Listener listenerObj in GetComponents<Listener>()) listenerObj.SetListenerState();
    }

    public void SetActiveStation(int tempStation)
    {
        currentStation = (ActiveStation)tempStation;
        //currentAppState = AppState.InteractiveScanning;
    }

    public void ActivateInteractiveState(int temp)
    {
        //temp ranges 4-6 and array index is 0-3
        currentInteractiveState[temp - 4] = InteractiveState.Active;
    }

    public void RegisterListener(Listener newListener)
    {
        listeners.Add(newListener);
    }

    public bool InputDetected(int temp)
    {
        //-1 two finger touch
        //0 - 5 eggy, tutorial, task, number, shape, color stations
        //6 - 8 left, center, right interactives
        //9 eggy interactive lost tracking for AppManager.AppState.InactiveTrackingLesson
        //10 tutorial station exercise, bit turns to 1
        //11 tutorial station exercise, bit turns to 0

        switch (currentAppState)
        {
            case AppManager.AppState.Welcome: //two finger touch
                if (temp == -1) SetAppState(AppState.ResetInstructions, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.ResetInstructions: //two finger touch
                if (temp == -1) SetAppState(AppState.ScanningLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.ScanningLesson: //Eggy index number
                if (temp == 3)
                {
                    SetAppState(AppState.RotatingLesson, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.RotatingLesson: //two finger touch
                if (temp == -1) SetAppState(AppState.ActiveTrackingLesson01, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.ActiveTrackingLesson01: //center cart index number
                if (temp == 7)
                {
                    SetAppState(AppState.ActiveTrackingLesson02, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.ActiveTrackingLesson02: //center cart index number
                if (temp == 6)
                {
                    SetAppState(AppState.ActiveTrackingLesson03, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.ActiveTrackingLesson03: //center cart index number
                if (temp == 8)
                {
                    SetAppState(AppState.InactiveTrackingLesson, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.InactiveTrackingLesson: //center cart lost tracking
                if (temp == -1) SetAppState(AppState.TrackingExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TrackingExercise: //two finger touch
                if (temp == -1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialStationScanning: //tutorial station index number
                if (temp == 1)
                {
                    SetAppState(AppState.TutorialInteractiveScanning, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialInteractiveScanning: //center cart index number
                if (temp == -1) SetAppState(AppState.TutorialExplanation, ActiveStation.None);
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialExplanation: //lesson on how to find the tutorial interaction
                if (temp == 7)
                {
                    SetAppState(AppState.TutorialExercise01, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialExercise01: //center cart tracking becomes inactive, ie bit turns to 0
                if (temp == 10) SetAppState(AppState.TutorialExercise02, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialExercise02: //center cart tracking becomes active, ie bit turns to 1
                if (temp == 11) SetAppState(AppState.TutorialExercise03, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.TutorialExercise03: //two finger touch
                if (temp == -1) SetAppState(AppState.NumberStationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.NumberStationScanning: //task station index number
                if (temp == 2)
                {
                    SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            default:
                shouldImagebeTracked = false;
                break;
        }
        textTrackingState.text = temp.ToString() + " " + shouldImagebeTracked.ToString();
        return shouldImagebeTracked;
    }

    public void UserInputDetected()
    {
        if(shouldRespondToUserInput)
        {
            InputDetected(-1);//code for 2 finger touch
            shouldRespondToUserInput = false;
            Invoke("ActivateUserInput", 3f);
        }
    }

    private void ActivateUserInput()
    {
        shouldRespondToUserInput = true;
    }
}

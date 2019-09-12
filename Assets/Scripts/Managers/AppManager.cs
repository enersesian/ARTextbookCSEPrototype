using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public enum AppState { Eggy01Welcome, Eggy02ResetInstructions, Eggy03ScanningLesson, Eggy04RotatingLesson, Eggy05ActiveTrackingLesson,
        Eggy06InactiveTrackingLesson, Eggy07TrackingExercise, Tutorial01StationScanning, Tutorial02BitScanning, Tutorial03BitExplanation,
        Tutorial04GoblinAdd, Tutorial05GoblinRemove, Tutorial06GoblinPractice, Number01StationScanning };
    public AppState currentAppState;

    public enum ActiveStation { Task, Number, Shape, Color, None };
    public ActiveStation currentStation;

    public enum InteractiveState { NotFound, NotActive, Active};
    public InteractiveState[] currentInteractiveState = new InteractiveState[3]; //up to 3 markers in an exercise

    private List<Listener> listeners = new List<Listener>();

    private bool shouldRespondToUserInput = true, shouldImagebeTracked;

    public Text textAppState, textTrackingState, textLastSpawned;

    private void Start() //called after awake to let listeners register first
    {
        StartApp();
    }

    public void StartApp() //also used with reset input
    {
        SetAppState(AppState.Eggy01Welcome, ActiveStation.None);
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
            case AppManager.AppState.Eggy01Welcome: //two finger touch
                if (temp == -1) SetAppState(AppState.Eggy02ResetInstructions, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy02ResetInstructions: //two finger touch
                if (temp == -1) SetAppState(AppState.Eggy03ScanningLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy03ScanningLesson: //Eggy index number
                if (temp == 0 || temp == 2)
                {
                    SetAppState(AppState.Eggy04RotatingLesson, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy04RotatingLesson: //two finger touch
                if (temp == -1) SetAppState(AppState.Eggy05ActiveTrackingLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy05ActiveTrackingLesson: //center cart index number
                if (temp == 7)
                {
                    SetAppState(AppState.Eggy06InactiveTrackingLesson, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy06InactiveTrackingLesson: //center cart lost tracking
                if (temp == -1) SetAppState(AppState.Eggy07TrackingExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy07TrackingExercise: //two finger touch
                if (temp == -1) SetAppState(AppState.Tutorial01StationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial01StationScanning: //tutorial station index number
                if (temp == 1)
                {
                    SetAppState(AppState.Tutorial02BitScanning, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial02BitScanning: //center cart index number
                if (temp == -1) SetAppState(AppState.Tutorial03BitExplanation, ActiveStation.None);
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial03BitExplanation: //lesson on how to find the tutorial interaction
                if (temp == 6)
                {
                    SetAppState(AppState.Tutorial04GoblinAdd, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial04GoblinAdd: //center cart tracking becomes inactive, ie bit turns to 0
                if (temp == 10) SetAppState(AppState.Tutorial05GoblinRemove, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial05GoblinRemove: //center cart tracking becomes active, ie bit turns to 1
                if (temp == 11) SetAppState(AppState.Tutorial06GoblinPractice, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial06GoblinPractice: //two finger touch
                if (temp == -1) SetAppState(AppState.Number01StationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number01StationScanning: //task station index number
                if (temp == 3)
                {
                    //SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
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

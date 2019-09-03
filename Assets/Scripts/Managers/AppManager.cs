using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{

    public enum AppState { Welcome, ScanningLesson, ScanningExercise, RotatingLesson, ActiveTrackingLesson,
        InactiveTrackingLesson, TrackingExercise, TutorialStationScanning, TutorialInteractiveScanning, TutorialExercise01, TutorialExercise02,
        TutorialExercise03, TaskStationScanning };
    public AppState currentAppState;

    public enum ActiveStation { Task, Number, Shape, Color, None };
    public ActiveStation currentStation;

    public enum InteractiveState { NotFound, NotActive, Active};
    public InteractiveState[] currentInteractiveState = new InteractiveState[3]; //up to 3 markers in an exercise

    private List<Listener> listeners = new List<Listener>();

    private bool shouldRespondToUserInput = true;

    public Text testingText;

    private void Start() //called after awake to let listeners register first
    {
        StartApp();
    }

    public void StartApp() //also used with reset input
    {
        SetAppState(AppState.Welcome, ActiveStation.None);
    }

    private void SetAppState(AppState tempState, ActiveStation tempStation)
    {
        currentAppState = tempState;
        currentStation = tempStation;
        testingText.text = currentAppState.ToString();
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
        testingText.text = temp.ToString();
        switch (currentAppState)
        {
            case AppManager.AppState.Welcome: //two finger touch
                if (temp == -1) SetAppState(AppState.ScanningLesson, ActiveStation.None);
                return false;

            case AppManager.AppState.ScanningLesson: //two finger touch
                if (temp == -1) SetAppState(AppState.ScanningExercise, ActiveStation.None);
                return false;

            case AppManager.AppState.ScanningExercise: //Eggy index number
                if (temp == 1) SetAppState(AppState.RotatingLesson, ActiveStation.None);
                return true;

            case AppManager.AppState.RotatingLesson: //two finger touch
                if (temp == -1) SetAppState(AppState.ActiveTrackingLesson, ActiveStation.None);
                return false;

            case AppManager.AppState.ActiveTrackingLesson: //center cart index number
                if (temp == 7) SetAppState(AppState.InactiveTrackingLesson, ActiveStation.None);
                return true;

            case AppManager.AppState.InactiveTrackingLesson: //center cart lost tracking
                if (temp == 0) SetAppState(AppState.TrackingExercise, ActiveStation.None);
                return false;

            case AppManager.AppState.TrackingExercise: //two finger touch
                if (temp == -1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                return false;

            case AppManager.AppState.TutorialStationScanning: //tutorial station index number
                if (temp == 1) SetAppState(AppState.TutorialInteractiveScanning, ActiveStation.None);
                return true;

            case AppManager.AppState.TutorialInteractiveScanning: //center cart index number
                if (temp == 1) SetAppState(AppState.TutorialExercise01, ActiveStation.None);
                return true;

            case AppManager.AppState.TutorialExercise01: //center cart tracking becomes inactive, ie bit turns to 1
                if (temp == -1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                return false;

            case AppManager.AppState.TutorialExercise02: //center cart tracking becomes active, ie bit turns to 0
                if (temp == -1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                return false;

            case AppManager.AppState.TutorialExercise03: //two finger touch
                if (temp == -1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                return false;

            case AppManager.AppState.TaskStationScanning: //task station index number
                if (temp == 1) SetAppState(AppState.TutorialStationScanning, ActiveStation.None);
                return true;

            default:
                return false;
        }
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppManager : MonoBehaviour
{
    public enum AppState { Eggy01Welcome, Eggy02ResetInstructions, Eggy03ScanningLesson, Eggy04RotatingLesson, Eggy05ActiveTrackingLesson,
        Eggy06InactiveTrackingLesson, Eggy07TrackingExercise, Tutorial01StationScanning, Tutorial02BitScanning, Tutorial03BitExplanation,
        Tutorial04GoblinAdd, Tutorial05CurrentStateExplanation, Tutorial06GoblinRemove, Tutorial07GoblinPractice, Number01StationScanning,
        Number02FirstBitExplaination, Number02FirstBitScanning, Number03SecondBitExplaination, Number03SecondBitScanning, Number04ThirdBitExplaination,
        Number04ThirdBitScanning, Number05SugarGoblinIntro, Number06FirstExercise, Number07SecondExercise, Number08ThirdExercise, Number09FourthExercise,
        Shape01StationScanning, Shape02StationExplaination, Shape03FirstBitExplaination, Shape04FirstBitScanning, Shape05SecondBitExplaination,
        Shape06SecondBitScanning, Shape07FinalExplaination, Shape08FirstExercise, Shape09SecondExercise, Shape10ThirdExercise,
        Color01StationScanning, Color02StationExplanation
    };
    public AppState currentAppState;
    public enum ActiveStation { Task, Number, Shape, Color, None };
    public ActiveStation currentStation;
    public enum InteractiveState { NotFound, NotActive, Active};
    //up to 3 markers in an exercise
    public InteractiveState[] currentInteractiveState = new InteractiveState[3]; 
    public Text textAppState, textTrackingState, textLastSpawned;

    private List<Listener> listeners = new List<Listener>();
    private bool shouldRespondToUserInput = true, shouldImagebeTracked;
    private View view;
    private int numberStationCandyBit, numberStationCookieBit, numberStationCoffeeBit, shapeStationCookieBit, shapeStationCandyBit;

    //called after awake to let listeners register first
    private void Start() 
    {
        view = GetComponent<View>();
        StartApp();
    }

    private void Update()
    {
        /*Used for testing out pages' sequence of actions from within editor
        if (Input.GetKeyDown(KeyCode.Alpha1)) InputDetected(2);
        if (Input.GetKeyDown(KeyCode.Alpha2)) InputDetected(8);
        if (Input.GetKeyDown(KeyCode.Alpha3)) InputDetected(6);
        if (Input.GetKeyDown(KeyCode.Alpha4)) InputDetected(7);
        */
    }

    //also used with reset input
    public void StartApp()
    {
        SetAppState(AppState.Eggy01Welcome, ActiveStation.None);
        //Used with the input key commands in update loop to test out each page's interactives in editor
        //SetAppState(AppState.Shape01StationScanning, ActiveStation.None);
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
        //9 eggy interactive lost tracking for AppManager.AppState.InactiveTrackingLesson, not currently used
        //10 tutorial station exercise, candy bit turns to 0
        //11 tutorial station exercise, candy bit turns to 1
        //12 tutorial station exercise, cookie bit turns to 0
        //13 tutorial station exercise, cookie bit turns to 1
        //14 tutorial station exercise, coffee bit turns to 0
        //15 tutorial station exercise, coffee bit turns to 1
        //16 Submit Answer button clicked

        switch (currentAppState)
        {
            case AppManager.AppState.Eggy01Welcome: //continue button pressed
                if (temp == -1) SetAppState(AppState.Eggy02ResetInstructions, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy02ResetInstructions: //continue button pressed
                if (temp == -1) SetAppState(AppState.Eggy03ScanningLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy03ScanningLesson: //Eggy index number
                if (temp == 0)// || temp == 2) //eggy is poor tracker and sometimes picks up other images first
                {
                    shouldImagebeTracked = true;
                    SetAppState(AppState.Eggy04RotatingLesson, ActiveStation.None);
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy04RotatingLesson: //continue button pressed
                if (temp == -1) SetAppState(AppState.Eggy05ActiveTrackingLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy05ActiveTrackingLesson: //candy interactive index number
                if (temp == 7)
                {
                    shouldImagebeTracked = true;
                    SetAppState(AppState.Eggy06InactiveTrackingLesson, ActiveStation.None);
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy06InactiveTrackingLesson: //candy interactive lost tracking
                if (temp == -1) SetAppState(AppState.Eggy07TrackingExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy07TrackingExercise: //continue button pressed
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

            case AppManager.AppState.Tutorial02BitScanning: //continue button pressed
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

            case AppManager.AppState.Tutorial04GoblinAdd: //cookie tracking becomes inactive, ie bit turns to 0
                if (temp == 12) SetAppState(AppState.Tutorial05CurrentStateExplanation, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial05CurrentStateExplanation:
                if(temp == 12)
                {
                    view.SetCurrentStatusText("Off");
                }
                if(temp == 13)
                {
                    view.SetCurrentStatusText("On");
                }
                if (temp == -1) SetAppState(AppState.Tutorial06GoblinRemove, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial06GoblinRemove: //cookie tracking becomes active, ie bit turns to 1 AND user clicks submit button
                if (temp == 12)
                {
                    view.SetCurrentStatusText("Off");
                }
                if (temp == 13)
                {
                    view.SetCurrentStatusText("On");
                }
                if (temp == 16) SetAppState(AppState.Tutorial07GoblinPractice, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Tutorial07GoblinPractice: //continue button pressed
                if (temp == 12)
                {
                    view.SetCurrentStatusText("Off");
                }
                if (temp == 13)
                {
                    view.SetCurrentStatusText("On");
                }
                if (temp == -1) SetAppState(AppState.Number01StationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number01StationScanning:
                if (temp == 2)
                {
                    SetAppState(AppState.Number02FirstBitExplaination, ActiveStation.None);
                    shouldImagebeTracked = true;

                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number02FirstBitExplaination:
                if (temp == -1) SetAppState(AppState.Number02FirstBitScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number02FirstBitScanning:
                if (temp == 8) //tracking coffeeBit
                {
                    shouldImagebeTracked = true;
                    numberStationCandyBit = 0;
                    numberStationCoffeeBit = 0;
                    numberStationCookieBit = 0;
                    SetAppState(AppState.Number03SecondBitExplaination, ActiveStation.None);
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number03SecondBitExplaination:
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == -1) SetAppState(AppState.Number03SecondBitScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number03SecondBitScanning:
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 6) //tracking coffeeBit and cookieBit
                {
                    SetAppState(AppState.Number04ThirdBitExplaination, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number04ThirdBitExplaination:
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == -1) SetAppState(AppState.Number04ThirdBitScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number04ThirdBitScanning:
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 7) //tracking coffeeBit, cookieBit, and candyBit
                {
                    SetAppState(AppState.Number05SugarGoblinIntro, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number05SugarGoblinIntro: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    numberStationCandyBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 11) //candyBit 1
                {
                    numberStationCandyBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == -1) SetAppState(AppState.Number06FirstExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number06FirstExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    numberStationCandyBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 11) //candyBit 1
                {
                    numberStationCandyBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 16) SetAppState(AppState.Number07SecondExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number07SecondExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    numberStationCandyBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 11) //candyBit 1
                {
                    numberStationCandyBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 16) SetAppState(AppState.Number08ThirdExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number08ThirdExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    numberStationCandyBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 11) //candyBit 1
                {
                    numberStationCandyBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 16) SetAppState(AppState.Number09FourthExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Number09FourthExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    numberStationCandyBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 11) //candyBit 1
                {
                    numberStationCandyBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 12) //cookieBit 0
                {
                    numberStationCookieBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 13) //cookieBit 1
                {
                    numberStationCookieBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 14) //coffeeBit 0
                {
                    numberStationCoffeeBit = 0;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 15) //coffeeBit 1
                {
                    numberStationCoffeeBit = 1;
                    view.SetCurrentStatusText((numberStationCandyBit * 4f + numberStationCookieBit * 2f + numberStationCoffeeBit * 1f).ToString());
                }
                if (temp == 16) SetAppState(AppState.Shape01StationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape01StationScanning:
                if (temp == 3)
                {
                    SetAppState(AppState.Shape02StationExplaination, ActiveStation.None);
                    shouldImagebeTracked = true;

                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape02StationExplaination:
                if (temp == -1) SetAppState(AppState.Shape03FirstBitExplaination, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape03FirstBitExplaination:
                if (temp == -1) SetAppState(AppState.Shape04FirstBitScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape04FirstBitScanning:
                if (temp == 6) //tracking cookieBit
                {
                    shouldImagebeTracked = true;
                    shapeStationCandyBit = 0;
                    shapeStationCookieBit = 0;
                    SetAppState(AppState.Shape05SecondBitExplaination, ActiveStation.None);
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape05SecondBitExplaination:
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if(shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == -1) SetAppState(AppState.Shape06SecondBitScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape06SecondBitScanning:
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == 7) //tracking candyBit and cookieBit
                {
                    SetAppState(AppState.Shape07FinalExplaination, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape07FinalExplaination:
                if (temp == 10) //candyBit 0
                {
                    shapeStationCandyBit = 0;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("sphere"); //01
                    else view.SetCurrentStatusText("cube"); //00
                }
                if (temp == 11) //candyBit 1
                {
                    shapeStationCandyBit = 1;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("cone"); //10
                }
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == -1) SetAppState(AppState.Shape08FirstExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape08FirstExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    shapeStationCandyBit = 0;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("sphere"); //01
                    else view.SetCurrentStatusText("cube"); //00
                }
                if (temp == 11) //candyBit 1
                {
                    shapeStationCandyBit = 1;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("cone"); //10
                }
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == 16) SetAppState(AppState.Shape09SecondExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape09SecondExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    shapeStationCandyBit = 0;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("sphere"); //01
                    else view.SetCurrentStatusText("cube"); //00
                }
                if (temp == 11) //candyBit 1
                {
                    shapeStationCandyBit = 1;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("cone"); //10
                }
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == 16) SetAppState(AppState.Shape10ThirdExercise, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Shape10ThirdExercise: //continue button pressed
                if (temp == 10) //candyBit 0
                {
                    shapeStationCandyBit = 0;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("sphere"); //01
                    else view.SetCurrentStatusText("cube"); //00
                }
                if (temp == 11) //candyBit 1
                {
                    shapeStationCandyBit = 1;
                    if (shapeStationCookieBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("cone"); //10
                }
                if (temp == 12) //cookieBit 0
                {
                    shapeStationCookieBit = 0;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("cone"); //10
                    else view.SetCurrentStatusText("cube"); //00

                }
                if (temp == 13) //cookieBit 1
                {
                    shapeStationCookieBit = 1;
                    if (shapeStationCandyBit == 1) view.SetCurrentStatusText("ring"); //11
                    else view.SetCurrentStatusText("sphere"); //01
                }
                if (temp == 16) SetAppState(AppState.Color01StationScanning, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Color01StationScanning:
                if (temp == 4)
                {
                    SetAppState(AppState.Color02StationExplanation, ActiveStation.None);
                    shouldImagebeTracked = true;
                }
                else shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Color02StationExplanation:
                //if (temp == -1) SetAppState(AppState.Shape03FirstBitExplaination, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

                //this is as far as I got for OC6 demo, need to:
                //add all color station cases
                //add output station conclusion case
        }
        textTrackingState.text = temp.ToString() + " " + shouldImagebeTracked.ToString();
        return shouldImagebeTracked;
    }

    public void UserInputDetected()
    {
        //setup a 3 second wait between continue button pressed to avoid user spamming button
        if(shouldRespondToUserInput)
        {
            InputDetected(-1);//code for continue button pressed
            shouldRespondToUserInput = false;
            Invoke("ActivateUserInput", 3f);
        }
    }

    public void AnswerSubmitted()
    {
        InputDetected(16);
    }

    private void ActivateUserInput()
    {
        shouldRespondToUserInput = true;
    }
}

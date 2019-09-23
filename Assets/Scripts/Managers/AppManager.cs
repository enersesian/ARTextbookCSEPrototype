﻿using System.Collections;
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
        Color01StationScanning
    };
    public AppState currentAppState;

    public enum ActiveStation { Task, Number, Shape, Color, None };
    public ActiveStation currentStation;

    public enum InteractiveState { NotFound, NotActive, Active};
    public InteractiveState[] currentInteractiveState = new InteractiveState[3]; //up to 3 markers in an exercise

    private List<Listener> listeners = new List<Listener>();

    private bool shouldRespondToUserInput = true, shouldImagebeTracked;

    public Text textAppState, textTrackingState, textLastSpawned;

    private View view;

    private int numberStationCandyBit, numberStationCookieBit, numberStationCoffeeBit;

    private void Start() //called after awake to let listeners register first
    {
        view = GetComponent<View>();
        StartApp();
    }

    public void StartApp() //also used with reset input
    {
        //SetAppState(AppState.Eggy01Welcome, ActiveStation.None);
        SetAppState(AppState.Number01StationScanning, ActiveStation.None);
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

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            InputDetected(2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            InputDetected(8);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            InputDetected(6);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            InputDetected(7);
        }
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
            case AppManager.AppState.Eggy01Welcome: //two finger touch
                if (temp == -1) SetAppState(AppState.Eggy02ResetInstructions, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy02ResetInstructions: //two finger touch
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

            case AppManager.AppState.Eggy04RotatingLesson: //two finger touch
                if (temp == -1) SetAppState(AppState.Eggy05ActiveTrackingLesson, ActiveStation.None);
                shouldImagebeTracked = false;
                break;

            case AppManager.AppState.Eggy05ActiveTrackingLesson: //center cart index number
                if (temp == 7)
                {
                    shouldImagebeTracked = true;
                    SetAppState(AppState.Eggy06InactiveTrackingLesson, ActiveStation.None);
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

            case AppManager.AppState.Tutorial06GoblinRemove: //center cart tracking becomes active, ie bit turns to 1 AND user clicks submit button
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

            case AppManager.AppState.Tutorial07GoblinPractice: //two finger touch
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

            case AppManager.AppState.Number05SugarGoblinIntro: //two finger touch
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

            case AppManager.AppState.Number06FirstExercise: //two finger touch
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

            case AppManager.AppState.Number07SecondExercise: //two finger touch
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

            case AppManager.AppState.Number08ThirdExercise: //two finger touch
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

            case AppManager.AppState.Number09FourthExercise: //two finger touch
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
                    //SetAppState(AppState.Number02FirstBitExplaination, ActiveStation.None);
                    shouldImagebeTracked = true;

                }
                else shouldImagebeTracked = false;
                break;

            default:
                shouldImagebeTracked = false;
                break;

                //add all shape station cases
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

    public void AnswerSubmitted()
    {
        InputDetected(16);
    }

    private void ActivateUserInput()
    {
        shouldRespondToUserInput = true;
    }
}

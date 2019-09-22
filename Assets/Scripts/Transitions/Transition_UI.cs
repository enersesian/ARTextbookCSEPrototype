using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition_UI : MonoBehaviour, ITransition
{
    public RawImage[] rawImages;
    public Image[] images;
    public Text[] texts;
    public float transitionSpeed;
    private bool isOn;

    public float imageHighestAlpha;
    public bool repeaterForTrackingStatus = true;

    public void TurnOff()
    {
        if(isOn)
        {
            isOn = false;
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, element.color.a, imageHighestAlpha));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, element.color.a, imageHighestAlpha));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, element.color.a, 1f));
        }
    }

    public void TurnOn(float tempWaitTime) //overloaded operator in case need a wait time
    {
        repeaterForTrackingStatus = true;
        Invoke("TurnOn", tempWaitTime);
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            isOn = true;
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, element.color.a, imageHighestAlpha));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, element.color.a, imageHighestAlpha));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, element.color.a, 1f));
        }
    }

    private IEnumerator AlphaTransition(MaskableGraphic element, float tempAlpha, float highestEndAlpha)
    {
        float elapsedTime = 0f, start, end;
        if (isOn) //Go from off to on
        {
            start = tempAlpha;
            end = highestEndAlpha;
        }
        else //Go from on to off
        {
            start = tempAlpha;
            end = 0f;
        }

        while (elapsedTime < transitionSpeed)
        {
            element.color = new Color(element.color.r, element.color.g, element.color.b, Mathf.Lerp(start, end, (elapsedTime / transitionSpeed)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        element.color = new Color(element.color.r, element.color.g, element.color.b, end);
        if (gameObject.name == "TrackingStatusIcon_Active" && repeaterForTrackingStatus)
        {
            if (isOn) TurnOff();
            else TurnOn();
        }
        yield return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition_UI : MonoBehaviour, ITransition
{
    public RawImage[] rawImages;
    public Image[] images;
    public Text[] texts;
    public float waitTime;
    private bool isOn;

    public float imageAlpha;
    public bool repeaterForTrackingStatus = true;

    public void TurnOff()
    {
        if(isOn)
        {
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, imageAlpha));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, imageAlpha));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, 1f));
            isOn = false;
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
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, imageAlpha));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, imageAlpha));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, 1f));
            isOn = true;
        }
    }

    private IEnumerator AlphaTransition(MaskableGraphic element, float tempAlpha)
    {
        float elapsedTime = 0f, start, end;
        if (isOn)
        {
            start = 0f;
            end = tempAlpha;
        }
        else
        {
            start = tempAlpha;
            end = 0f;
        }

        while (elapsedTime < waitTime)
        {
            element.color = new Color(element.color.r, element.color.g, element.color.b, Mathf.Lerp(start, end, (elapsedTime / waitTime)));
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

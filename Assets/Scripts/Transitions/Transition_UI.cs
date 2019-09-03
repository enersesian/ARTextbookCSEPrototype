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

    public void TurnOff()
    {
        if(isOn)
        {
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, false, 0.5f));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, false, 1f));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, false, 1f));
            isOn = false;
        }
    }

    public void TurnOn(float tempWaitTime) //overloaded operator in case need a wait time
    {
        Invoke("TurnOn", tempWaitTime);
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, true, 0.5f));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, true, 1f));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, true, 1f));
            isOn = true;
        }
    }

    private IEnumerator AlphaTransition(MaskableGraphic element, bool toOn, float tempAlpha)
    {
        float elapsedTime = 0f, start, end;
        if (toOn)
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
        yield return null;
    }
}

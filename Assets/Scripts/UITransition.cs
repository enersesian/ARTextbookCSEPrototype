using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITransition : MonoBehaviour
{
    public RawImage[] rawImages;
    public Image[] images;
    public Text[] texts;
    private bool isOn;
    // Use this for initialization
    void Start ()
    {
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TurnOff()
    {
        if(isOn)
        {
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, false));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, false));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, false));
            isOn = false;
        }
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            foreach (MaskableGraphic element in rawImages) StartCoroutine(AlphaTransition(element, true));
            foreach (MaskableGraphic element in images) StartCoroutine(AlphaTransition(element, true));
            foreach (MaskableGraphic element in texts) StartCoroutine(AlphaTransition(element, true));
            isOn = true;
        }
        
    }

    private IEnumerator AlphaTransition(MaskableGraphic element, bool toOn)
    {
        float elapsedTime = 0f, waitTime = 1f, start, end;
        if (toOn)
        {
            start = 0f;
            end = 1f;
        }
        else
        {
            start = 1f;
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

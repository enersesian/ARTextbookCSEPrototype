using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Transition_3D : MonoBehaviour, ITransition
{
    private bool isOn;
    public float transitionSpeed;

    public void TurnOff()
    {
        if(isOn)
        {
            isOn = false;
            StartCoroutine(AlphaTransition());
            
        }
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            isOn = true;
            StartCoroutine(AlphaTransition());
            
        }
        
    }

    private IEnumerator AlphaTransition()
    {
        float elapsedTime = 0f;
        Vector3 start, end;
        if (isOn)
        {
            start = Vector3.zero;
            end = Vector3.one;
        }
        else
        {
            start = Vector3.one;
            end = Vector3.zero;
        }

        while (elapsedTime < transitionSpeed)
        {
            transform.localScale = Vector3.Lerp(start, end, (elapsedTime / transitionSpeed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localScale = end;
        yield return null;
    }
}

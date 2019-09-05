using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Listener : MonoBehaviour
{
    static protected AppManager gameManager;

    public virtual void SetListenerState() { }

    protected void Awake()
    {
        gameManager = GameObject.FindWithTag("GameController").GetComponent<AppManager>();
        gameManager.RegisterListener(this);
    }

}


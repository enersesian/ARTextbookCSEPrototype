namespace GoogleARCore.Examples.AugmentedImage
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.InteropServices;
    using GoogleARCore;
    using GoogleARCoreInternal;
    using UnityEngine;

    public class ARBookPageMainTracker : MonoBehaviour
    {
        private bool[] interfaceBits = new bool[3];
        private ARBookPageVisualizer visualizer;
        private bool coolDownTime = true;
        private int stationCounter;

        // Use this for initialization
        void Start()
        {
            visualizer = GetComponent<ARBookPageVisualizer>();
            for(int i = 0; i < 3; i++)
            {
                interfaceBits[i] = true;
            }
        }

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                switch(stationCounter)
                {
                    case 0:
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        stationCounter = 1;
                        break;
                    case 1:
                        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
                        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        stationCounter = 2;
                        break;
                    case 2:
                        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
                        transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        stationCounter = 3;
                        break;
                    case 3:
                        transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                        stationCounter = 0;
                        break;
                }
            }
        }

        public void SetInterface(int interfaceBit)
        {
            if(coolDownTime)
            {
                coolDownTime = false;
                interfaceBits[interfaceBit] = !interfaceBits[interfaceBit];
                //transform.GetChild(0).GetChild(interfaceBit).gameObject.SetActive(interfaceBits[interfaceBit]);
                Invoke("ResetCoolDownTimer", 2f);
            }
        }

        private void ResetCoolDownTimer()
        {
            coolDownTime = true;
        }
    }
}

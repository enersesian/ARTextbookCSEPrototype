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

        // Use this for initialization
        void Start()
        {
            visualizer = GetComponent<ARBookPageVisualizer>();
            for(int i = 0; i < 3; i++)
            {
                interfaceBits[i] = true;
            }
        }

        public void SetInterface(int interfaceBit)
        {
            if(coolDownTime)
            {
                coolDownTime = false;
                interfaceBits[interfaceBit] = !interfaceBits[interfaceBit];
                //visualizer.ARBookPageElements[visualizer.thisInterfaceElement].transform.GetChild(interfaceBit).gameObject.SetActive(interfaceBits[interfaceBit]);
                transform.GetChild(0).GetChild(interfaceBit).gameObject.SetActive(interfaceBits[interfaceBit]);
                Invoke("ResetCoolDownTimer", 2f);
            }
        }

        private void ResetCoolDownTimer()
        {
            coolDownTime = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class ViveInputTryout : MonoBehaviour
{
    public SteamVR_Action_Single squeezeAction;
    //what does SteamVR_Action_Single represents? only actions with boolean value?
    //setting the class correctly will help when mapping variable to intended actions in Unity Editor

    void Update()
    {
        if (SteamVR_Actions.default_OpenPanel.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Debug.Log("OpenPanel button is pressed!");
        }

        if (SteamVR_Actions.default_OpenPanel.GetStateUp(SteamVR_Input_Sources.Any))
        {
            Debug.Log("OpenPanel button is released!");
        }

        float triggerValue = squeezeAction.GetAxis(SteamVR_Input_Sources.Any);

        if (triggerValue > 0f)
            Debug.Log(triggerValue);
    }
}

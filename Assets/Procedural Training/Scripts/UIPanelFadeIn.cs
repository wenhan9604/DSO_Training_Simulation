using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;

public class UIPanelFadeIn : MonoBehaviour
{
    private GameObject HUDCanvas;
    private string targetName;

    void Start()
    {
        HUDCanvas = this.transform.Find("Canvas").gameObject;
    }

    void Update()
    {
        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            Debug.Log("OpenPanel button is pressed!");
            if (UpdateEyeGazedTarget())
            {
                if (UpdateCanvas())
                {
                    Debug.Log("successfully changed panel in Canvas");
                }
            }
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.Any))
        {
            Debug.Log("OpenPanel button is released!");
            //set inactive all panels
            foreach (Transform panel in HUDCanvas.transform)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }

    bool UpdateEyeGazedTarget()
    {
        if (TobiiXR.FocusedObjects.Count > 0)
        {
            var focusedObject = TobiiXR.FocusedObjects[0];
            targetName = focusedObject.GameObject.name;
            GameObject toolTip = focusedObject.GameObject.transform.Find("ToolTip").gameObject;

            if (toolTip != null)
            {
                Debug.Log("Tooltip on Target is Found and updated");
                return true;
            }
            else
                return false;
        }
        else
            return false;
    }

    bool UpdateCanvas()
    {
        //check targetName against each child Object
        foreach (Transform panel in HUDCanvas.transform)
        {
            if (panel.gameObject.name == targetName)
            {
                panel.gameObject.SetActive(true);
                return true;
            }
        }
        return false;
    }
}

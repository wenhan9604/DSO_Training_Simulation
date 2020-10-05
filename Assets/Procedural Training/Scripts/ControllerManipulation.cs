using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;

public class ControllerManipulation : MonoBehaviour
{
    private GameObject HUDCanvas;
    private string targetName;

    private bool isGazeOnTank = false;
    private bool isGazeOnMiniTank = false;

    [SerializeField] private SteamVR_Action_Vector2 touchpadTouch;
    [SerializeField] private GameObject miniTank;
    [SerializeField] private float rotationSpeed = 50;
    private Vector2 touchDeltaValue;

    void Start()
    {
        HUDCanvas = this.transform.Find("Canvas").gameObject;
    }

    void Update()
    {
        if (touchpadTouch.GetChanged(SteamVR_Input_Sources.Any))
        {
            UpdateEyeGazedTarget();

            if (isGazeOnMiniTank)
            {
                touchDeltaValue = touchpadTouch.GetAxisDelta(SteamVR_Input_Sources.Any);

                if (touchDeltaValue.sqrMagnitude <0.08 && touchDeltaValue.sqrMagnitude > -0.08)
                {
                    Debug.Log("deltavalue = " + touchDeltaValue);
                    float movement = Time.deltaTime * rotationSpeed * 50;
                    miniTank.transform.Rotate(touchDeltaValue.y * movement, touchDeltaValue.x * movement, 0);
                    //alternateive way of scripting
                    //miniTank.transform.Rotate(Vector3.up, rotationSpeed * touchDeltaValue.x);
                    //miniTank.transform.Rotate(Vector3.right, rotationSpeed * touchDeltaValue.y);
                }
            }
        }

        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            UpdateEyeGazedTarget();

            if (isGazeOnTank)
            {
                UpdateCanvas();
            } 
        }
        else if (SteamVR_Actions.default_GrabPinch.GetStateUp(SteamVR_Input_Sources.Any))
        {
            //set inactive all panels
            foreach (Transform panel in HUDCanvas.transform)
            {
                panel.gameObject.SetActive(false);
            }
        }
    }

    void UpdateEyeGazedTarget()
    {
        if (TobiiXR.FocusedObjects.Count > 0)
        {
            var focusedObject = TobiiXR.FocusedObjects[0];
            targetName = focusedObject.GameObject.name;
            GameObject toolTip = focusedObject.GameObject.transform.Find("ToolTip").gameObject;

            if (toolTip != null)
            {
                isGazeOnTank = true;
            }
            if (targetName == "T90Mini")
            {
                isGazeOnMiniTank = true;
            }
        }
        else
        {
            isGazeOnTank = false;
            isGazeOnMiniTank = false;
        }
    }

    void UpdateCanvas()
    {
        //check targetName against each child Object
        foreach (Transform panel in HUDCanvas.transform)
        {
            if (panel.gameObject.name == targetName)
            {
                panel.gameObject.SetActive(true);
            }
        }
    }
}

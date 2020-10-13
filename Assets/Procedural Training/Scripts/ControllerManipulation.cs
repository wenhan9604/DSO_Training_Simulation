using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;

public class ControllerManipulation : MonoBehaviour
{
    //UIPanel Features
    private GameObject HUDCanvas;
    private string targetName;

    private bool isGazeOnMainObject = false;
    private bool isGazeOnMiniObject = false;

    //Rotating MiniObject
    [SerializeField] private SteamVR_Action_Vector2 touchpadTouch;
    [SerializeField] private GameObject miniObj;
    [SerializeField] private float rotationSpeed = 50;
    private Vector2 touchDeltaValue;

    //Controlling Animation of MiniObject
    [SerializeField] private Animator _UAVanimator;
    private bool isExplodedViewActivated = false;

    void Start()
    {
        HUDCanvas = this.transform.Find("Canvas").gameObject;
    }

    void Update()
    {
        //Controlling Explosion of Object with ButtonClick
        if(SteamVR_Actions.default_ExplodedView.GetStateDown(SteamVR_Input_Sources.Any))
        {
            //UpdateEyeGazedTarget();
            isExplodedViewActivated = !isExplodedViewActivated;
            _UAVanimator.SetBool("IsExplodedViewActivated", isExplodedViewActivated);

        }

        //Rotating MiniObject with Trackpad
        if (touchpadTouch.GetChanged(SteamVR_Input_Sources.Any))
        {
            UpdateEyeGazedTarget();

            if (isGazeOnMiniObject)
            {
                touchDeltaValue = touchpadTouch.GetAxisDelta(SteamVR_Input_Sources.Any);

                if (touchDeltaValue.sqrMagnitude <0.08 && touchDeltaValue.sqrMagnitude > -0.08)
                {
                    Debug.Log("deltavalue = " + touchDeltaValue);
                    float movement = Time.deltaTime * rotationSpeed * 50;
                    miniObj.transform.Rotate(touchDeltaValue.y * movement, touchDeltaValue.x * movement, 0);
                    //alternateive way of scripting
                    //miniTank.transform.Rotate(Vector3.up, rotationSpeed * touchDeltaValue.x);
                    //miniTank.transform.Rotate(Vector3.right, rotationSpeed * touchDeltaValue.y);
                }
            }
        }

        //UIPanel Pop up depending on Visual Target
        if (SteamVR_Actions.default_GrabPinch.GetStateDown(SteamVR_Input_Sources.Any))
        {
            UpdateEyeGazedTarget();

            if (isGazeOnMainObject)
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
                isGazeOnMainObject = true;
            }
            if (targetName == "MA9Mini")
            {
                isGazeOnMiniObject = true;
            }
        }
        else
        {
            isGazeOnMainObject = false;
            isGazeOnMiniObject = false;
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

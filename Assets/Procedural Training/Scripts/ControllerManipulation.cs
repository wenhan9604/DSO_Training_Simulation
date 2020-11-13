using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;
using Valve.VR.InteractionSystem.Sample;

public class ControllerManipulation : MonoBehaviour
{
    //UIPanel Features
    private string targetName;

    private bool isGazeOnMiniObject = false;

    //Rotating MiniObject
    [SerializeField] private SteamVR_Action_Vector2 touchpadTouch;
    [SerializeField] private GameObject miniObj;
    [SerializeField] private float rotationSpeed = 50;
    private Vector2 touchDeltaValue;

    //Setting VIVE controllers to be false when object is grabbed
    [SerializeField] private GameObject viveControllers;
    private void Awake()
    {
        InteractableWings.onAttachedToHand += SetControllersActive;
    }

    private void OnDisable()
    {
        InteractableWings.onAttachedToHand -= SetControllersActive;
    }

    void Update()
    {
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
    }

    void UpdateEyeGazedTarget()
    {
        if (TobiiXR.FocusedObjects.Count > 0)
        {
            var focusedObject = TobiiXR.FocusedObjects[0];
            targetName = focusedObject.GameObject.name;

            if (targetName == "MA9Mini")
            {
                isGazeOnMiniObject = true;
            }
        }
        else
        {
            isGazeOnMiniObject = false;
        }
    }

    void SetControllersActive(bool isObjectGrabbed)
    {
        if(isObjectGrabbed)
        {
            viveControllers.SetActive(false);
        }
        else
        {
            viveControllers.SetActive(true);
        }
    }
}

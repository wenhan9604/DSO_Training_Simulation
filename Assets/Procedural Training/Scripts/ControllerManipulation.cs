using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;
using Valve.VR.InteractionSystem.Sample;

public class CompatibleViveControllerwithSteam : MonoBehaviour
{
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem.Sample;


public class OutlineLogic : MonoBehaviour
{

    [SerializeField] private Color initOutlineColor = new Color(1, 1, 1, 1);
    [SerializeField] private Color activatedOutlineColor;
    private Color noColor = new Color(0, 0, 0, 0);

    private void Awake()
    {
        InteractableWings.onAttachedToHand += InitializeOutline;
        InteractableWings.onWithinProximity += ChangeColour;
    }

    private void OnDisable()
    {
        InteractableWings.onAttachedToHand -= InitializeOutline;
        InteractableWings.onWithinProximity -= ChangeColour;
    }

    void InitializeOutline(bool isActivated)
    {
        print("is the object attached? " + isActivated);

        if(isActivated)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", initOutlineColor);
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", noColor);

        }
    }

    void ChangeColour(bool isWithinProximity)
    {
        if(isWithinProximity)
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", activatedOutlineColor);
        }
        else
        {
            this.GetComponent<MeshRenderer>().material.SetColor("_OutlineColor", initOutlineColor);
        }
    }
}

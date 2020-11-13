using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Set up Event Listener for Animator
/// </summary>
public class MA9MiniAnimScript : MonoBehaviour 
{
    private Animator _UAVAnimator;

    //Listening for Events
    private void Awake()
    {
        EyeGazeInteractionControl.onGazeAnimTarget += ActivateTransition;
    }
    private void OnDisable()
    {
        EyeGazeInteractionControl.onGazeAnimTarget -= ActivateTransition;
    }

    private void Start()
    {
        _UAVAnimator = GetComponent<Animator>();
    }

    protected void ActivateTransition(string objectName , bool isTriggerOn)
    {
        _UAVAnimator.SetBool(objectName, isTriggerOn);
        print("object: " + objectName + "bool: " + isTriggerOn);
    }
}

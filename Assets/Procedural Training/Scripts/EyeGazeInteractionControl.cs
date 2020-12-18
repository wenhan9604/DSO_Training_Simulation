using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Tobii.XR;
using System.Linq;

/// <summary>
/// When Gaze is enabled, user will be able to press Menu button when gazing on visual targets to activate gaze interactions
/// </summary>
public class EyeGazeInteractionControl : MonoBehaviour
{
    public delegate void OnGazeAnimTarget(string targetName, bool isGazed);
    public static event OnGazeAnimTarget onGazeAnimTarget;

    public delegate void OnGazePanelTarget(string targetName, bool isGazed);
    public static event OnGazePanelTarget onGazePanelTarget;

    private Dictionary<string, bool> gazePanelFlags = new Dictionary<string, bool>();
    private Dictionary<string, bool> gazeAnimFlags = new Dictionary<string, bool>();

    // Start is called before the first frame update
    void Start()
    {
        SetUpFlags();
    }

    // Update is called once per frame
    void Update()
    {
        //single button input
        if(SteamVR_Actions.default_EyeGazeButton.GetStateDown(SteamVR_Input_Sources.Any))
        {
            UpdateEyeGazeTarget();
        }
    }

    /// <summary>
    /// Set up Flags for each tagged object in Scene
    /// </summary>
    void SetUpFlags()
    {
        GameObject[] gazeAnimTargets = GameObject.FindGameObjectsWithTag("EyeGazeAnim");
        foreach (var target in gazeAnimTargets)
        {
            gazeAnimFlags.Add(target.name, false);
        }

        GameObject[] gazePanelTargets = GameObject.FindGameObjectsWithTag("EyeGazePanel");
        foreach (var target in gazePanelTargets)
        {
            gazePanelFlags.Add(target.name, false);
        }
    }

    void UpdateEyeGazeTarget()
    {
        if(TobiiXR.FocusedObjects.Count>0)
        {
            var gazeTarget = TobiiXR.FocusedObjects[0].GameObject;

            if(gazeTarget.tag == "EyeGazePanel")
            {
                DisableAllGazeAnimFlags();
                CallGazePanelTarget(gazeTarget);
            }
            else if(gazeTarget.tag == "EyeGazeAnim")
            {
                DisableAllGazePanelFlags();
                CallGazeTargetAnim(gazeTarget);
            }
            else
            {
                DisableAllGazeAnimFlags();
                DisableAllGazePanelFlags();
            }
        }
        else
        {
            DisableAllGazeAnimFlags();
            DisableAllGazePanelFlags();
        }
    }

    /// <summary>
    /// Call events which send out GazeTarget's name and flag.
    /// </summary>
    /// <param name="gazeTarget"></param>
    private void CallGazePanelTarget(GameObject gazeTarget)
    {
        if (gazePanelFlags.TryGetValue(gazeTarget.name, out bool value))
        {
            if (onGazePanelTarget != null)
            {
                gazePanelFlags[gazeTarget.name] = !value;
                onGazePanelTarget(gazeTarget.name, gazePanelFlags[gazeTarget.name]);
            }
        }
    }

    /// <summary>
    /// Call events which send out GazeTarget's name and flag.
    /// </summary>
    /// <param name="gazeTarget"></param>
    private void CallGazeTargetAnim(GameObject gazeTarget)
    {
        if (gazeAnimFlags.TryGetValue(gazeTarget.name, out bool value))
        {
            if (onGazeAnimTarget != null)
            {
                gazeAnimFlags[gazeTarget.name] = !value;
                onGazeAnimTarget(gazeTarget.name, gazeAnimFlags[gazeTarget.name]);
            }
        }
    }

    /// <summary>
    /// Set all flags in Gaze Panel Dictionary to be false. Send an event for each flags that are switched off.
    /// </summary>
    private void DisableAllGazePanelFlags()
    {
        foreach (var flag in gazePanelFlags.ToArray())
        {
            if (flag.Value == true)
            {
                gazePanelFlags[flag.Key] = false;
                if (onGazePanelTarget != null)
                {
                    onGazePanelTarget(flag.Key, false);
                }
            }
        }
    }    
    
    /// <summary>
    /// Set all flags in Gaze Anim Dictionary to be false. Send an event for each flags that are switched off.
    /// </summary>
    private void DisableAllGazeAnimFlags()
    {
        foreach (var flag in gazeAnimFlags.ToArray())
        {
            if (flag.Value == true)
            {
                gazeAnimFlags[flag.Key] = false;
                if (onGazeAnimTarget != null)
                {
                    onGazeAnimTarget(flag.Key, false);
                }
            }
        }
    }
}

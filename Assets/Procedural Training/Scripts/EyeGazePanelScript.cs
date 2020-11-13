using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGazePanelScript : MonoBehaviour
{
    private void Awake()
    {
        EyeGazeInteractionControl.onGazePanelTarget += UpdatePanel;
    }
    private void OnDisable()
    {
        EyeGazeInteractionControl.onGazePanelTarget -= UpdatePanel;
    }

    void UpdatePanel(string targetName, bool isActivated)
    {
        //check targetName against every Panels
        foreach (Transform panel in transform)
        {
            if (panel.gameObject.name == targetName)
            {
                panel.gameObject.SetActive(isActivated);
            }
        }
    }
}

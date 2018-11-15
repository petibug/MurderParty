using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlertUI : MonoBehaviour {

    public GameObject HideObject;

    private void OnEnable()
    {
        HideObject.SetActive(true);
    }

    public void CloseAlertPanel()
    {
        gameObject.SetActive(false);
        UI.instance.AlertPanelOpen = false;
        UI.instance.PlayPlayerAlert();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlertUI : MonoBehaviour {

	public void CloseAlertPanel()
    {
        gameObject.SetActive(false);
        UI.instance.AlertPanelOpen = false;
        UI.instance.PlayPlayerAlert();
    }
}

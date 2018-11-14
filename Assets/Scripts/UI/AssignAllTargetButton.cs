using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignAllTargetButton : MonoBehaviour {

    private void OnEnable()
    {
        Button assignButton = GetComponent<Button>();

        if (GamePlay.instance.PlayerList.Count < 2)
        {
            
            assignButton.interactable = false;
        }
        else
        {
            assignButton.interactable = true;
        }
    }
}

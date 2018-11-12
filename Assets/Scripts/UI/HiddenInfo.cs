using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HiddenInfo : MonoBehaviour {

    private bool hidden = false;
    public GameObject BigButton;
    public GameObject SmallButton;
    
    // Use this for initialization
	void Start () {
        ShowButton();
    }
	
    private void HideButton()
    {
        hidden = true;
        BigButton.SetActive(false);
        SmallButton.SetActive(true);
    }

    private void ShowButton()
    {
        hidden = false;
        BigButton.SetActive(true);
        SmallButton.SetActive(false);
    }

    public void SwitchButton()
    {
        if(hidden == true)
        {
            ShowButton();
        }
        else
        {
            HideButton();
        }
    }

    public void OnEnable()
    {
        ShowButton();
    }
}

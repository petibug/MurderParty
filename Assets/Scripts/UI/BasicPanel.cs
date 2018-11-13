using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPanel : MonoBehaviour
{
    bool isOpen = false;
    
    // Use this for initialization
	void Start () {

	}
	

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
        isOpen = false;
    }

    public void SwitchPanel()
    {
        if (isOpen == true)
        {
            ClosePanel();
        }
        else
        {
            OpenPanel();
        }
    }
}

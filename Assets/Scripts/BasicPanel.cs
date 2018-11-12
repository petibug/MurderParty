using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPanel : MonoBehaviour
{

	// Use this for initialization
	void Start () {

	}
	

    public void OpenPanel()
    {
        gameObject.SetActive(true);
    }

    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPanel : MonoBehaviour
{
    bool isOpen = false;

    Animator anim;
    
    // Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	

    public void OpenPanel()
    {
        gameObject.SetActive(true);
        isOpen = true;
    }

    public void ClosePanel()
    {
 
        if (anim != null)
        {
            anim.SetTrigger("close");
        }
        else
        {
            gameObject.SetActive(false);
        }
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

    public void Deactivate()
    {
        gameObject.SetActive(false);
    }
}

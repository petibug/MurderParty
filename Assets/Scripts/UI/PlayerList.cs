using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerList : MonoBehaviour {

    public GameObject Target_icon;
    public GameObject Dead_icon;
    public GameObject Removed_icon;
    public GameObject Victorious_icon;

    public Text NameText;
    public Text ScoreText;

    public enum IconType { target, dead, removed, victorious, nothing}

    public void Reset()
    {
        ScoreText.text = "0";
        SetIcon((int)IconType.nothing);
    }

    public void SetIcon(int type)
    {
        switch (type)
        {
            case (int)IconType.target:
                Target_icon.SetActive(true);
                Dead_icon.SetActive(false);
                Removed_icon.SetActive(false);
                Victorious_icon.SetActive(false);
                // Debug.Log("icon target");
                break;

            case (int)IconType.dead:
                Target_icon.SetActive(false);
                Dead_icon.SetActive(true);
                Removed_icon.SetActive(false);
                Victorious_icon.SetActive(false);
                // Debug.Log("icon dead");
                break;

            case (int)IconType.removed:
                Target_icon.SetActive(false);
                Dead_icon.SetActive(false);
                Removed_icon.SetActive(true);
                Victorious_icon.SetActive(false);
                // Debug.Log("icon removed");
                break;

            case (int)IconType.victorious:
                Target_icon.SetActive(false);
                Dead_icon.SetActive(false);
                Removed_icon.SetActive(false);
                Victorious_icon.SetActive(true);
                // Debug.Log("icon removed");
                break;

            case (int)IconType.nothing:
                Target_icon.SetActive(false);
                Dead_icon.SetActive(false);
                Removed_icon.SetActive(false);
                Victorious_icon.SetActive(false);
                // Debug.Log("icon nothing");
                break;
        }
    }
}

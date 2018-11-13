using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddPlayerPanel : MonoBehaviour {

    public InputField NameInputField;
    public InputField JobInputField;

    public Button AddPlayer_Button;
    public Button AddPlayerWithTarget_Button;


    private List<string> PlayerNameList;

    // Use this for initialization
    void Start () {
        AddPlayer_Button.onClick.AddListener(delegate { ValidateForm(false); });
        AddPlayerWithTarget_Button.onClick.AddListener(delegate { ValidateForm(true); });
    }

    private void OnEnable()
    {
        NameInputField.text = "";
        JobInputField.text = "";
    }

    public void ValidateForm(bool addTarget = false)
    {
        List<Player> PlayerList = GamePlay.instance.PlayerList;
        PlayerNameList = new List<string>();

        foreach (Player player in PlayerList)
        {
            PlayerNameList.Add(player.PlayerName);
        }


        bool formAccepted = true;

        string nom = NameInputField.text;

        if (nom == "")
        {
            Debug.Log("empty name");
            formAccepted = false;
        }
        else
        {
            foreach (string nameCompare in PlayerNameList)
            {
                if (nom == nameCompare)
                {
                    Debug.Log("name already exists");
                    formAccepted = false;
                    break;
                }
            }
        }

        if (formAccepted == true)
        {
            if(addTarget == false)
            {
                GamePlay.instance.AddPlayer(nom, JobInputField.text);
            }
            else
            {
                GamePlay.instance.AddPlayerWithTarget(nom, JobInputField.text);
            }
        }
    }

}

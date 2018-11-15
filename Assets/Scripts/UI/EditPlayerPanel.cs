using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditPlayerPanel : MonoBehaviour {

    public InputField NameInputField;
    public InputField JobInputField;
    public Player player;

    public Button OK_Button;


    private List<string> PlayerNameList;

    // Use this for initialization
    void Start () {
        OK_Button.onClick.AddListener(delegate { ValidateForm(); });
    }

    public void ValidateForm()
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

        if (formAccepted == true && player != null)
        {
            Debug.Log("Editing player: "+ nom);
            GamePlay.instance.EditPlayer(player, nom, JobInputField.text);
        }
    }

}

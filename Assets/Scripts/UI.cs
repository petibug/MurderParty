using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI : MonoBehaviour {

    public static UI instance = null;



    //prefabs
    public GameObject PlayerPrefab;
    public GameObject PlayerShortPrefab;

    //panels
    public RectTransform myPanel;
    public GameObject PlayerPanel;
    public GameObject PlayerPanelAssassin;
    public GameObject ConfirmationPanel;
    private enum ConfirmationTypes { kill, remove, assign };

    //buttons
    public Button AssignTarget_Button;
    public Button RemovePlayer_Button;
    public Button ConfirmationOK_Button;

    public Dictionary<Player, GameObject> UIPlayerlist;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        UIPlayerlist = new Dictionary<Player, GameObject>();
    }


    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }

    public void AddPlayer(Player newPlayer)
    {
        GameObject newPlayerUI = Instantiate(PlayerPrefab);
        newPlayerUI.transform.SetParent(myPanel);
        RectTransform T = newPlayerUI.GetComponent<RectTransform>();
        T.localScale = new Vector3(1, 1, 1);

        UIPlayerlist.Add(newPlayer, newPlayerUI);

        SetPlayerName(newPlayerUI, newPlayer.PlayerName);
        SetPlayerScore(newPlayerUI, newPlayer.kills);

        //button
        Button button = newPlayerUI.GetComponent<Button>();
        button.onClick.AddListener(delegate { OpenPlayerPanel(newPlayer); });

        //style
        SetPlayerStyle(newPlayer);
    }

    public void RemovePlayer(Player player)
    {
        SetPlayerStyle(player);
    }

    public void PlayerKilled(Player victim, Player assassin)
    {

        SetPlayerName(UIPlayerlist[victim], victim.PlayerName + " - killed");
        SetPlayerScore(UIPlayerlist[assassin], assassin.kills);
        SetPlayerStyle(victim);
    }

    private void OpenPlayerPanel(Player player)
    {
        PaintPlayerPanel(player);
        OpenPanel(PlayerPanel);
    }

    private void PaintPlayerPanel(Player player)
    {
        //names 
        SetPlayerName(PlayerPanel, player.PlayerName);
        SetPlayerJob(PlayerPanel, player.PlayerJob);
        string targetName = player.GetTarget() == null ? "" : player.GetTarget().PlayerName;
        SetPlayerTarget(PlayerPanel, targetName);

        //main buttons
        //Assign target button
        AssignTarget_Button.onClick.RemoveAllListeners();
        AssignTarget_Button.onClick.AddListener(delegate { ConfirmAction((int)ConfirmationTypes.assign, player); });
       // AssignTarget_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

        //Remove player button
        RemovePlayer_Button.onClick.RemoveAllListeners();
        RemovePlayer_Button.onClick.AddListener(delegate { ConfirmAction((int)ConfirmationTypes.remove, player); });
       // RemovePlayer_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

        //trackers list
        //clear list
        foreach (Transform child in PlayerPanelAssassin.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        //create new list
        List<Player> PlayerAssassins = GamePlay.instance.GetPlayerAssassinList(player);
        foreach (Player assassin in PlayerAssassins)
        {
            GameObject newPlayerUI = Instantiate(PlayerShortPrefab);
            newPlayerUI.transform.SetParent(PlayerPanelAssassin.transform);
            RectTransform T = newPlayerUI.GetComponent<RectTransform>();
            T.localScale = new Vector3(1, 1, 1);

            //display name
            SetPlayerName(newPlayerUI, assassin.PlayerName);

            //button to assign killer
            Button button = newPlayerUI.GetComponent<Button>();
            button.onClick.AddListener(delegate { ConfirmAction((int)ConfirmationTypes.kill, player, assassin); });
          //  button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });
        }
    }

    private void ConfirmAction(int type, Player victim, Player assassin = null)
    {
        PaintConfirmationPanel(type, victim, assassin);
        OpenPanel(ConfirmationPanel);
    }

    private void PaintConfirmationPanel(int type, Player victim, Player assassin = null)
    {
        //reset
        string ConfirmationText = "";
        ConfirmationOK_Button.onClick.RemoveAllListeners();

        //assign
        switch (type)
        {
            case (int)ConfirmationTypes.assign:
                //text
                ConfirmationText = "Are you sure you want to assign a new target to \n" + victim.PlayerName + "?";
                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlay.instance.AssignTarget(victim); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

                break;

            case (int)ConfirmationTypes.kill:
                //text
                ConfirmationText = "Are you sure you want to confirm that \n" + assassin.PlayerName + " has killed " + victim.PlayerName + "?";

                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlay.instance.KillPlayer(victim, assassin); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

                break;

            case (int)ConfirmationTypes.remove:
                //text
                ConfirmationText = "Are you sure you want to remove this player: \n" + victim.PlayerName + "?";
                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlay.instance.RemovePlayer(victim); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

                break;

        }

        GameObject ConfTextGO = ConfirmationPanel.transform.Find("ConfirmationText").gameObject;
        ConfTextGO.GetComponent<Text>().text = ConfirmationText;
    }

    public void SetWinner(Player player)
    {
        SetPlayerStyle(player);
    }

    private void SetPlayerName(GameObject playerUI, String name)
    {
        GameObject Name = playerUI.transform.Find("name").gameObject;
        Name.GetComponent<Text>().text = name;
    }

    private void SetPlayerJob(GameObject playerUI, String name)
    {
        GameObject Name = playerUI.transform.Find("job").gameObject;
        Name.GetComponent<Text>().text = name;
    }

    private void SetPlayerTarget(GameObject playerUI, String name)
    {
        GameObject Name = playerUI.transform.Find("target").gameObject;
        Name.GetComponent<Text>().text = name;
    }

    private void SetPlayerScore(GameObject playerUI, int score)
    {
        GameObject Score = playerUI.transform.Find("score").gameObject;
        Score.GetComponent<Text>().text = score.ToString();
    }

    private void SetPlayerStyle(Player player)
    {
        GameObject playerUI = UIPlayerlist[player];
        Image image = playerUI.GetComponent<Image>();

        if (player.isVictorious() == true)
        {
            image.color = new Color(0, 1, 0);
        }

        if (player.isItRemoved() == true)
        {
            image.color = new Color(.5f, .5f, .5f);
        }

        if(player.IsItDead() == true)
        {
            image.color = new Color(1, 0, 0);
        }
    }

    private void ClosePanel (GameObject panel)
    {
        BasicPanel PanelFunction;
        if (PanelFunction = panel.GetComponent<BasicPanel>())
        {
            PanelFunction.ClosePanel();
        }
        else
        {
            panel.SetActive(false);
            Debug.Log("Forcing panel closing");
        }
    }

    private void OpenPanel(GameObject panel)
    {
        BasicPanel PanelFunction;
        if (PanelFunction = panel.GetComponent<BasicPanel>())
        {
            PanelFunction.OpenPanel();
        }
        else
        {
            panel.SetActive(true);
            Debug.Log("Forcing panel opening");
        }
    }

}

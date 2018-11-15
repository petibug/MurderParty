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
    public GameObject HomePanel;
    public RectTransform ListPlayerPanel;
    public GameObject PlayerPanel;
    public GameObject PlayerPanelTarget;
    public GameObject PlayerPanelAssassin;
    public GameObject ConfirmationPanel;
    public GameObject PlayerAlertPanel;
    public GameObject AddPlayerPanel;

    public bool AlertPanelOpen;


    //buttons
    public Button KILL_Button;
    public Button AssignTarget_Button;
    public Button RemovePlayer_Button;
    public Button ConfirmationOK_Button;

    private enum ConfirmationTypes { kill, remove, assign };

    public Dictionary<Player, GameObject> UIPlayerlist;
    private List<PlayerAlert> PlayerAlerts;

    //managers
    GamePlay GamePlayManager;

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);


        UIPlayerlist = new Dictionary<Player, GameObject>();
        PlayerAlerts = new List<PlayerAlert>();
        AlertPanelOpen = false;
    }


    // Use this for initialization
    void Start() {
        GamePlayManager = GamePlay.instance;
        
    }

    public void ResetUI()
    {
        ClearPlayers();
        HidePanels();
        PlayerAlerts.Clear();
    }

    public void AddPlayer(Player newPlayer)
    {
        GameObject newPlayerUI = Instantiate(PlayerPrefab);
        newPlayerUI.transform.SetParent(ListPlayerPanel);
        RectTransform T = newPlayerUI.GetComponent<RectTransform>();
        T.localScale = new Vector3(1, 1, 1);

        UIPlayerlist.Add(newPlayer, newPlayerUI);

        PaintPlayerList(newPlayer);
    }

    public void RemovePlayer(Player player)
    {
        SetPlayerStyle(player);
    }

    public void AssignTargetToPlayer(Player player)
    {
           SetPlayerStyle(player);
    }

    public void PlayerKilled(Player victim, Player assassin)
    {
        SetPlayerScore(UIPlayerlist[assassin], assassin.kills);
        SetPlayerStyle(victim);
    }

    public void SetWinner(Player player)
    {
        SetPlayerStyle(player);
    }

    public void AddPlayerAlert(Player victim, Player assassin)
    {
        PlayerAlert Alert = new PlayerAlert(victim.PlayerName, assassin.PlayerName);
        PlayerAlerts.Add(Alert);
        PlayPlayerAlert();
    }

    public void PlayPlayerAlert()
    {
        if(PlayerAlerts.Count > 0 && AlertPanelOpen == false)
        {
            PaintPlayerAlert(PlayerAlerts[0].GetVictimName(), PlayerAlerts[0].GetAssassinName());
            OpenPanel(PlayerAlertPanel);
            AlertPanelOpen = true;
            PlayerAlerts.RemoveAt(0);
        }
    }

    public void ClearPlayers()
    {
        UIPlayerlist.Clear();

        foreach (Transform child in ListPlayerPanel)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void NewGame()
    {
        OpenPanel(AddPlayerPanel);
    }

    public void ResetPlayer(Player player)
    {
        GameObject playerUI = UIPlayerlist[player];
        SetPlayerScore(UIPlayerlist[player], player.kills);
        SetPlayerName(UIPlayerlist[player], player.PlayerName);
        SetPlayerStyle(player);
    }

    private void OpenPlayerPanel(Player player)
    {
        PaintPlayerPanel(player);
        OpenPanel(PlayerPanel);
    }

    private void ConfirmAction(int type, Player victim, Player assassin = null)
    {
        PaintConfirmationPanel(type, victim, assassin);
        OpenPanel(ConfirmationPanel);
    }

    //*** PAINT PANELS ***//

    private void PaintPlayerList(Player player)
    {
        GameObject PlayerUI = UIPlayerlist[player];

        SetPlayerName(PlayerUI, player.PlayerName);
        SetPlayerScore(PlayerUI, player.kills);

        //button
        Button button = PlayerUI.GetComponent<Button>();
        button.onClick.AddListener(delegate { OpenPlayerPanel(player); });

        SetPlayerStyle(player);
    }

    private void PaintPlayerPanel(Player player)
    {
        //names 
        SetPlayerName(PlayerPanel, player.PlayerName);
        SetPlayerJob(PlayerPanel, player.PlayerJob);
        SetPlayerScore(PlayerPanel, player.kills);

        string targetName = player.GetTarget() == null ? "" : player.GetTarget().PlayerName;
        string targetJob = player.GetTarget() == null ? "" : player.GetTarget().PlayerJob;
        SetPlayerTarget(PlayerPanelTarget, targetName, targetJob);




        //main buttons

        //kill button
        KILL_Button.onClick.RemoveAllListeners();
        KILL_Button.onClick.AddListener(delegate { ConfirmAction((int)ConfirmationTypes.kill, player.GetTarget(), player); });


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
        List<Player> PlayerAssassins = GamePlayManager.GetPlayerAssassinList(player);
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

    private void PaintPlayerAlert(string victim, string assassin)
    {
        GameObject Name;
        GameObject Frame = PlayerAlertPanel.transform.Find("Frame").gameObject;
        Name = Frame.transform.Find("assassinName").gameObject;
        Name.GetComponent<Text>().text = assassin;

         Name = Frame.transform.Find("victimeName").gameObject;
        Name.GetComponent<Text>().text = victim;
    }



    private void PaintConfirmationPanel(int type, Player victim, Player assassin = null)
    {
        //reset
        string ConfirmationText = "";
        ConfirmationOK_Button.onClick.RemoveAllListeners();

        string SizePlayer = "<size=200>";

        //assign
        switch (type)
        {
            case (int)ConfirmationTypes.assign:
                //text
                ConfirmationText = "Are you sure you want to assign a new target to \n" + SizePlayer + victim.PlayerName + "</size>\n?";
                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlayManager.AssignTargetToPlayer(victim); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });
                 break;

            case (int)ConfirmationTypes.kill:
                //text
                ConfirmationText = "Are you sure you want to confirm that \n" + SizePlayer + assassin.PlayerName + "\n</size> has killed \n" + SizePlayer + victim.PlayerName + "</size>\n?";

                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlayManager.KillPlayer(victim, assassin); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { Handheld.Vibrate(); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

                break;

            case (int)ConfirmationTypes.remove:
                //text
                ConfirmationText = "Are you sure you want to remove this player: \n" + SizePlayer + victim.PlayerName + "</size>\n?";
                //buttons
                ConfirmationOK_Button.onClick.AddListener(delegate { GamePlayManager.RemovePlayer(victim); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(ConfirmationPanel); });
                ConfirmationOK_Button.onClick.AddListener(delegate { ClosePanel(PlayerPanel); });

                break;

        }

        GameObject ConfTextGO = ConfirmationPanel.transform.Find("frame").gameObject;
        ConfTextGO = ConfTextGO.transform.Find("ConfirmationText").gameObject;
        ConfTextGO.GetComponent<Text>().text = ConfirmationText;
    }

    //sub painting



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

    private void SetPlayerTarget(GameObject playerUI, String name, String job = "")
    {
        GameObject NameGO = playerUI.transform.Find("target").gameObject;
        if (NameGO != null) NameGO.GetComponent<Text>().text = name;

        GameObject JobGO = playerUI.transform.Find("targetJob").gameObject;
        if(JobGO != null) JobGO.GetComponent<Text>().text = job;
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
        PlayerList iconHolder = playerUI.GetComponent<PlayerList>();
        
        iconHolder.SetIcon((int)PlayerList.IconType.nothing);


        if (player.IsItDead() == true)
        {
            iconHolder.SetIcon((int)PlayerList.IconType.dead);
        }
        else if(player.isItRemoved() == true)
        {
            iconHolder.SetIcon((int)PlayerList.IconType.removed);
        }
        else if(player.isVictorious() == true)
        {
            iconHolder.SetIcon((int)PlayerList.IconType.victorious);
        }
        else if(player.GetTarget() != null)
        {
            iconHolder.SetIcon((int)PlayerList.IconType.target);
        }
    }

    private void HidePanels()
    {
        PlayerPanel.SetActive(false);
        ConfirmationPanel.SetActive(false);
        PlayerAlertPanel.SetActive(false);
        HomePanel.SetActive(false);
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

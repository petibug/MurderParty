using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UI : MonoBehaviour {

    public static UI instance = null;

    public RectTransform myPanel;
    public GameObject PlayerPrefab;
    public GameObject PlayerShortPrefab;
    public GameObject PlayerPanel;
    public GameObject PlayerPanelAssassin;
    public Button AssignTarget;
    public Button Remove;

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
    void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddPlayer(Player newPlayer)
    {
        GameObject newPlayerUI = Instantiate(PlayerPrefab);
        newPlayerUI.transform.SetParent(myPanel);
        RectTransform T =  newPlayerUI.GetComponent<RectTransform>();
        T.localScale = new Vector3(1, 1, 1);

        UIPlayerlist.Add(newPlayer, newPlayerUI);

        SetPlayerName(newPlayerUI, newPlayer.PlayerName);
        SetPlayerScore(newPlayerUI, newPlayer.kills);

        //button
        Button button = newPlayerUI.GetComponent<Button>();
        button.onClick.AddListener(delegate { PlayerClicked(newPlayer); });

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

    private void PlayerClicked(Player player)
    {
        //names 
        PlayerPanel.SetActive(true);
        SetPlayerName(PlayerPanel, player.PlayerName);
        SetPlayerJob(PlayerPanel, player.PlayerJob);
        string targetName = player.Target == null ? "" : player.Target.PlayerName;
        SetPlayerTarget(PlayerPanel, targetName);

        //main buttons
        AssignTarget.onClick.RemoveAllListeners();
        AssignTarget.onClick.AddListener(delegate { GamePlay.instance.AssignTarget(player); });
        AssignTarget.onClick.AddListener(ClosePlayerPanel);
        Remove.onClick.RemoveAllListeners();
        Remove.onClick.AddListener(delegate { GamePlay.instance.RemovePlayer(player); });
        Remove.onClick.AddListener(ClosePlayerPanel);

        //trackers
        foreach (Transform child in PlayerPanelAssassin.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach(Player assassin in player.TargetedBy)
        {
            GameObject newPlayerUI = Instantiate(PlayerShortPrefab);
            newPlayerUI.transform.SetParent(PlayerPanelAssassin.transform);
            RectTransform T = newPlayerUI.GetComponent<RectTransform>();
            T.localScale = new Vector3(1, 1, 1);

            SetPlayerName(newPlayerUI, assassin.PlayerName);

            //button
            Button button = newPlayerUI.GetComponent<Button>();
            button.onClick.AddListener(delegate { GamePlay.instance.KillPlayer(player,assassin); });
            button.onClick.AddListener(ClosePlayerPanel);
        }
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


    public void ClosePlayerPanel()
    {
        PlayerPanel.SetActive(false);
    }
 


}

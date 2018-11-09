using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;using System;

public class UI : MonoBehaviour {

    public static UI instance = null;

    public RectTransform myPanel;
    public GameObject PlayerPrefab;
    public GameObject PlayerPanel;

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
    }

    public void RemovePlayer(Player player)
    {
        GameObject playerUI = UIPlayerlist[player];
        Destroy(playerUI);
        UIPlayerlist.Remove(player);
    }

    public void PlayerKilled(Player victim, Player assassin)
    {

        SetPlayerName(UIPlayerlist[victim], victim.PlayerName + " - killed");
        SetPlayerScore(UIPlayerlist[assassin], assassin.kills);


    }

    private void PlayerClicked(Player player)
    {
        PlayerPanel.SetActive(true);
        SetPlayerName(PlayerPanel, player.PlayerName);
        SetPlayerJob(PlayerPanel, player.PlayerJob);
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

    private void SetPlayerScore(GameObject playerUI, int score)
    {
        GameObject Score = playerUI.transform.Find("score").gameObject;
        Score.GetComponent<Text>().text = score.ToString();
    }


    public void ClosePlayerPanel()
    {
        PlayerPanel.SetActive(false);
    }
 


}

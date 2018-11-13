using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GamePlay : MonoBehaviour {

    public static GamePlay instance = null;

    private GameData MainGameData;
    private PersistentData LoadSave;
    public List<Player> PlayerList;
    private UI UIManager;
    

    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

      // DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start () {
        UIManager = UI.instance;
        LoadSave = PersistentData.instance;

        MainGameData = new GameData();
        PlayerList = MainGameData.PlayerList;

        LoadSave.LoadData(PlayerList);

        ContinueGame();
    }

    //****** MAIN ACTIONS *****\\

    public void NewGame(bool ClearPlayers = true)
    {
        Debug.Log("new Game");

        if (ClearPlayers == true)
        {
            Debug.Log("clearing players");
            UIManager.ResetUI();
            PlayerList.Clear();
            UIManager.NewGame();
            //PopulateSample();
            //AssignAllTargets();
        }
        else
        {
            Debug.Log("Reseting players");
            UIManager.ResetUI();
            foreach (Player player in PlayerList)
            {
                player.Reset();
            }
            Populate();
            AssignAllTargets();
        }
    }

    public void ContinueGame()
    {
        Debug.Log("Continuing loaded Game");
        UIManager.ResetUI();
        Populate();
    }

    public Player AddPlayerWithTarget(string name, string job)
    {
        Player newPlayer = AddPlayer(name, job);
        AssignTarget(newPlayer);

        return newPlayer;
    }

    public Player AddPlayerWithTarget(string name, string job, Player target)
    {
        Player newPlayer = AddPlayer(name, job);
        newPlayer.AssignTarget(target);

        return newPlayer;
    }

    public Player AddPlayer(string name, string job)
    {
        Player newPlayer = new Player(name, job);
        PlayerList.Add(newPlayer);
        UIManager.AddPlayer(newPlayer);

        LoadSave.SaveData(PlayerList);
        return newPlayer;
    }

    public void RemovePlayer(Player player)
    {
        //Remove player
        Debug.Log("Removing player: " + player.PlayerName);
        //set player as removed
        player.RemovePlayer();
        UIManager.RemovePlayer(player);

        //look for target loss
        CheckLostTarget(player);

        LoadSave.SaveData(PlayerList);
    }

    public void KillPlayer(Player victim, Player assassin)
    {
        Debug.Log("---> " + assassin.PlayerName + " has killed " + victim.PlayerName);
        victim.Killed(assassin);
        assassin.HasKilled();
        UIManager.PlayerKilled(victim, assassin);

        int deadCount = 0;
        foreach (Player dead in PlayerList)
        {
            if (dead.IsItDead() == true || dead.isItRemoved() == true) deadCount++;
        }

        if (deadCount < PlayerList.Count - 1)
        {
            AssignTarget(assassin, true);

            //look for target loss
            CheckLostTarget(victim);
        }
        else
        {
            Debug.Log("VICTORY for player: " + assassin.PlayerName + " !!!!!");
            assassin.PlayerWins();
            UIManager.SetWinner(assassin);
        }

        LoadSave.SaveData(PlayerList);
    }

    public void AssignTargetToPlayer(Player player)
    {
        AssignTarget(player,true);

        LoadSave.SaveData(PlayerList);
        ;    }

    //***** HELPERS ****//

    private void CheckLostTarget(Player removedPlayer)
    {
        //look for target loss
        foreach (Player lookPlay in PlayerList)
        {
            //Remove from target lists
            if (lookPlay.GetTarget() == removedPlayer)
            {
                Debug.Log("player: " + lookPlay.PlayerName + " has lost its target");

                lookPlay.ClearTarget();
                AssignTarget(lookPlay,true);
            }
        }
    }

    private void AssignTarget(Player killer, bool ShowTargetMessage = false)
    {

        List<Player> TargetList = new List<Player>();

        Dictionary<Player,int> TempList = CountPlayersTargetList();



        int minCount = -1;

        foreach (KeyValuePair<Player, int> KVPair in TempList)
        {

            if (KVPair.Key != killer) {
 
                //only get the players with the lowest assassins
                if (minCount == -1)
                {
                    minCount = KVPair.Value;
                }
                else
                {
                    if (KVPair.Value > minCount)
                        break;
                }

                //add player to target list
                TargetList.Add(KVPair.Key);
                //Debug.Log("Target added: " + KVPair.Key.PlayerName + " with " + KVPair.Value + " trackers");
            }
        }

        //check if unassigned target may only be able to target themselves
        if (TargetList.Count == 2)
        {
            Player PlayerToKeep = null;
            int IndexToRemove;
            foreach (Player target in TargetList)
            {
                if (target.GetTarget() == null && TargetList.Contains(target))
                {
                    PlayerToKeep = target;
                    break;
                }
            }

            if (PlayerToKeep != null)
            {
                IndexToRemove = TargetList.IndexOf(PlayerToKeep) == 0 ? 1 : 0;
                TargetList.RemoveAt(IndexToRemove);
                //Debug.Log("Potential futur self-assign forced: " + PlayerToKeep.PlayerName);
            }
        }

        //remove from assassin targeting the player from the target list if possible.
        if (TargetList.Count > 1) { 
            List<Player> AssassinsOfPlayer = GetPlayerAssassinList(killer);
            foreach (Player assassin in AssassinsOfPlayer)
            {
                if (TargetList.Count > 1)
                {
                    if (TargetList.Remove(assassin)) //remove
                    { 
                        //Debug.Log("Potential target removed: " + assassin.PlayerName);
                    }
                }
            }
        }

        //assign target
        if (TargetList.Count > 0) { 
            int pick = Random.Range(0, TargetList.Count - 1);
            //Debug.Log("index chosen: " + pick + " - out of " + TargetList.Count);
            Player PlayerPick = TargetList[pick];

            killer.AssignTarget(PlayerPick); //assign target to killer
            UIManager.AssignTargetToPlayer(killer);
            Debug.Log("++ Kill assignment: " + killer.PlayerName + " must kill " + PlayerPick.PlayerName);

            //display message
            if (ShowTargetMessage == true)
            {
                UIManager.AddPlayerAlert(PlayerPick, killer);
            }

        }
        else //if no target is available
        {
            Debug.Log("No target available for " + killer.PlayerName + ". Victory?");
            killer.PlayerWins();
            UIManager.SetWinner(killer);
        }

    }

    private void AssignAllTargets()
    {
        foreach (Player player in PlayerList)
        {
            AssignTarget(player,false);
        }

        LoadSave.SaveData(PlayerList);
    }

    private Dictionary<Player, int> CountPlayersTargetList()
    {
        Dictionary<Player, int> PlayerAssassinCount = new Dictionary<Player, int>();
        Dictionary<Player, int> PlayerAssassinCountSorted = new Dictionary<Player, int>();

        foreach (Player player in PlayerList)
        {
            if (player.IsItDead() == false && player.isItRemoved() == false)
            {
                if (!PlayerAssassinCount.ContainsKey(player))
                {
                    PlayerAssassinCount.Add(player, 0);
                }

                if (player.GetTarget() != null)
                {
                    if (PlayerAssassinCount.ContainsKey(player.GetTarget()))
                    {
                        PlayerAssassinCount[player.GetTarget()]++;
                    }
                    else
                    {
                        PlayerAssassinCount.Add(player.GetTarget(), 1);
                    }
                }                
            }
        }
        
        // Order by values.
        // ... Use LINQ to specify sorting by value.
        var items = from pair in PlayerAssassinCount
                    orderby pair.Value ascending
                    select pair;
        
        // Display results.
        foreach (KeyValuePair<Player, int> pair in items)
        {
            // Debug.Log("test sort dictionary : " + pair.Key.PlayerName + " : " + pair.Value);
            PlayerAssassinCountSorted.Add(pair.Key, pair.Value);
        }
        return PlayerAssassinCountSorted;
    }

    public List<Player> GetPlayerAssassinList(Player victim)
    {
        List<Player> AssassinList = new List<Player>();

        foreach (Player player in PlayerList)
        {
            if (player.GetTarget() == victim && player != victim)
            {
                AssassinList.Add(player);
            }
        }


        return AssassinList;
    }

    private void Populate()
    {
        foreach(Player player in PlayerList)
        {
            UIManager.AddPlayer(player);
        }
    }

    private void PopulateSample()
    {

        AddPlayer("Player 01", "artisan");
        AddPlayer("Player 02", "artisan");
        AddPlayer("Player 03", "artisan");
        AddPlayer("Player 04", "artisan");
        AddPlayer("Player 05", "artisan");
    }

    private void DisplayPlayersList()
    {
        Debug.Log("------ Starting player list ------");
        foreach (Player play in PlayerList)
        {
            Debug.Log("Player: " + play.PlayerName + " - Player Job: " + play.PlayerJob);
        }
    }

}

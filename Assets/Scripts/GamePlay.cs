using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlay : MonoBehaviour {

    public static GamePlay instance = null;

    public List<Player> PlayerList;


    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start () {
        PlayerList = new List<Player>();
        Populate();
 
        foreach(Player player in PlayerList)
        {
            AssignTarget(player);
        }
       // AssignTarget(PlayerList[0]);
        //DisplayPlayersList();

        //KillPlayer(PlayerList[0], PlayerList[1]);

        //RemovePlayer(PlayerList[3]);
        // DisplayPlayersList();
    }

    public void AddPlayer(string name, string job)
    {
        Player newPlayer = new Player(name, job);
        PlayerList.Add(newPlayer);
        UI.instance.AddPlayer(newPlayer);
    }

    public void RemovePlayer(Player player)
    {
        //Remove player
        Debug.Log("Removing player: " + player.PlayerName);
        //set player as removed
        player.RemovePlayer();
        UI.instance.RemovePlayer(player);
        RebuildTargetList();

        foreach (Player lookPlay in PlayerList)
        {
            //Remove from target lists
            if (lookPlay.Target == player)
            {
                Debug.Log("player: " + lookPlay.PlayerName + " has lost its target");
                //should Trigger a panel to assign new target

                lookPlay.Target = null;
                AssignTarget(lookPlay);
            }
        }
    }

    public void KillPlayer(Player victim, Player assassin)
    {
        Debug.Log("---> " + assassin.PlayerName + " has killed " + victim.PlayerName);
        victim.Killed(assassin);
        assassin.HasKilled();
        UI.instance.PlayerKilled(victim, assassin);

        AssignTarget(assassin);

        foreach (Player lookPlay in PlayerList)
        {
            //Remove from target lists
            if (lookPlay.Target == victim)
            {
                Debug.Log("player: " + lookPlay.PlayerName + " has lost its target");
                //should Trigger a panel to assign new target
                
                lookPlay.Target = null;
                AssignTarget(lookPlay);
            }
        }

        
    }


    public void AssignTarget(Player killer)
    {
        RebuildTargetList();

        List<Player> TargetList = new List<Player>();

        List<Player> TempList = new List<Player>();

        foreach (Player tempPlay in PlayerList)
        {
            if (tempPlay.IsItDead() == false && tempPlay != killer && tempPlay.isItRemoved() == false) TempList.Add(tempPlay);
        }

        TempList.Sort(delegate (Player x, Player y)
        {
            if (x.TargetedBy == null && y.TargetedBy == null) return 0;
            else return x.TargetedBy.Count.CompareTo(y.TargetedBy.Count);
        });

        
        //test templist
        foreach(Player player in TempList)
        {
            Debug.Log("Player: "+ player.PlayerName + " has " + player.TargetedBy.Count + " trackers");
        }
        

        int minCount = -1;

        foreach (Player target in TempList)
        {
            if (minCount == -1)
            {
                minCount = target.TargetedBy.Count;
            }
            else
            {
                if (target.TargetedBy.Count > minCount)
                    break;
            }

            TargetList.Add(target);
        }

        if (TargetList.Count > 0) { 
            int pick = Random.Range(0, TargetList.Count - 1);
            Debug.Log("index chosen: " + pick + " - out of " + TargetList.Count);
            Player PlayerPick = TargetList[pick];

            killer.Target = PlayerPick; //assign target to killer
            Debug.Log("++ Kill assignment: " + killer.PlayerName + " must kill " + PlayerPick.PlayerName);

            RebuildTargetList();
        }
        else
        {
            Debug.Log("No target available for " + killer.PlayerName + ". Victory?");
            killer.PlayerWins();
            UI.instance.SetWinner(killer);
        }

    }

    private void RebuildTargetList()
    {
        foreach (Player player in PlayerList)
        {
            player.ClearAssassinList();
        }

        foreach (Player player2 in PlayerList)
        {
            Player target = player2.Target;
            if (target != null)
            {
                target.AddAssassin(player2);
            }
        }
    }

    private void Populate()
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

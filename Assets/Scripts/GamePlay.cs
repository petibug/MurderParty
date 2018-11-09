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

        KillPlayer(PlayerList[0], PlayerList[1]);

        // RemovePlayer(PlayerList[3]);
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
        
        foreach (Player lookPlay in PlayerList)
        {
            //Remove from target lists
            if (lookPlay.Target == player)
            {
                Debug.Log("player: " + lookPlay.PlayerName + " has lost its target");
                lookPlay.Target = null;
            }

            //remove from targeted list
            lookPlay.TargetedBy.Remove(player);
        }

        //Remove player
        Debug.Log("Removed player: " + player.PlayerName);

        PlayerList.Remove(player);
        UI.instance.RemovePlayer(player);

 
    }

    public void KillPlayer(Player victim, Player assassin)
    {
        Debug.Log("---> " + assassin.PlayerName + " has killed " + victim.PlayerName);
        victim.Killed(assassin);
        assassin.HasKilled();
        UI.instance.PlayerKilled(victim, assassin);
        AssignTarget(assassin);
    }


    public Player AssignTarget(Player killer)
    {
        List<Player> TargetList = new List<Player>();

        List<Player> TempList = new List<Player>();
        foreach(Player tempPlay in PlayerList)
        {
            TempList.Add(tempPlay);
        }

        TempList.Sort(delegate (Player x, Player y)
        {
            if (x.TargetedBy == null && y.TargetedBy == null) return 0;
            else return x.TargetedBy.Count.CompareTo(y.TargetedBy.Count);
        });

        int minCount = -1;

        foreach (Player target in TempList)
        {
            if(minCount == -1) {
                minCount = target.TargetedBy.Count;
            }
            else {
                if (target.TargetedBy.Count > minCount)
                    break;
            }

            if (target.IsItDead() == false && target != killer)
            {
                TargetList.Add(target);
            }
        }

        int pick = Random.Range(0, TargetList.Count);
        Player PlayerPick = TargetList[pick];

        PlayerPick.AddAssassin(killer); //assign assassin to target
        killer.Target = PlayerPick; //assign target to killer

        return PlayerPick;
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

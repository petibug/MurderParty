using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PersistentData : MonoBehaviour
{
    public static PersistentData instance = null;

    [Header("Meta")]
    public string persisterName;

    [Header("Scriptable Objects")]
    public scriptableData gameDataConverted;


    //Awake is always called before any Start functions
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        // DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {

    }

    public void LoadData(List<Player> playerList)
    {

        if (File.Exists(Application.persistentDataPath + string.Format("/{0}.pso", persisterName)))
        {
            Debug.Log("Loading...");
            int playercount = 0;

            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + string.Format("/{0}.pso", persisterName), FileMode.Open);
            JsonUtility.FromJsonOverwrite((string)bf.Deserialize(file), gameDataConverted);
            file.Close();

            List<PlayerIO> PIO = gameDataConverted.PlayerListIO;
            playerList.Clear();


            Dictionary<PlayerIO, Player> PlayersWithTarget = new Dictionary<PlayerIO, Player>();
            Dictionary<PlayerIO, Player> PlayersKilled = new Dictionary<PlayerIO, Player>();

            foreach (PlayerIO playerIO in PIO)
            {
                Player newPlayer = new Player(playerIO._PlayerName, playerIO._PlayerJob);
                newPlayer.isDead = playerIO._isDead;
                newPlayer.removed = playerIO._removed;
                newPlayer.isVictory = playerIO._isVictory;
                newPlayer.kills = playerIO._kills;

                if (playerIO._Target != "") PlayersWithTarget.Add(playerIO,newPlayer);
                if (playerIO._KilledBy != "") PlayersKilled.Add(playerIO, newPlayer);

                playerList.Add(newPlayer);
                playercount++;

            }

            foreach (KeyValuePair<PlayerIO, Player> pair in PlayersWithTarget)
            {
                Player target = playerList.Find(x => x.PlayerName == pair.Key._Target);
                //Debug.Log("searching target for player: " + pair.Value.PlayerName +" :  : " + pair.Key._Target);
                if (target != null)
                {
                    pair.Value.Target = target;
                    //Debug.Log("Player: " + pair.Value.PlayerName + " - target: " + target.PlayerName + " added while loading");
                }

            }

            foreach (KeyValuePair<PlayerIO, Player> pair in PlayersKilled)
            {
                Player killedBy = playerList.Find(x => x.PlayerName == pair.Key._KilledBy);
                if (killedBy != null)
                {
                    pair.Value.KilledBy = killedBy;
                    //Debug.Log("Player: " + pair.Value.PlayerName + " - killed by: " + killedBy.PlayerName + " added while loading");
                }

            }

            Debug.Log("Loaded " + playercount + " player(s)");
        }
        else
        {
            Debug.Log("Loading failed");
        }

    }

    public void SaveData(List<Player> playerList)
    {

        //convert the players
        List<PlayerIO> PIO = gameDataConverted.PlayerListIO;
        PIO.Clear();

        if (playerList != null && playerList.Count != 0)
        {
            foreach(Player player in playerList)
            {
                PlayerIO newPIO = new PlayerIO
                    (
                    player.PlayerName,
                    player.PlayerJob,
                    player.IsItDead(),
                    player.isItRemoved(),
                    player.isVictorious(),
                    player.Target != null ? player.Target.PlayerName : "",
                    player.KilledBy!= null ? player.KilledBy.PlayerName : "",
                    player.kills
                    );
                PIO.Add(newPIO);
            }
        }


        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + string.Format("/{0}.pso", persisterName));
        var json = JsonUtility.ToJson(gameDataConverted);
        bf.Serialize(file, json);
        file.Close();

        Debug.Log("Game saved");
    }


}
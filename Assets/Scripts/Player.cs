using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : IEquatable<Player>,IComparable<Player>
{

    public string PlayerName;
    public string PlayerJob;
    private bool isDead;
    public Player Target;
    public List<Player> TargetedBy;
    public int kills;


    public Player(string name, string job)
    {
        PlayerName = name;
        PlayerJob = job;
        isDead = false;
        TargetedBy = new List<Player>();
        kills = 0;
    }

    public void Killed(Player assasin)
    {
        isDead = true;
        RemoveAssassin(assasin);
    }

    public bool IsItDead()
    {
        return isDead;
    }
    
    public void HasKilled()
    {
        kills++;
    }

    public void AddAssassin(Player assassin)
    {
        if (!TargetedBy.Exists(x => x == assassin))
        {
            TargetedBy.Add(assassin);
        }

        // LIST assassins
        foreach(Player pl in TargetedBy)
        {
            Debug.Log("++ Kill assignment: " + pl.PlayerName + " must kill " + PlayerName);
        } 
    }

    public void RemoveAssassin(Player p)
    {
        TargetedBy.RemoveAll(x => x == p);
    }

    public bool Equals(Player other)
    {
        if (other == null) return false;
        return (this.PlayerName.Equals(other.PlayerName));
    }

    public int CompareTo(Player other)
    {
        if (other == null)
        {
            return 1;
        }

        //Return the difference in power.
        return kills - other.kills;
    }
}

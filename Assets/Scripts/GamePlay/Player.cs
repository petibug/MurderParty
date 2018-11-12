using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player : IEquatable<Player>,IComparable<Player>
{

    public string PlayerName;
    public string PlayerJob;
    private bool isDead;
    private bool removed;
    private bool isVictory;
    private Player Target;
    public Player KilledBy;
    public int kills;


    public Player(string name, string job)
    {
        PlayerName = name;
        PlayerJob = job;
        isDead = false;
        removed = false;
        isVictory = false;
        kills = 0;
    }

    public void Killed(Player assassin)
    {
        isDead = true;
        KilledBy = assassin;
        Target = null;
    }

    public bool IsItDead()
    {
        return isDead;
    }
    
    public void HasKilled()
    {
        kills++;
    }

    public void AssignTarget(Player target)
    {
        Target = target;
    }

    public Player GetTarget()
    {
        return Target;
    }

    public void ClearTarget()
    {
        Target = null;
    }

    public void RemovePlayer()
    {
        removed = true;
        Target = null;
    }

    public bool isItRemoved()
    {
        return removed;
    }

    public void PlayerWins()
    {
        isVictory = true;
    }

    public bool isVictorious()
    {
        return isVictory;
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

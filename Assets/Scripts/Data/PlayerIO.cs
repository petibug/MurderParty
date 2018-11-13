using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerIO 
{
    public string _PlayerName;
    public string _PlayerJob;
    public bool _isDead;
    public bool _removed;
    public bool _isVictory;
    public string _Target;
    public string _KilledBy;
    public int _kills;

    public PlayerIO
        (
        string PlayerName,
        string PlayerJob,
        bool isDead,
        bool removed,
        bool isVictory,
        string Target,
        string KilledBy,
        int kills
        )
    {
        _PlayerName = PlayerName;
        _PlayerJob = PlayerJob;
        _isDead = isDead;
        _removed = removed;
        _isVictory = isVictory;
        _Target = Target;
        _KilledBy = KilledBy;
        _kills = kills;
    }



}

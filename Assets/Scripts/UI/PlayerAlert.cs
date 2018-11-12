using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAlert {

    private string VictimName;
    private string AssassinName;

    public PlayerAlert(string victim, string assassin)
    {
        VictimName = victim;
        AssassinName = assassin;
    }

    public string GetVictimName()
    {
        return VictimName;
    }

    public string GetAssassinName()
    {
        return AssassinName;
    }

}

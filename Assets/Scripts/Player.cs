using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerStats playerStats;

    void Start()
    {
        playerStats = new PlayerStats(new PlayerMeters(10, 20, 30, 40, 50), new Inventory(new ArrayList()), Stage.Student, 0);
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }
}

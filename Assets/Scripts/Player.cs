using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerStats playerStats;
    HUDManager hudManager;

    void Start()
    {
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
        playerStats = new PlayerStats(new PlayerMeters(10, 20, 30, 40, 50), new Inventory(new ArrayList()), Stage.Student, 0);
    }

    void Update()
    {
        hudManager.HappinessValue.text = playerStats.GetPlayerMeters().GetHappiness().ToString();
        hudManager.HungerValue.text = playerStats.GetPlayerMeters().GetHunger().ToString();
        hudManager.StressValue.text = playerStats.GetPlayerMeters().GetStress().ToString();
        hudManager.WeightValue.text = playerStats.GetPlayerMeters().GetWeight().ToString();
        hudManager.KnowledgeValue.text = playerStats.GetPlayerMeters().GetKnowledge().ToString();
    }

    public PlayerStats GetPlayerStats()
    {
        return playerStats;
    }

    public void SetPlayerStats(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }

    public void ModifyPlayerStats(Stat[] activityStatNames, int[] activityStatValues)
    {
        for (int i = 0; i < activityStatNames.Length; i++)
        {
            this.playerStats.ModifyStats(activityStatNames[i], activityStatValues[i]);
        }
    }

    public void AddItem(Item item)
    {
        this.playerStats.GetInventory().AddItem(item);
    }
    public void RemoveItem(Item item)
    {
        this.playerStats.GetInventory().RemoveItem(item);
    }
    public int GetMoney()
    {
        return this.playerStats.GetMoney();
    }
}

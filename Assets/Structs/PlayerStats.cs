public struct PlayerStats
{
    PlayerMeters meters;
    Inventory inventory;
    Stage stage;
    int money;

    public PlayerStats(PlayerMeters meters, Inventory inventory, Stage stage, int money)
    {
        this.meters = meters;
        this.inventory = inventory;
        this.stage = stage;
        this.money = money;
    }

    public PlayerMeters GetPlayerMeters() { return meters; }
    public Inventory GetInventory() { return inventory; }
    public Stage GetStage() { return stage; }
    public int GetMoney() { return money; }

    public void SetStage(Stage stage) { this.stage = stage;}
    public void SetMoney(int money) { this.money = money; }
    public void ModifyStats(Stat stat, int value)
    {
        switch (stat)
        {
            case Stat.Happiness:
                meters.AddHappiness(value);
                break;
            case Stat.Hunger:
                meters.AddHunger(value);
                break;
            case Stat.Stress:
                meters.AddStress(value);
                break;
            case Stat.Weight:
                meters.AddWeight(value);
                break;
            case Stat.Knowledge:
                meters.AddKnowledge(value);
                break;
            default:
                break;
        }
    }

    public void AddMoney(int money) { this.money += money; if (money < 0) money = 0; }
}

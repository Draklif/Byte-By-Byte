using UnityEngine;

public struct PlayerMeters
{
    int happiness;
    int hunger;
    int stress;
    int weight;
    int knowledge;

    public PlayerMeters(int happiness, int hunger, int stress, int weight, int knowledge)
    {
        this.happiness = happiness;
        this.hunger = hunger;
        this.stress = stress;
        this.weight = weight;
        this.knowledge = knowledge;
    }

    public int GetHappiness() { return happiness; }
    public int GetHunger() { return hunger; }
    public int GetStress() { return stress; }
    public int GetWeight() { return weight; }
    public int GetKnowledge() { return knowledge; }

    public void SetHappiness(int happiness) { this.happiness = happiness; }
    public void SetHunger(int hunger) { this.hunger = hunger; }
    public void SetStress(int stress) { this.stress = stress; }
    public void SetWeight(int weight) { this.weight = weight; }
    public void SetKnowledge(int knowledge) { this.knowledge = knowledge; }

    public void AddHappiness(int happiness) { Mathf.Clamp(this.happiness += happiness, 0, 100); }
    public void AddHunger(int hunger) { Mathf.Clamp(this.hunger += hunger, 0, 100); }
    public void AddStress(int stress) { Mathf.Clamp(this.stress += stress, 0, 100); }
    public void AddWeight(int weight) { Mathf.Clamp(this.weight += weight, 0, 100); }
    public void AddKnowledge(int knowledge) { Mathf.Clamp(this.knowledge += knowledge, 0, 100); }
}

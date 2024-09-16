using System;

public class SpecialEffects
{
    public EffectScriptableObject effectData;
    public DateTime startTime;

    public SpecialEffects(EffectScriptableObject effectData)
    {
        this.effectData = effectData;
        this.startTime = DateTime.Now;
    }

    public bool IsExpired()
    {
        return (DateTime.Now - startTime).TotalHours > effectData.durationInHours;
    }

    public float GetRemainingTime()
    {
        return (float)(effectData.durationInHours - (DateTime.Now - startTime).TotalHours);
    }

    public void UpdateStartTime(DateTime newStartTime)
    {
        startTime = newStartTime;
    }
}
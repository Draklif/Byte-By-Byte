using UnityEngine;

[CreateAssetMenu(fileName = "Effect", menuName = "ScriptableObjects/EffectScriptableObject", order = 1)]
public class EffectScriptableObject : ScriptableObject
{
    public int effectId;
    public Stat statAffected;
    public float modifier;
    public float durationInHours;
    public bool affectsIncrease;
    public bool affectsDecrease;
}
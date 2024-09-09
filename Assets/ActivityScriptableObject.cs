using UnityEngine;

[CreateAssetMenu(fileName = "Activity", menuName = "ScriptableObjects/ActivityScriptableObject", order = 1)]
public class ActivityScriptableObject : ScriptableObject
{
    public int activityId;
    public string activityName;
    public Stat[] statNames;
    public int[] statValues;
    public int timeToComplete;
}
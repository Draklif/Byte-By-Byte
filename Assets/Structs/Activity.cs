using static UnityEditor.Progress;

public struct Activity
{
    int activityId;
    string activityName;
    Stat[] statNames;
    int[] statValues;
    int timeToComplete;

    public Activity(int activityId, string activityName, Stat[] statNames, int[] statValues, int timeToComplete)
    {
        this.activityId = activityId;
        this.activityName = activityName;
        this.statNames = statNames;
        this.statValues = statValues;
        this.timeToComplete = timeToComplete;
    }

    public Activity(ActivityScriptableObject activity)
    {
        this.activityId = activity.activityId;
        this.activityName = activity.activityName;
        this.statNames = activity.statNames;
        this.statValues = activity.statValues;
        this.timeToComplete = activity.timeToComplete;
    }

    public Stat[] GetStatNames()
    {
        return statNames;
    }
    public int[] GetStatValues()
    {
        return statValues;
    }

    public int GetActivityId()
    {
        return activityId;
    }

    public int GetActivityTime()
    {
        return timeToComplete;
    }
}
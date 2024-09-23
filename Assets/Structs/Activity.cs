using static UnityEditor.Progress;

public struct Activity
{
    int activityId;
    string activityName;
    Stat[] statNames;
    int[] statValues;
    Time[] availableTimes;
    int timeToComplete;
    int usesMax;

    public Activity(int activityId, string activityName, Stat[] statNames, int[] statValues, Time[] availableTimes, int timeToComplete, int usesRemaining, int usesMax)
    {
        this.activityId = activityId;
        this.activityName = activityName;
        this.statNames = statNames;
        this.statValues = statValues;
        this.availableTimes = availableTimes;
        this.timeToComplete = timeToComplete;
        this.usesMax = usesMax;
    }

    public Activity(ActivityScriptableObject activity)
    {
        this.activityId = activity.activityId;
        this.activityName = activity.activityName;
        this.statNames = activity.statNames;
        this.statValues = activity.statValues;
        this.availableTimes = activity.availableTimes;
        this.timeToComplete = activity.timeToComplete;
        this.usesMax = activity.usesMax;
    }

    public Stat[] GetStatNames()
    {
        return statNames;
    }

    public int[] GetStatValues()
    {
        return statValues;
    }

    public int[] GetStatValuesHalved()
    {
        int[] halvedValues = new int[statValues.Length];

        for (int i = 0; i < statValues.Length; i++)
        {
            halvedValues[i] = statValues[i] / 2;
        }

        return halvedValues;
    }


    public int GetActivityId()
    {
        return activityId;
    }

    public int GetActivityTime()
    {
        return timeToComplete;
    }

    public int GetMaxUses()
    {
        return usesMax;
    }
}
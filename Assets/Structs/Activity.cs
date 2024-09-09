public struct Activity
{
    public string activityName;
    public Stat[] statNames;
    public int[] statValues;

    public Activity(string activityName, Stat[] statNames, int[] statValues)
    {
        this.activityName = activityName;
        this.statNames = statNames;
        this.statValues = statValues;
    }

    public Activity(ActivityScriptableObject activity)
    {
        this.activityName = activity.activityName;
        this.statNames = activity.statNames;
        this.statValues = activity.statValues;
    }
}
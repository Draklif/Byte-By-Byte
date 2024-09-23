using UnityEngine;

[CreateAssetMenu(fileName = "Job", menuName = "ScriptableObjects/JobScriptableObject", order = 1)]
public class JobScriptableObject : ScriptableObject
{
    public int jobId;
    public string jobName;
    public string jobDesc;
    public int gradeThreshold;
    public int baseSalary;
    public int stressImpact;
    public int knowledgeMultiplier;
}
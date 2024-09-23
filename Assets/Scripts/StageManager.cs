using System;
using System.Collections;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    private int Grade;
    private int WeeklyAssistance;
    private bool Study;
    private Stage actualStage;
    private Player player;
    private WorldManager world;
    private ScriptableObjects SO;

    public event Action OnGradeUpdated;

    void Start()
    {
        world = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();
        actualStage = player.GetPlayerStats().GetStage();

        LoadGrade();
        LoadAssistance();
        LoadCanStudy();

        world.OnNewDay += CalculateStudy;
        world.OnNewWeek += CalculateAssistance;
    }

    void CalculateStudy()
    {
        if (world.GetActualDay() != Day.Saturday || world.GetActualDay() != Day.Sunday)
        {
            Study = true;
        }
        else
        {
            Study = false;
        }
        SaveCanStudy();
    }

    void CalculateAssistance()
    {
        int daysAbsent = 5 - WeeklyAssistance;
        ModifyGrade(-5 * daysAbsent);
        ModifyGrade(2 * WeeklyAssistance);
        ResetAssistance();
    }

    public int GetGrade() { return Grade; }
    public bool CanStudy() { return Study; }
    public void GoStudy() {
        if ((world.GetActualDay() != Day.Saturday || world.GetActualDay() != Day.Sunday) && Study)
        {
            IncrementAssistance();
            int playerKnowledge = player.GetPlayerStats().GetPlayerMeters().GetKnowledge();
            float approvalChance = 0.4f;

            // Exam day
            if (world.GetActualDay() == Day.Friday)
            {
                float actualChance = UnityEngine.Random.Range(0f, 1f);
                if (playerKnowledge >= 60)
                {
                    approvalChance = 0.6f + (0.009f * (playerKnowledge - 60));
                }
                else
                {
                    approvalChance = 0.4f - ((60 - playerKnowledge) * 0.005f);
                }

                if (actualChance <= approvalChance)
                {
                    ModifyGrade(10);
                    player.ModifyPlayerStats(new Stat[] { Stat.Happiness, Stat.Stress }, new int[] { 15, -10 });
                }
                else
                {
                    ModifyGrade(-10);
                    player.ModifyPlayerStats(new Stat[] { Stat.Happiness, Stat.Stress }, new int[] { -10, 15 });
                }
            }
            // Normal day
            else
            {
                if (playerKnowledge >= 30)
                {
                    ModifyGrade(2);
                }
                player.ModifyPlayerStats(new Stat[] { Stat.Knowledge, Stat.Stress }, new int[] { -5, 10 });
            }
            Study = false;
            SaveCanStudy();
        }
    }

    public void ModifyGrade(int value) 
    {
        Grade += value;
        Grade = Mathf.Clamp(Grade, 0, 50);
        OnGradeUpdated?.Invoke();
        SaveGrade();
    }

    public void IncrementAssistance()
    {
        WeeklyAssistance += 1;
        SaveAssistance();
    }

    public void ResetAssistance()
    {
        WeeklyAssistance = 0;
        SaveAssistance();
    }

    public void FinishStudentStage()
    {
        ArrayList AvailableJobs = new ArrayList();
        foreach (JobScriptableObject job in SO.scriptableJobs)
        {
            if (Grade >= job.gradeThreshold)
            {
                AvailableJobs.Add(job);
            }
        }
    }

    void SaveGrade()
    {
        PlayerPrefs.SetInt("Progression_Grade", Grade);
        PlayerPrefs.Save();
    }

    void LoadGrade()
    {
        Grade = PlayerPrefs.GetInt("Progression_Grade", 0);
    }

    void SaveAssistance()
    {
        PlayerPrefs.SetInt("Progression_Assistance", Grade);
        PlayerPrefs.Save();
    }

    void LoadAssistance()
    {
        WeeklyAssistance = PlayerPrefs.GetInt("Progression_Assistance", 0);
    }

    void SaveCanStudy()
    {
        PlayerPrefs.SetInt("Progression_CanStudy", Study ? 1 : 0);
        PlayerPrefs.Save();
    }

    void LoadCanStudy()
    {
        int canStudyInt = PlayerPrefs.GetInt("Progression_CanStudy", 0);
        Study = canStudyInt > 0;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityManager : MonoBehaviour
{
    ScriptableObjects SO;
    Activity[] listOfActivities;
    Dictionary<int, int> activityUsages;
    TimeSpan timer;
    Coroutine timerCoroutine;
    int actualActivityId;
    Player player;
    WorldManager worldManager;
    StageManager stageManager;

    public event Action OnActivityCompleted;
    public event Action OnActivityStarted;
    public event Action<TimeSpan> OnTimerUpdated;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        worldManager = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        stageManager = GameObject.FindGameObjectWithTag("Stage").GetComponent<StageManager>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();

        listOfActivities = new Activity[SO.scriptableActivities.Length];
        activityUsages = new Dictionary<int, int>();

        LoadActivityUsages();

        worldManager.OnNewDay += ResetActivityUses;

        for (int i = 0; i < SO.scriptableActivities.Length; i++)
        {
            listOfActivities[i] = new Activity(SO.scriptableActivities[i]);
        }
    }

    IEnumerator ActivityTimer(int id = 0)
    {
        while (timer.TotalSeconds > 0)
        {
            OnTimerUpdated?.Invoke(timer);
            yield return new WaitForSeconds(1);
            timer = timer.Subtract(TimeSpan.FromSeconds(1));
        }

        HandleActivityReward(id, false);
    }

    Activity? FindActivityById(int id)
    {
        foreach (Activity activity in listOfActivities)
        {
            if (activity.GetActivityId() == id)
            {
                return activity;
            }
        }
        return null;
    }

    public void ExecuteActivity(int id)
    {
        Activity? actualActivity = FindActivityById(id);

        if (actualActivity != null)
        {
            if (activityUsages.ContainsKey(id) && activityUsages[id] >= actualActivity.Value.GetMaxUses() && activityUsages[id] != -1)
            {
                Debug.Log("No more uses remaining for this activity today.");
                return;
            }

            if (timerCoroutine != null)
            {
                HUDManager hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
                hudManager.ShowAlert(
                    "Cancel activity",
                    "You are already on an acitivity. Do you wish to cancel it?",
                    () =>
                    {
                        StopCoroutine(timerCoroutine);
                        StartNewActivity(id);
                    },
                    () =>
                    {
                        Debug.Log("La actividad no fue cancelada.");
                    }
                );
            }
            else
            {
                StartNewActivity(id);
            }
        }
    }

    void StartNewActivity(int id)
    {
        Activity? actualActivity = FindActivityById(id);

        if (actualActivity != null)
        {
            if (!StartNewSpecialActivity(id))
            {
                return;
            }

            OnActivityStarted?.Invoke();
            int duration = actualActivity.Value.GetActivityTime();
            timer = TimeSpan.FromSeconds(duration);
            timerCoroutine = StartCoroutine(ActivityTimer(id));
            actualActivityId = id;

            if (activityUsages.ContainsKey(id))
            {
                activityUsages[id]++;
            }
            else
            {
                activityUsages[id] = 1;
            }

            SaveActivityUsages();
        }
    }

    bool StartNewSpecialActivity(int id)
    {
        bool shouldContinue = false;
        switch (id)
        {
            case 101: // Student_Study
                if (stageManager.CanStudy())
                {
                    shouldContinue = true;
                }
                break;
            default:
                shouldContinue = true;
                break;
        }
        return shouldContinue;
    }

    public void SkipActivity()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            HandleActivityReward(actualActivityId, true);
        }
    }

    void HandleActivityReward(int id, bool halved)
    {
        Activity? actualActivity = FindActivityById(id);
        if (actualActivity != null)
        {
            if (HandleSpecialActivityReward(id, halved)) { return; }
        
            if (halved)
            {
                player.ModifyPlayerStats(actualActivity.Value.GetStatNames(), actualActivity.Value.GetStatValuesHalved());
            }
            else
            {
                player.ModifyPlayerStats(actualActivity.Value.GetStatNames(), actualActivity.Value.GetStatValues());
            }
            OnActivityCompleted?.Invoke();
        }
    }

    bool HandleSpecialActivityReward(int id, bool halved)
    {
        bool isSpecialActivity = true;
        switch (id)
        {
            case 100: // Work
                player.ModifyMoneyWork(halved);
                OnActivityCompleted?.Invoke();
                break;
            case 101: // Student Stage Study
                stageManager.GoStudy();
                OnActivityCompleted?.Invoke();
                break;
            default:
                isSpecialActivity = false;
                break;
        }
        return isSpecialActivity;
    }

    void ResetActivityUses()
    {
        activityUsages.Clear();

        foreach (Activity activity in listOfActivities)
        {
            PlayerPrefs.DeleteKey("Activity_" + activity.GetActivityId());
        }
        PlayerPrefs.Save();
    }

    void LoadActivityUsages()
    {
        foreach (Activity activity in listOfActivities)
        {
            int uses = PlayerPrefs.GetInt("Activity_" + activity.GetActivityId(), 0);
            activityUsages[activity.GetActivityId()] = uses;
        }
    }

    void SaveActivityUsages()
    {
        foreach (var usage in activityUsages)
        {
            PlayerPrefs.SetInt("Activity_" + usage.Key, usage.Value);
        }
        PlayerPrefs.Save();
    }
}

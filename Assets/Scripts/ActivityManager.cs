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

        LoadCurrentActivity();
    }

    IEnumerator WaitForHUDAndStartActivity()
    {
        yield return new WaitForSeconds(1);

        OnActivityStarted?.Invoke();
    }

    IEnumerator ActivityTimer(int id = 0)
    {
        while (timer.TotalSeconds > 0)
        {
            OnTimerUpdated?.Invoke(timer);

            string activityData = id + ":" + timer.TotalSeconds;
            PlayerPrefs.SetString("Activity_Current", activityData);
            PlayerPrefs.Save();

            yield return new WaitForSeconds(1);
            timer = timer.Subtract(TimeSpan.FromSeconds(1));
        }

        PlayerPrefs.DeleteKey("Activity_Current");
        PlayerPrefs.DeleteKey("Activity_StartDate");
        PlayerPrefs.Save();

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
            if (activityUsages.ContainsKey(id) && activityUsages[id] >= actualActivity.Value.GetMaxUses() && actualActivity.Value.GetMaxUses() != -1)
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

            PlayerPrefs.SetString("Activity_StartDate", DateTime.Now.ToString());
            PlayerPrefs.Save();

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
            case 102: // Sleep
                if (player.CanSleep())
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

            PlayerPrefs.DeleteKey("Activity_Current");
            PlayerPrefs.DeleteKey("Activity_StartDate");
            PlayerPrefs.Save();

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
            case 102: // Sleep
                player.Sleep(halved);
                OnActivityCompleted?.Invoke();
                break;
            default:
                isSpecialActivity = false;
                break;
        }

        PlayerPrefs.DeleteKey("Activity_Current");
        PlayerPrefs.Save();

        return isSpecialActivity;
    }

    void ResetActivityUses()
    {
        activityUsages.Clear();
        PlayerPrefs.DeleteKey("Activity_Usages");
        PlayerPrefs.Save();
    }

    void LoadActivityUsages()
    {
        string activityData = PlayerPrefs.GetString("Activity_Usages", "");
        if (!string.IsNullOrEmpty(activityData))
        {
            string[] activityPairs = activityData.Split(',');

            foreach (string pair in activityPairs)
            {
                string[] activityInfo = pair.Split(':');
                if (activityInfo.Length == 2)
                {
                    int activityId = int.Parse(activityInfo[0]);
                    int uses = int.Parse(activityInfo[1]);

                    activityUsages[activityId] = uses;
                }
            }
        }
        else
        {
            foreach (Activity activity in listOfActivities)
            {
                activityUsages[activity.GetActivityId()] = 0;
            }
        }
    }

    void SaveActivityUsages()
    {
        List<string> activityDataList = new List<string>();

        foreach (var usage in activityUsages)
        {
            activityDataList.Add(usage.Key + ":" + usage.Value);
        }

        string activityData = string.Join(",", activityDataList);
        PlayerPrefs.SetString("Activity_Usages", activityData);
        PlayerPrefs.Save();
    }

    void LoadCurrentActivity()
    {
        if (PlayerPrefs.HasKey("Activity_Current"))
        {
            string activityData = PlayerPrefs.GetString("Activity_Current");
            string[] dataParts = activityData.Split(':');

            if (dataParts.Length == 2)
            {
                int currentActivityId = int.Parse(dataParts[0]);
                double remainingTime = double.Parse(dataParts[1]);

                if (PlayerPrefs.HasKey("Activity_StartDate"))
                {
                    DateTime startDate = DateTime.Parse(PlayerPrefs.GetString("Activity_StartDate"));
                    TimeSpan timePassed = DateTime.Now - startDate;

                    remainingTime -= timePassed.TotalSeconds;

                    if (remainingTime <= 0)
                    {
                        HandleActivityReward(currentActivityId, false);
                        return;
                    }
                }

                timer = TimeSpan.FromSeconds(remainingTime);

                timerCoroutine = StartCoroutine(ActivityTimer(currentActivityId));
                actualActivityId = currentActivityId;
                StartCoroutine(WaitForHUDAndStartActivity());
            }
        }
    }

}

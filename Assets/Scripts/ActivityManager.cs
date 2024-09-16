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
    WorldManager world;

    public event Action OnActivityCompleted;
    public event Action OnActivityStarted;
    public event Action<TimeSpan> OnTimerUpdated;

    private int workActivityId = 100;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        world = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();

        listOfActivities = new Activity[SO.scriptableActivities.Length];
        activityUsages = new Dictionary<int, int>();

        world.OnNewDay += ResetActivityUses;

        for (int i = 0; i < SO.scriptableActivities.Length; i++)
        {
            listOfActivities[i] = new Activity(SO.scriptableActivities[i]);
        }

        LoadActivityUsages();
    }

    IEnumerator ActivityTimer(int id = 0)
    {
        while (timer.TotalSeconds > 0)
        {
            OnTimerUpdated?.Invoke(timer);
            yield return new WaitForSeconds(1);
            timer = timer.Subtract(TimeSpan.FromSeconds(1));
        }

        HandleActivityReward(id);
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

    //public void ExecuteActivity(int id)
    //{
    //    Activity? actualActivity = FindActivityById(id);

    //    if (actualActivity != null)
    //    {
    //        if (activityUsages.ContainsKey(id) && activityUsages[id] >= actualActivity.Value.GetMaxUses())
    //        {
    //            Debug.Log("No more uses remaining for this activity today.");
    //            return;
    //        }

    //        if (timerCoroutine != null)
    //        {
    //            StopCoroutine(timerCoroutine);
    //        }

    //        OnActivityStarted?.Invoke();
    //        int duration = actualActivity.Value.GetActivityTime();
    //        timer = TimeSpan.FromSeconds(duration);
    //        timerCoroutine = StartCoroutine(ActivityTimer(id));
    //        actualActivityId = id;

    //        if (activityUsages.ContainsKey(id))
    //        {
    //            activityUsages[id]++;
    //        }
    //        else
    //        {
    //            activityUsages[id] = 1;
    //        }

    //        SaveActivityUsages();
    //    }
    //}

    public void ExecuteActivity(int id)
    {
        Activity? actualActivity = FindActivityById(id);

        if (actualActivity != null)
        {
            if (activityUsages.ContainsKey(id) && activityUsages[id] >= actualActivity.Value.GetMaxUses())
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
                        CancelAndStartNewActivity(id);
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

    void CancelAndStartNewActivity(int id)
    {
        StopCoroutine(timerCoroutine);
        StartNewActivity(id);
    }

    void StartNewActivity(int id)
    {
        Activity? actualActivity = FindActivityById(id);

        if (actualActivity != null)
        {
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


    public void ExecuteWork()
    {
        Activity? actualActivity = FindActivityById(workActivityId);

        if (actualActivity != null)
        {
            if (activityUsages.ContainsKey(workActivityId) && activityUsages[workActivityId] >= actualActivity.Value.GetMaxUses())
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
                        CancelAndStartNewActivity(workActivityId);
                    },
                    () =>
                    {
                        Debug.Log("La actividad no fue cancelada.");
                    }
                );
            }
            else
            {
                StartNewActivity(workActivityId);
            }
        }

        //Activity? actualActivity = FindActivityById(workActivityId);

        //if (actualActivity != null)
        //{
        //    if (activityUsages.ContainsKey(workActivityId) && activityUsages[workActivityId] >= actualActivity.Value.GetMaxUses())
        //    {
        //        Debug.Log("No more uses remaining for this activity today.");
        //        return;
        //    }

        //    if (timerCoroutine != null)
        //    {
        //        StopCoroutine(timerCoroutine);
        //    }

        //    OnActivityStarted?.Invoke();
        //    int duration = actualActivity.Value.GetActivityTime();
        //    timer = TimeSpan.FromSeconds(duration);
        //    timerCoroutine = StartCoroutine(ActivityTimer(workActivityId));
        //    actualActivityId = workActivityId;

        //    if (activityUsages.ContainsKey(workActivityId))
        //    {
        //        activityUsages[workActivityId]++;
        //    }
        //    else
        //    {
        //        activityUsages[workActivityId] = 1;
        //    }

        //    SaveActivityUsages();
        //}
    }

    public void SkipActivity()
    {
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            timerCoroutine = null;
            HandleActivityReward(actualActivityId);
        }
    }

    void HandleActivityReward(int id)
    {
        Activity? actualActivity = FindActivityById(id);
        if (id == workActivityId && actualActivity != null)
        {
            player.ModifyMoneyWork();
            OnActivityCompleted?.Invoke();
            return;
        }
        
        if (actualActivity != null)
        {
            player.ModifyPlayerStats(actualActivity.Value.GetStatNames(), actualActivity.Value.GetStatValues());
        }
        OnActivityCompleted?.Invoke();
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

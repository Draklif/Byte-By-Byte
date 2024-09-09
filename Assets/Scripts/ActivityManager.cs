using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActivityManager : MonoBehaviour
{
    ScriptableObjects SO;
    HUDManager hudManager;
    Activity[] listOfActivities;
    TimeSpan timer;
    bool isRunning;
    Coroutine timerCoroutine;
    int actualActivityId;
    Player player;

    IEnumerator ActivityTimer(int id = 0)
    {
        while (timer.TotalSeconds > 0)
        {
            hudManager.RemainingTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timer.Hours, timer.Minutes, timer.Seconds);
            yield return new WaitForSeconds(1);
            timer = timer.Subtract(TimeSpan.FromSeconds(1));
        }

        hudManager.RemainingTimer.text = "00:00:00";
        HandleActivityReward(id);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
        SO = GameObject.FindGameObjectWithTag("SO").GetComponent<ScriptableObjects>();
        listOfActivities = new Activity[SO.scriptableActivities.Length];
        for (int i = 0; i < SO.scriptableActivities.Length; i++)
        {
            listOfActivities[i] = new Activity(SO.scriptableActivities[i]);
        }
    }

    void FixedUpdate()
    {
        if (isRunning)
        {
            hudManager.RemainingTimer.text = string.Format("{0:D2}:{1:D2}:{2:D2}", timer.Hours, timer.Minutes, timer.Seconds);
        }
        else
        {
            hudManager.RemainingTimer.text = "00:00:00";
        }
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
        actualActivityId = id;
        Activity? actualActivity = FindActivityById(id);

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        if (actualActivity != null)
        {
            int duration = actualActivity.Value.GetActivityTime();
            timer = TimeSpan.FromSeconds(duration);
            isRunning = true;
            timerCoroutine = StartCoroutine(ActivityTimer(id));
        }
    }

    public void SkipActivity()
    {
        isRunning = false;
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
            HandleActivityReward(actualActivityId);
        }
    }

    public void HandleActivityReward(int id)
    {
        Activity? actualActivity = FindActivityById(id);

        if (actualActivity != null)
        {
            player.ModifyPlayerStats(actualActivity.Value.GetStatNames(), actualActivity.Value.GetStatValues());
        }
    }
}

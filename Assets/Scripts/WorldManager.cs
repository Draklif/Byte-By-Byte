using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public bool isNewDay, isNewWeek, isNewMonth;

    Day actualDay;
    DateTime firstPlayedDate, lastPlayedDate, actualPlayedDate, lastCheckDate;
    TimeSpan elapsedTimeLastPlayed, elapsedTimeFirstPlayed;

    HUDManager hudManager;

    IEnumerator ManualMode()
    {
        yield return new WaitForSeconds(60);
    }

    void Start()
    {
        hudManager = GameObject.FindGameObjectWithTag("HUD").GetComponent<HUDManager>();
        actualPlayedDate = DateTime.Now;
        actualDay = (Day)actualPlayedDate.DayOfWeek;

        SaveFirstDate();
        SaveLastDate();
        GetElapsedTime();
        CheckDaily();
    }

    void Update()
    {
        StartCoroutine(ManualMode());
        actualPlayedDate = DateTime.Now;
        hudManager.ActualTimer.text = string.Format("{0:D2}:{1:D2}", actualPlayedDate.Hour, actualPlayedDate.Minute);
    }

    public Day GetActualDay()
    {
        return actualDay;
    }

    void SaveFirstDate()
    {
        if (PlayerPrefs.HasKey("FirstPlayedDate"))
        {
            string savedDate = PlayerPrefs.GetString("FirstPlayedDate");
            firstPlayedDate = DateTime.Parse(savedDate);
            Debug.Log("Primera fecha de juego registrada: " + firstPlayedDate);
        }
        else
        {
            firstPlayedDate = DateTime.Now;
            Debug.Log("Primera vez que se ejecuta el juego, primera fecha de juego asignada a ahora.");

            PlayerPrefs.SetString("FirstPlayedDate", firstPlayedDate.ToString());
        }

        PlayerPrefs.Save();
    }

    void SaveLastDate()
    {
        if (PlayerPrefs.HasKey("LastPlayedDate"))
        {
            string savedDate = PlayerPrefs.GetString("LastPlayedDate");
            lastPlayedDate = DateTime.Parse(savedDate);
            Debug.Log("Última fecha de juego registrada: " + lastPlayedDate);
        }
        else
        {
            lastPlayedDate = DateTime.Now;
            Debug.Log("Primera vez que se ejecuta el juego, última fecha de juego asignada a ahora.");
        }

        PlayerPrefs.SetString("LastPlayedDate", actualPlayedDate.ToString());
        PlayerPrefs.Save();
    }

    void SaveCheckDate()
    {
        PlayerPrefs.SetString("LastCheckDate", actualPlayedDate.ToString());
        PlayerPrefs.Save();
    }

    void GetElapsedTime()
    {
        firstPlayedDate = DateTime.Now.AddDays(-20);
        lastPlayedDate = DateTime.Now.AddDays(-2);

        elapsedTimeLastPlayed = actualPlayedDate - lastPlayedDate;
        elapsedTimeFirstPlayed = actualPlayedDate - firstPlayedDate;

        Debug.Log("Tiempo transcurrido desde la última vez que jugaste: " + elapsedTimeLastPlayed.Days + " días - " + elapsedTimeLastPlayed.Hours + ":" + elapsedTimeLastPlayed.Minutes);
        Debug.Log("Tiempo transcurrido desde la primera vez que jugaste: " + elapsedTimeFirstPlayed.Days + " días - " + elapsedTimeFirstPlayed.Hours + ":" + elapsedTimeFirstPlayed.Minutes);
    }

    void CheckFlags()
    {
        isNewDay = (elapsedTimeLastPlayed.Days >= 1 || actualPlayedDate.Day != lastPlayedDate.Day);
        isNewWeek = (elapsedTimeLastPlayed.Days >= 7 || actualPlayedDate.DayOfWeek == DayOfWeek.Monday && lastPlayedDate.DayOfWeek != DayOfWeek.Monday);
        isNewMonth = (elapsedTimeLastPlayed.Days >= 30 || actualPlayedDate.Month != lastPlayedDate.Month || actualPlayedDate.Day == 1);
    }

    void CheckDaily()
    {
        bool canExecuteDaily = true;
        if (PlayerPrefs.HasKey("LastCheckDate"))
        {
            string savedCheckDate = PlayerPrefs.GetString("LastCheckDate");
            lastCheckDate = DateTime.Parse(savedCheckDate);

            canExecuteDaily = actualPlayedDate.Date > lastCheckDate.Date;
        }

        if (canExecuteDaily)
        {
            CheckFlags();
            SaveCheckDate();
        }
    }
}

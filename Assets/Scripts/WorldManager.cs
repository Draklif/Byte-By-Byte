using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public bool isNewDay, isNewWeek, isNewMonth;

    Day actualDay;
    Day previousDay;
    Time actualTime;
    Time previousTime;
    DateTime firstPlayedDate, lastPlayedDate, actualPlayedDate, lastCheckDate;
    TimeSpan elapsedTimeLastPlayed, elapsedTimeFirstPlayed;

    public event Action OnNewDay;
    public event Action OnNewWeek;
    public event Action OnNewMonth;
    public event Action OnNewTimeOfDay;

    void Start()
    {
        actualPlayedDate = DateTime.Now;
        actualDay = (Day)actualPlayedDate.DayOfWeek;
        previousDay = actualDay;
        actualTime = CalculateTimeOfDay(actualPlayedDate.Hour);
        previousTime = actualTime;

        SaveFirstDate();
        SaveLastDate();
        GetElapsedTime();
        CheckDaily();
    }


    void Update()
    {
        actualPlayedDate = DateTime.Now;
        Day currentDay = (Day)actualPlayedDate.DayOfWeek;
        Time currentTime = CalculateTimeOfDay(actualPlayedDate.Hour);

        if (currentDay != previousDay)
        {
            actualDay = currentDay;
            previousDay = actualDay;
            OnNewDay?.Invoke();
        }

        if (currentTime != previousTime)
        {
            actualTime = currentTime;
            previousTime = actualTime;
            OnNewTimeOfDay?.Invoke();
        }
    }


    public Day GetActualDay()
    {
        return actualDay;
    }

    public Time GetActualTime()
    {
        return actualTime;
    }

    public DateTime GetActualDate()
    {
        return actualPlayedDate;
    }

    Time CalculateTimeOfDay(int hour)
    {
        if (hour >= 6 && hour < 12)
            return Time.Morning;
        else if (hour >= 12 && hour < 18)
            return Time.Afternoon;
        else if (hour >= 18 && hour < 24)
            return Time.Evening;
        else
            return Time.Midnight;
    }


    public void CheckTime()
    {
        int hour = actualPlayedDate.Hour;

        if (hour >= 6 && hour < 12)
        {
            actualTime = Time.Morning;
        }
        else if (hour >= 12 && hour < 18)
        {
            actualTime = Time.Afternoon;
        }
        else if (hour >= 18 && hour < 24)
        {
            actualTime = Time.Evening;
        }
        else
        {
            actualTime = Time.Midnight;
        }
    }

    void SaveFirstDate()
    {
        if (PlayerPrefs.HasKey("Date_FirstPlayed"))
        {
            string savedDate = PlayerPrefs.GetString("Date_FirstPlayed");
            firstPlayedDate = DateTime.Parse(savedDate);
            Debug.Log("Primera fecha de juego registrada: " + firstPlayedDate);
        }
        else
        {
            firstPlayedDate = DateTime.Now;
            Debug.Log("Primera vez que se ejecuta el juego, primera fecha de juego asignada a ahora.");

            PlayerPrefs.SetString("Date_FirstPlayed", firstPlayedDate.ToString());
        }

        PlayerPrefs.Save();
    }

    void SaveLastDate()
    {
        if (PlayerPrefs.HasKey("Date_LastPlayed"))
        {
            string savedDate = PlayerPrefs.GetString("Date_LastPlayed");
            lastPlayedDate = DateTime.Parse(savedDate);
            Debug.Log("Última fecha de juego registrada: " + lastPlayedDate);
        }
        else
        {
            lastPlayedDate = DateTime.Now;
            Debug.Log("Primera vez que se ejecuta el juego, última fecha de juego asignada a ahora.");
        }

        PlayerPrefs.SetString("Date_LastPlayed", actualPlayedDate.ToString());
        PlayerPrefs.Save();
    }

    void SaveCheckDate()
    {
        PlayerPrefs.SetString("Date_LastCheck", actualPlayedDate.ToString());
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
        bool newDay = (elapsedTimeLastPlayed.Days >= 1 || actualPlayedDate.Day != lastPlayedDate.Day);
        bool newWeek = (elapsedTimeLastPlayed.Days >= 7 || actualPlayedDate.DayOfWeek == DayOfWeek.Monday && lastPlayedDate.DayOfWeek != DayOfWeek.Monday);
        bool newMonth = (elapsedTimeLastPlayed.Days >= 30 || actualPlayedDate.Month != lastPlayedDate.Month || actualPlayedDate.Day == 1);

        if (newDay)
        {
            isNewDay = true;
            OnNewDay?.Invoke();
        }

        if (newWeek)
        {
            isNewWeek = true;
            OnNewWeek?.Invoke();
        }

        if (newMonth)
        {
            isNewMonth = true;
            OnNewMonth?.Invoke();
        }
    }

    void CheckDaily()
    {
        bool canExecuteDaily = true;
        if (PlayerPrefs.HasKey("Date_LastCheck"))
        {
            string savedCheckDate = PlayerPrefs.GetString("Date_LastCheck");
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

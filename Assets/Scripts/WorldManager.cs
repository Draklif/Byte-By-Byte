using System;
using UnityEngine;

public class WorldManager : MonoBehaviour
{
    public bool isNewDay, isNewWeek, isNewMonth;

    Day actualDay;
    DateTime firstPlayedDate, lastPlayedDate, actualPlayedDate, lastCheckDate;
    TimeSpan elapsedTimeLastPlayed, elapsedTimeFirstPlayed;

    void Start()
    {
        //PlayerPrefs.DeleteAll();

        actualPlayedDate = DateTime.Now;
        actualDay = (Day)actualPlayedDate.DayOfWeek;

        SaveFirstDate();
        SaveLastDate();
        GetElapsedTime();

        if (ShouldPerformDailyCheck())
        {
            CheckFlags();
            SaveCheckDate();
        }
        else
        {
            Debug.Log("El chequeo de nuevo día/semana/mes ya se realizó hoy.");
        }
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
        if (elapsedTimeLastPlayed.Days >= 1 || actualPlayedDate.Day != lastPlayedDate.Day)
        {
            isNewDay = true;
            Debug.Log("Es un nuevo día.");
        }
        else
        {
            isNewDay = false;
        }

        if (elapsedTimeLastPlayed.Days >= 7 || actualPlayedDate.DayOfWeek == DayOfWeek.Monday && lastPlayedDate.DayOfWeek != DayOfWeek.Monday)
        {
            isNewWeek = true;
            Debug.Log("Es una nueva semana.");
        }
        else
        {
            isNewWeek = false;
        }

        if (elapsedTimeLastPlayed.Days >= 30 || actualPlayedDate.Month != lastPlayedDate.Month || actualPlayedDate.Day == 1)
        {
            isNewMonth = true;
            Debug.Log("Es un nuevo mes.");
        }
        else
        {
            isNewMonth = false;
        }
    }

    bool ShouldPerformDailyCheck()
    {
        if (PlayerPrefs.HasKey("LastCheckDate"))
        {
            string savedCheckDate = PlayerPrefs.GetString("LastCheckDate");
            lastCheckDate = DateTime.Parse(savedCheckDate);

            return actualPlayedDate.Date > lastCheckDate.Date;
        }

        return true;
    }

    void SaveCheckDate()
    {
        PlayerPrefs.SetString("LastCheckDate", actualPlayedDate.ToString());
        PlayerPrefs.Save();
    }
}

using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DataLogger : MonoBehaviour
{
    private List<string> logData = new List<string>();

    string filePath;

    void Start()
    {
        filePath = Application.persistentDataPath + "/learning_data.csv";

        // Header
        logData.Add("Time,Event,Value");
    }

    public void LogEvent(string eventName, string value)
    {
        string time = Time.time.ToString("F2");
        logData.Add(time + "," + eventName + "," + value);

        Debug.Log("Logged: " + eventName + " - " + value);
    }

    public void SaveToFile()
    {
        File.WriteAllLines(filePath, logData.ToArray());
        Debug.Log("Data saved to: " + filePath);
    }
}
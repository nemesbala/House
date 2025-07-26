using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class FactoryProductEstimate : MonoBehaviour
{
    public TextMeshProUGUI Estimate;

    // Start is called before the first frame update
    public void OnEnable()
    {
        string PowerFilePath;
        PowerFilePath = Path.Combine(Application.persistentDataPath, "powerLevels.txt");
        float CurrentPower = ReadOverallFloat(PowerFilePath);

        string MoodFilePath;
        MoodFilePath = Path.Combine(Application.persistentDataPath, "moodLevels.txt");
        float CurrentMood = ReadOverallFloat(MoodFilePath);

        Estimate.text = (CurrentMood * CurrentPower * 100f).ToString("F2") + " %";
    }

    /*public void Calculate(int product)
    {
        string PowerFilePath;
        PowerFilePath = Path.Combine(Application.persistentDataPath, "powerLevels.txt");
        float CurrentPower = ReadOverallFloat(PowerFilePath);

        string MoodFilePath;
        MoodFilePath = Path.Combine(Application.persistentDataPath, "moodLevels.txt");
        float CurrentMood = ReadOverallFloat(MoodFilePath);

        Estimate.text = (CurrentMood * CurrentPower * 100f).ToString("F2") + " %";

        EstimateInInt.text = ((int)Math.Ceiling(product * CurrentPower * CurrentMood)).ToString();
    }*/

    float ReadOverallFloat(string path)
    {

        string[] lines = File.ReadAllLines(path);

        foreach (string line in lines)
        {
            if (line.StartsWith("Overall="))
            {
                string valueStr = line.Split('=')[1].Trim();

                if (float.TryParse(valueStr, out float result))
                {
                    return result;
                }
                else
                {
                    Debug.LogError("Failed to parse Overall value to float.");
                    return -1f;
                }
            }
        }

        // If "Overall=" line wasn't found, return an error value
        return 1f;
    }
}

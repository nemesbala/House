using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class WPandSP : MonoBehaviour
{
    string FilePath;
    public TextMeshProUGUI WText;
    public TextMeshProUGUI SText;
    public int Wpoints;
    public int Spoints;

    // Start is called before the first frame update
    void Start()
    {
        FilePath = Path.Combine(Application.persistentDataPath, "WSPoints.txt");
        // Check if the Cash.txt file exists
        if (File.Exists(FilePath))
        {
            UpdateDisplay();
        }
        else
        {
            Wpoints = 50;
            Spoints = 50;
            WText.text = Wpoints.ToString() + " Worker Point";
            SText.text = Spoints.ToString() + " Science Point";
            SaveToFile();
        }
    }

    void SaveToFile()
    {
        string WP = (Wpoints + "," + Spoints);
        File.WriteAllText(FilePath, WP.ToString());
    }

    void UpdateDisplay()
    {
        string PointsString = File.ReadAllText(FilePath);
        string[] data = PointsString.Split(',');
        Wpoints = Int32.Parse(data[0]);
        Spoints = Int32.Parse(data[1]);
        WText.text = Wpoints.ToString() + " Worker Point";
        SText.text = Spoints.ToString() + " Science Point";
    }

    public void ChangeWPoints(int amount)
    {
        Wpoints += amount;
        SaveToFile();
        UpdateDisplay();
    }

    public void ChangeSPoints(int amount)
    {
        Spoints += amount;
        SaveToFile();
        UpdateDisplay();
    }

    public void SubtrackWPoints(int amount)
    {
        Wpoints -= amount;
        SaveToFile();
        UpdateDisplay();
    }

    public void SubtracktSPoints(int amount)
    {
        Spoints -= amount;
        SaveToFile();
        UpdateDisplay();
    }
}

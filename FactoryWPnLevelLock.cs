using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FactoryWPnLevelLock : MonoBehaviour
{
    public int WPNeeded;
    public int MinLevel;
    public GameObject UISelectButton;
    private int currentLevel;
    private int currentWP;
    public Factory factory;
    public bool CheckSPInstedOfWP = false;
    public bool Producing;

    public void Start()
    {
        string houseid = factory.houseID;
        string filePathq = Path.Combine(Application.persistentDataPath, "Building", houseid + ".txt");
        if (File.Exists(filePathq))
        {
            string[] liness = File.ReadAllLines(filePathq);
            string[] Adata = liness[0].Split(',');
            int check = Int32.Parse(Adata[4]);
            if (liness.Length > 1 || check == 0)
            {
                Producing = true;
            }
            else
            {
                Producing = false;
            }
        }

        string WPFilePath = Path.Combine(Application.persistentDataPath, "WSPoints.txt");
        string LevelfilePath = Path.Combine(Application.persistentDataPath, "XPData.txt");

        string[] lines = File.ReadAllLines(LevelfilePath);
        if (lines.Length >= 2)
        {
            currentLevel = int.Parse(lines[1]);  // Read level from the second line
        }

        string PointsString = File.ReadAllText(WPFilePath);
        string[] data = PointsString.Split(',');

        if(CheckSPInstedOfWP)
        {
            currentWP = Int32.Parse(data[1]);
        }
        else
        {
            currentWP = Int32.Parse(data[0]);
        }

        if(currentWP >= WPNeeded && currentLevel >= MinLevel && !Producing)
        {
            UISelectButton.SetActive(true);
        }
        else
        {
            UISelectButton.SetActive(false);
        }
    }
}

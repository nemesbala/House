using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class LevelBlocker : MonoBehaviour
{
    public GameObject Blocker;
    public int NeededLevel;
    private int CurrentLevel;
    private string FilePath;

    public void OnEnable()
    {
        FilePath = Path.Combine(Application.persistentDataPath, "XPData.txt");
        
        string[] lines = File.ReadAllLines(FilePath);
        if (lines.Length >= 2)
        {
            CurrentLevel = int.Parse(lines[1]);  // Read level from the second line
        }

        if (CurrentLevel >= NeededLevel)
        Blocker.SetActive(false);
    }
}

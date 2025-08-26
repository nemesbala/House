using UnityEngine;
using System.IO;

public class TechSaveFileCreator : MonoBehaviour
{
    public string fileName = "techtree.txt";
    public int numberOfTechnologies = 20;
    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), fileName);
        CreateFileIfNeeded();
    }

    public void CreateFileIfNeeded()
    {
        if (!File.Exists(filePath))
        {
            string[] lines = new string[numberOfTechnologies];

            for (int i = 0; i < numberOfTechnologies; i++)
            {
                lines[i] = "0"; // 0 = Locked
            }

            File.WriteAllLines(filePath, lines);
        }
    }
}

using UnityEngine;
using System.IO;

public class PlaneLoader : MonoBehaviour
{
    [Header("Plane Settings")]
    public GameObject[] replacementPlanes; // Array of possible planes to instantiate

    private string saveFilePath;

    void Start()
    {
        saveFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "plane_data.txt");

        if (!File.Exists(saveFilePath))
        {
            File.WriteAllText(saveFilePath, string.Empty); // Create an empty file
            Debug.Log($"Created empty file at: {saveFilePath}");
        }

        LoadPlaneData();
    }

    private void LoadPlaneData()
    {
        if (File.Exists(saveFilePath))
        {
            string[] lines = File.ReadAllLines(saveFilePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split(',');
                if (parts.Length == 4)
                {
                    float x = float.Parse(parts[0]);
                    float y = 0f;
                    float z = float.Parse(parts[2]);
                    string planeName = parts[3];

                    foreach (GameObject plane in replacementPlanes)
                    {
                        if (plane.name == planeName)
                        {
                            GameObject newPlane = Instantiate(plane, new Vector3(x, y, z), Quaternion.identity);
                            newPlane.name = plane.name; // Remove "(Clone)" from the name
                            break;
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("No saved plane data found.");
        }
    }
}

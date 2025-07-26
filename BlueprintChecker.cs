using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BlueprintChecker : MonoBehaviour
{
    private string BlueprintFilePath;
    public int Building_ID;
    public int Number_Of_Blueprints;

    // Start is called before the first frame update
    public void Start()
    {
        BlueprintFilePath = Path.Combine(Application.persistentDataPath, "Blueprints.txt");
        if (!File.Exists(BlueprintFilePath))
        {
            int[] zeros = new int[150];
            for (int i = 0; i < zeros.Length; i++)
            {
                zeros[i] = 3; // Initial value for each blueprint
            }
            zeros[15] = 1;
            zeros[16] = 1;
            // Convert the array to a string separated by commas
            string zeroString = string.Join(",", zeros);
            // Write the string to the file
            File.WriteAllText(BlueprintFilePath, zeroString);
        }

        LoadBlueprints();
    }

    // Load blueprints data from the file
    private void LoadBlueprints()
    {
        if (File.Exists(BlueprintFilePath))
        {
            string[] datas = File.ReadAllLines(BlueprintFilePath);
            string data = datas[0];
            string[] parts = data.Split(',');
            Number_Of_Blueprints = int.Parse(parts[Building_ID]);
        }
        else
        {
            Debug.LogError("Blueprint file not found.");
        }
    }

    // Function to get the blueprint count for a specific building ID
    public int GetBlueprintCount(int buildingID)
    {
        if (File.Exists(BlueprintFilePath))
        {
            string[] datas = File.ReadAllLines(BlueprintFilePath);
            string data = datas[0];
            string[] parts = data.Split(',');

            if (buildingID >= 0 && buildingID < parts.Length)
            {
                return int.Parse(parts[buildingID]);
            }
            else
            {
                Debug.LogError("Invalid Building ID.");
                return -1; // Error value
            }
        }
        else
        {
            Debug.LogError("Blueprint file not found.");
            return -1; // Error value
        }
    }

    public void SubtractBlueprint(int buildingID)
    {
        if (File.Exists(BlueprintFilePath))
        {
            string[] datas = File.ReadAllLines(BlueprintFilePath);
            string data = datas[0];
            string[] parts = data.Split(',');

            if (buildingID >= 0 && buildingID < parts.Length)
            {
                int currentCount = int.Parse(parts[buildingID]);

                if (currentCount > 0) // Ensure we don't go below zero
                {
                    currentCount--;
                    parts[buildingID] = currentCount.ToString();

                    // Recreate the string and save it back to the file
                    string updatedData = string.Join(",", parts);
                    File.WriteAllText(BlueprintFilePath, updatedData);

                    Start();
                }
                else
                {
                    Start();
                    Debug.LogWarning("Cannot subtract. Blueprint count is already zero.");
                }
            }
            else
            {
                Debug.LogError("Invalid Building ID.");
            }
        }
        else
        {
            Debug.LogError("Blueprint file not found.");
        }
    }

    public void AddBlueprint(int buildingID)
    {
        if (File.Exists(BlueprintFilePath))
        {
            string[] datas = File.ReadAllLines(BlueprintFilePath);
            string data = datas[0];
            string[] parts = data.Split(',');

            if (buildingID >= 0 && buildingID < parts.Length)
            {
                int currentCount = int.Parse(parts[buildingID]);

                currentCount++;
                parts[buildingID] = currentCount.ToString();

                // Recreate the string and save it back to the file
                string updatedData = string.Join(",", parts);
                File.WriteAllText(BlueprintFilePath, updatedData);

                Start();
            }
            else
            {
                Debug.LogError("Invalid Building ID.");
            }
        }
        else
        {
            Debug.LogError("Blueprint file not found.");
        }
    }
}
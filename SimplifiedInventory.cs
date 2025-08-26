using System.IO;
using TMPro;
using UnityEngine;

public class SimplifiedInventory : MonoBehaviour
{
    private string inventoryFilePath; // Path to the inventory save file
    int[] InventoryID = new int[34];

    void Start()
    {
        // Define the path to save the file
        inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "inventory.txt");

        // Check if the file exists, otherwise initialize with 0s
        if (!File.Exists(inventoryFilePath))
        {
            //Debug.Log("File does not exist. Initializing with 34 zeros.");
            InventoryID[0] = 10;
            SaveIntegersToFile(InventoryID);
        }
    }

    // Read integers from the file
    private int[] ReadIntegersFromFile()
    {
        try
        {
            string[] lines = File.ReadAllLines(inventoryFilePath);
            int[] integers = new int[lines.Length];
            for (int i = 0; i < lines.Length; i++)
            {
                integers[i] = int.Parse(lines[i]);
            }
            return integers;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to read integers from file: " + e.Message);
            return new int[0]; // Return an empty array on failure
        }
    }

    // Method to save integers into a text file
    private void SaveIntegersToFile(int[] integers)
    {
        try
        {
            // Convert the array of integers to a single string with line breaks
            string content = string.Join("\n", integers);

            // Write the content to the file
            //File.WriteAllText(inventoryFilePath, content);
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save integers to file: " + e.Message);
        }
    }
}

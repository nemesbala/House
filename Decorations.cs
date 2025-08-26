using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class Decorations : MonoBehaviour
{
    public int houseIndex; // The index of the house in the BuildingSystem array
    public string houseID; // Unique ID for this house
    private string saveFilePath;
    public GameObject objectToEnable;
    public GameObject EmptyPlot;
    public GameObject Construction;
    private CashDisplay cashDisplay;
    public int ConstructionPlacementCost;
    public int MoodToTake;
    public int PowerToTake;
    public int MoodToGive;
    public int PowerToGive;
    public BlueprintChecker blueprintChecker;
    public int HouseArrayID;
    public MoodManager Mood;
    public PowerManager Power;

    void Start()
    {
        cashDisplay = FindObjectOfType<CashDisplay>();

        // Generate a unique ID if it doesn't already exist
        if (string.IsNullOrEmpty(houseID))
        {
            houseID = System.Guid.NewGuid().ToString();
        }

        if (!Directory.Exists(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building")))
        {
            Directory.CreateDirectory(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building"));
        }
        saveFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building", houseID + ".txt");

        // Load house data from a file
        string directoryPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building");
        string filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building", houseID + ".txt");

        if (File.Exists(filePath))
        {
            gameObject.layer = 0;
            string[] datas = File.ReadAllLines(filePath);
            string data = datas[0];
            string[] parts = data.Split(',');
            houseIndex = int.Parse(parts[0]);
            Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        }
        else
        {
            Debug.LogError($"File not found: {filePath}");
            SaveHouseData();
        }

        //----------Find and Assign Mood and Power Sliders------------
        Mood = FindObjectOfType<MoodManager>();
        Power = FindObjectOfType<PowerManager>();
        GameObject GblueprintChecker = GameObject.Find("Canvas (Has Sound Attached!)");
        blueprintChecker = GblueprintChecker.GetComponent<BlueprintChecker>();
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        SaveHouseData();
        cashDisplay.SubtractCash(ConstructionPlacementCost);
        Power.IncreaseNegative(PowerToTake);
        Mood.IncreasePositive(MoodToGive);
        Power.IncreasePositive(PowerToGive);
        blueprintChecker.SubtractBlueprint(HouseArrayID);
        gameObject.layer = 0;
    }

    // This method is triggered when the object is clicked
    void OnClick()
    {

    }

    // Save the house's data to a file
    public void SaveHouseData()
    {
        string data = $"{houseIndex},{transform.position.x},{transform.position.y},{transform.position.z}";
        File.WriteAllText(saveFilePath, data);
        Debug.Log($"House {houseID} data saved to: {saveFilePath}");
    }

    // Load house data from a string
    public void LoadHouseData(string data)
    {
        string[] parts = data.Split(',');
        houseIndex = int.Parse(parts[0]);
        Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        transform.position = position;
    }
}


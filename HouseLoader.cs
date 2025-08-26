using System.IO;
using UnityEngine;

public class HouseLoader : MonoBehaviour
{
    public BuildingSystem buildingSystem; // Reference to the BuildingSystem script

    void Start()
    {
        if (!Directory.Exists(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building")))
        {
            Directory.CreateDirectory(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building"));
        }

        LoadAllHouses();
    }

    // Load all houses from the save directory
    void LoadAllHouses()
    {
        string directoryPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building");
        string[] files = Directory.GetFiles(directoryPath, "*.txt");

        foreach (string file in files)
        {
            string[] datas = File.ReadAllLines(file);
            string data = datas[0];
            //Debug.Log(file + " has the following data: " + data);
            LoadHouseFromData(data, Path.GetFileNameWithoutExtension(file));
        }
    }

    // Load a single house from its saved data
    void LoadHouseFromData(string data, string houseID)
    {
        string[] parts = data.Split(',');

        //Debug.Log($"Data parts length: {parts.Length}");
        //Debug.Log(parts[1]);

        int houseIndex = int.Parse(parts[0]);
        Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));

        // Instantiate the house
        GameObject housePrefab = buildingSystem.buildings[houseIndex];
        GameObject houseInstance = Instantiate(housePrefab, position, Quaternion.identity);

        // Initialize the house with the loaded data
        House houseScript = houseInstance.GetComponent<House>();
        if (houseScript != null)
        {
            houseScript.houseID = houseID; // Assign the loaded ID
            houseScript.LoadHouseData(data);
        }

        Factory FactoryScript = houseInstance.GetComponent<Factory>();
        if(FactoryScript != null)
        {
            FactoryScript.houseID = houseID;
            FactoryScript.LoadHouseData(data);
        }

        Road RoadScript = houseInstance.GetComponent<Road>();
        if(RoadScript != null)
        {
            RoadScript.houseID = houseID;
            RoadScript.LoadHouseData(data);
        }

        Obstacle ObstacleScript = houseInstance.GetComponent<Obstacle>();
        if (ObstacleScript != null)
        {
            ObstacleScript.houseID = houseID;
            ObstacleScript.LoadHouseData(data);
        }
    }
}

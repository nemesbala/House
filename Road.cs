using System.IO;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;

public class Road : MonoBehaviour
{
    [Header("Construction Checker")]
    [Tooltip("You can move the area where the place will be checked for available space.")]
    public Vector3 offset;
    [Tooltip("You can resize the area where the place will be checked for available space.")]
    public Vector3 size;
    //private Camera mainCamera;
    [Tooltip("Auto assigned when started.")]
    public XPManager xpManager;
    [Tooltip("Auto assigned when started.")]
    public WPandSP wpandsp;
    [Tooltip("The index of the house in the BuildingSystem array.")]
    public int houseIndex;
    [Tooltip("Unique ID for this house. Auto generated upon construction.")]
    public string houseID;
    [Tooltip("Assign the Text Field that will display the House ID.")]
    public TextMeshProUGUI IDText;
    private string saveFilePath;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    [Tooltip("This is the UI, that is going to be enabled. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToEnable;
    [Tooltip("This is the UI, that is going to be enabled, when the mouse hover over the building. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToHoverEnable;

    [Header("Other Stuff")]
    private CashDisplay cashDisplay;
    public int ConstructionPlacementCost;
    public RoadConnectionChecker roadConnectionChecker;

    [Header("Mood & Power")]
    [Tooltip("Check this box if this building GIVES power. Leave it unchecked if it TAKES power!")]
    public bool PowerGiver;
    [Tooltip("Check this box if this building GIVES mood. Leave it unchecked if it TAKES mood!")]
    public bool MoodGiver;
    public int MoodToTake01;
    public int PowerToTake01;
    public int MoodToGive01;
    public int PowerToGive01;

    [Tooltip("Auto assigned when started.")]
    public MoodManager Mood;
    [Tooltip("Auto assigned when started.")]
    public PowerManager Power;

    [Header("Something")]
    [Tooltip("Auto assigned when started.")]
    public BlueprintChecker blueprintChecker;
    public int HouseArrayID;
    public AudioClip clip;
    public TextMeshProUGUI PEffects;
    public TextMeshProUGUI MEffects;
    public AudioMixer audioMixer;
    public bool CanBeClickedOn = false;

    void Start()
    {
        //mainCamera = Camera.main;
        cashDisplay = FindObjectOfType<CashDisplay>();
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        //Debug.Log($"Vector3: {offset}");
        // Generate a unique ID if it doesn't already exist
        if (string.IsNullOrEmpty(houseID))
        {
            houseID = System.Guid.NewGuid().ToString();
        }

        if (xpManager == null)
        {
            xpManager = FindObjectOfType<XPManager>();
        }

        if (wpandsp == null)
        {
            wpandsp = FindObjectOfType<WPandSP>();
        }

        if (IDText != null)
        {
            IDText.text = $"ID: ${houseID}";
        }
        else
        {
            Debug.LogError("IDText component is not assigned.");
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
            CanBeClickedOn = true;
            string[] datas = File.ReadAllLines(filePath);
            string data = datas[0];
            //Debug.Log(data + "HERE IS WHAT IS IMPORTANTE!");
            string[] parts = data.Split(',');
            gameObject.layer = 0;
            // Parsing the data from the file
            houseIndex = int.Parse(parts[0]);
            Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        }

        //The following section replaces the call to LoadFutureTime()
        //string filePathq = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        //----------Find and Assign Mood and Power Sliders------------
        Mood = FindObjectOfType<MoodManager>();
        Power = FindObjectOfType<PowerManager>();

        GameObject GblueprintChecker = GameObject.Find("Canvas (Has Sound Attached!)");
        blueprintChecker = GblueprintChecker.GetComponent<BlueprintChecker>();
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        //Debug.Log(houseID);
        SaveHouseData();
        cashDisplay.SubtractCash(ConstructionPlacementCost);
        if (PowerGiver)
        {
            Power.IncreasePositive(PowerToGive01);
        }
        else if (!PowerGiver)
        {
            Power.IncreaseNegative(PowerToTake01);
        }

        if (MoodGiver)
        {
            Mood.IncreasePositive(MoodToGive01);
        }
        else if (!MoodGiver)
        {
            Mood.IncreaseNegative(MoodToTake01);
        }
        blueprintChecker.SubtractBlueprint(HouseArrayID);
        gameObject.layer = 0;
        roadConnectionChecker.Start();
        MainRoadChecker.Instance.CollectBuildings();
        MainRoadChecker.Instance.RecheckAllBuildings();
        StartCoroutine(ResetPlacing());
    }

    public void OnMouseHover()
    {
        objectToHoverEnable.SetActive(true);
    }

    public void NoMouseHover()
    {
        objectToHoverEnable.SetActive(false);
    }

    private IEnumerator ResetPlacing()
    {
        yield return new WaitForEndOfFrame();
        CanBeClickedOn = true; // Allow UI clicks again
    }

    public void CloseWindow()
    {
        objectToEnable.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && objectToEnable.activeSelf == true)
        {
            objectToEnable.SetActive(false);
            publicBoolForPauseMenuOpen.isAnUIOpened = false;
        }
    }

    public void UIClosing()
    {
        publicBoolForPauseMenuOpen.isAnUIOpened = false;
    }

    // This method is triggered when the object is clicked
    public void OnClick()
    {
        // Enable the specified GameObject
        if (objectToEnable != null && CanBeClickedOn)
        {
            objectToEnable.SetActive(true);
            publicBoolForPauseMenuOpen.isAnUIOpened = true;
            if (PowerGiver)
            {
                PEffects.text = "+ " + PowerToGive01 + " Power";
            }
            else if (!PowerGiver)
            {
                PEffects.text = "- " + PowerToTake01 + " Power";
            }

            if (MoodGiver)
            {
                MEffects.text = "+ " + MoodToGive01 + " Mood";
            }
            else if (!MoodGiver)
            {
                MEffects.text = "- " + MoodToTake01 + " Mood";
            }
        }
    }

    public void DestroyBuilding()
    {
        string path = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building", houseID + ".txt");
        if (File.Exists(path))
        {
            // Delete the folder and its contents
            File.Delete(path); // 'true' to delete recursively
        }
        else
        {
            Debug.LogWarning("File does not exist: " + path);
        }

        if (PowerGiver)
        {
            Power.DecreasePositive(PowerToGive01);
        }
        else if (!PowerGiver)
        {
            Power.DecreaseNegative(PowerToTake01);
        }

        if (MoodGiver)
        {
            Mood.DecreasePositive(MoodToGive01);
        }
        else if (!MoodGiver)
        {
            Mood.DecreaseNegative(MoodToTake01);
        }

        objectToEnable.SetActive(false);
        MainRoadChecker.Instance.CollectBuildings();
        MainRoadChecker.Instance.RecheckAllBuildings();
        UIClosing();
        Destroy(gameObject);
    }

    float ReadOverallFloat(string path)
    {
        if (!File.Exists(path))
        {
            //Debug.LogError("File not found at path: " + path);
            return 1f;
        }

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

    // Save the house's data to a file
    public void SaveHouseData()
    {
        List<string> data = new List<string>();
        data.Add($"{houseIndex},{transform.position.x},{transform.position.y},{transform.position.z}");
        try
        {
            string[] lines = File.ReadAllLines(saveFilePath);
            if (lines.Length > 1)
            {
                data.Add(lines[1]);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
        File.WriteAllLines(saveFilePath, data);
    }
    
    // Load house data from a string
    public void LoadHouseData(string data)
    {
        string[] parts = data.Split(',');
        houseIndex = int.Parse(parts[0]);
        Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        transform.position = position;
        //Debug.Log("Succesful loading log from building: " + houseID + "BuildingIndex: " + houseIndex + "Current stage: " + Stage + "Current loacation: " + position);
    }
}
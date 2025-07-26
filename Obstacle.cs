using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class Obstacle : MonoBehaviour
{
    [Tooltip("The index of the house in the BuildingSystem array.")]
    public int houseIndex;
    [Tooltip("Unique ID for this house. Auto generated upon construction.")]
    public string houseID;
    [Tooltip("Assign the Text Field that will display the House ID.")]
    public TextMeshProUGUI IDText;
    [Tooltip("Auto assigned when started.")]
    public XPManager xpManager;
    [Tooltip("Auto assigned when started.")]
    public WPandSP wpandsp;
    [Tooltip("Current stage of the house. Leave it as 0, unless you know what you are doing!")]
    public int Stage;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    private string saveFilePath;
    private string inventoryFilePath;
    public int TimeToGive;

    [Header("Building Stages")]
    [Tooltip("This is the UI, that is going to be enabled. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToEnable;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 0")]
    public GameObject EmptyPlot;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 1")]
    public GameObject Construction01;

    [Header("Upgrade Buttons")]
    [Tooltip("Assign here the correct button, that enables construction from empty to Stage 1.")]
    public GameObject Button0;
    [Tooltip("Assign here the correct button, that finishes construction from empty to Stage 1.")]
    public GameObject Button1;

    [Header("Materials to give")]
    public int NeddedWood01;
    public int NeddedWoodenBeam01;
    public int NeddedBrick01;
    public int NeddedConcrete01;

    [Header("Construction Times")]
    public int TimeToConstruct01;

    [Header("Other Stuff")]
    public DateTime futureTime;
    [Tooltip("This is the 3D icon above the building that shows up, when the construction is finished")]
    public GameObject ConstructionIcon;
    public TextMeshProUGUI timerText;
    public int HouseArrayID;
    public AudioClip clip;
    public TextMeshProUGUI MaterialsNeededText;
    public AudioMixer audioMixer;
    private bool CanBeClickedOn = false;

    void Start()
    {
        cashDisplay = FindObjectOfType<CashDisplay>();
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        // Generate a unique ID if it doesn't already exist
        if (string.IsNullOrEmpty(houseID))
        {
            Stage = 0;
            ResidentType = 0;
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

        if (!Directory.Exists(Path.Combine(Application.persistentDataPath, "Building")))
        {
            Directory.CreateDirectory(Path.Combine(Application.persistentDataPath, "Building"));
        }
        saveFilePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        // Load house data from a file
        string directoryPath = Path.Combine(Application.persistentDataPath, "Building");
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

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
            Stage = int.Parse(parts[4]);
        }

        switch (Stage)
        {
            case 1:
                {
                    Stage = 1;
                    EmptyPlot.SetActive(false);
                    Construction.SetActive(true);
                    break;
                }
            case 2:
                {
                    Stage = 2;
                    EmptyPlot.SetActive(false);
                    Construction.SetActive(false);
                    Stage1.SetActive(true);
                    break;
                }
        }

        string filePathq = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        if (File.Exists(filePathq))
        {
            string[] lines = File.ReadAllLines(filePathq);
            //Debug.Log(lines.Length);
            if (lines.Length > 1)
            {
                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
            }
        }
    }

    void Update()
    {
        if (Stage == 1)
        {
            CheckTimeAndEnableObject();
            UpdateTimerText();
        }
        else if (Stage == 2)
        {
            DateTime currentTime = DateTime.Now;

            string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
            string[] lines = File.ReadAllLines(filePath);
            string lastSavedTimeString = lines[1];
            DateTime futureTimeC = DateTime.Parse(lastSavedTimeString);
            UpdateTimerText();
            if (currentTime >= futureTimeC)
            {
                ConstructionIcon.SetActive(true);
                Button1.SetActive(true);
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape) && objectToEnable.activeSelf == true)
        {
            objectToEnable.SetActive(false);
            publicBoolForPauseMenuOpen.isAnUIOpened = false;
        }
    }

    void LoadFutureTime()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 1)
            {

                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
            }
        }
    }

    void CheckTimeAndEnableObject()
    {
        DateTime currentTime = DateTime.Now;

        if (currentTime >= futureTime)
        {
            if (CashIcon != null && !CashIcon.activeSelf && !IsDecorationOrPowerPlant)
            {
                CashIcon.SetActive(true);
                //Debug.Log($"{CashIcon.name} has been enabled at {currentTime}.");
            }
        }
    }

    void SaveFutureTime()
    {
        if (Stage == 1)
        {
            DateTime currentTime = DateTime.Now;
            futureTime = currentTime.AddSeconds(TimeToGive);
            string futureTimeString = futureTime.ToString("yyyy-MM-dd HH:mm:ss");

            // Define the path to the file
            string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }

            //Debug.Log($"Future time appended: {futureTimeString} to file: {filePath}");
        }
    }

    void UpdateTimerText()
    {
        DateTime currentTime = DateTime.Now;
        TimeSpan remainingTime = futureTime - currentTime;

        if (remainingTime.TotalSeconds > 0)
        {
            // Format the remaining time as HH:MM:SS
            string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
            remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);

            // Update the TMPro text component
            timerText.text = timeString;
        }
        else
        {
            // If the time has elapsed, display 00:00:00
            timerText.text = "00:00:00";
        }
    }

    public void UpgradeFinish01()
    {
        Stage = 1;
        EmptyPlot.SetActive(false);
        Construction.SetActive(true);
        Button0.SetActive(false);
        objectToEnable.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct01);
        futureTime = currentTime.AddSeconds(TimeToConstruct01);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");
        //Debug.Log(futureTimeC);
        // Define the path to the file
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        UIClosing();
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "1";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }

            //--Subtract the materials--
            string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
            string[] datas = File.ReadAllLines(inventoryFilePath);

            int Wood = int.Parse(datas[0]);
            int WoodenBeam = int.Parse(datas[1]);
            int WoodenPanel = int.Parse(datas[2]);
            int Brick = int.Parse(datas[12]);
            int RoofTile = int.Parse(datas[13]);
            int Concrete = int.Parse(datas[14]);
            int Tile = int.Parse(datas[15]);
            int Nail = int.Parse(datas[20]);
            int Steel = int.Parse(datas[21]);
            int MetalSheet = int.Parse(datas[22]);
            int SteelRod = int.Parse(datas[23]);
            int Screw = int.Parse(datas[24]);

            Wood = Wood - NeddedWood01;
            WoodenBeam = WoodenBeam - NeddedWoodenBeam01;
            WoodenPanel = WoodenPanel - NeddedWoodenPanel01;
            Brick = Brick - NeddedBrick01;
            RoofTile = RoofTile - NeddedRoofTile01;
            Concrete = Concrete - NeddedConcrete01;
            Tile = Tile - NeddedTile01;
            Nail = Nail - NeddedNails01;
            Steel = Steel - NeddedSteel01;
            MetalSheet = MetalSheet - NeddedMetalSheet01;
            SteelRod = SteelRod - NeddedSteelRod01;
            Screw = Screw - NeddedScrew01;

            datas[0] = Wood.ToString();
            datas[1] = WoodenBeam.ToString();
            datas[2] = WoodenPanel.ToString();
            datas[12] = Brick.ToString();
            datas[13] = RoofTile.ToString();
            datas[14] = Concrete.ToString();
            datas[15] = Tile.ToString();
            datas[20] = Nail.ToString();
            datas[21] = Steel.ToString();
            datas[22] = MetalSheet.ToString();
            datas[23] = SteelRod.ToString();
            datas[24] = Screw.ToString();

            using (StreamWriter Wwriter = new StreamWriter(inventoryFilePath))
            {
                foreach (string element in datas)
                {
                    Wwriter.WriteLine(element); // Write each element on a new line
                }
            }
        }
        else
        {
            Debug.LogError($"File not found at: {filePath}");
        }
    }

    public void Upgrade01()
    {
        Stage = 2;
        UIClosing();
        objectToEnable.SetActive(false);
        EmptyPlot.SetActive(false);
        Construction.SetActive(false);
        Stage1.SetActive(true);
        ConstructionIcon.SetActive(false);
        SaveHouseData();
        SaveFutureTime();
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        SaveHouseData();
        CheckRoadConnection();
        ResidentType = 0;
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
        CanBeClickedOn = true;
    }

    public void UIClosing()
    {
        publicBoolForPauseMenuOpen.isAnUIOpened = false;
    }

    public void OnClick()
    {
        // Enable the specified GameObject
        if (objectToEnable != null && CanBeClickedOn)
        {
            if (CashIcon.activeSelf == false)
            {
                objectToEnable.SetActive(true);
                publicBoolForPauseMenuOpen.isAnUIOpened = true;

                Button0.SetActive(false);
                Button1.SetActive(false);
            }
            else if (CashIcon.activeSelf)
            {
                CashIcon.SetActive(false);
                SaveFutureTime();
                if (clip != null)
                {
                    string SFXFilePath;
                    SFXFilePath = Path.Combine(Application.persistentDataPath, "Audio.txt");
                    string[] datas = File.ReadAllLines(SFXFilePath);
                    float volume = float.Parse(datas[0]);
                    GameObject Cam = GameObject.Find("Main Camera");
                    AudioSource.PlayClipAtPoint(clip, Cam.transform.position, volume);
                }


                if (cashDisplay != null)
                {
                    xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1));
                    wpandsp.ChangeWPoints(Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1));
                    AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "Worker Point";
                    animatedFloatingTextUponCollection.SetActive(false);
                    animatedFloatingTextUponCollection.SetActive(true);
                }
                else
                {
                    Debug.LogError("CashDisplay script not found in the scene.");
                }
            }
        }
    }

    // Save the house's data to a file
    public void SaveHouseData()
    {
        List<string> data = new List<string>();
        data.Add($"{houseIndex},{transform.position.x},{transform.position.y},{transform.position.z},{Stage}");
        try
        {
            string[] lines = File.ReadAllLines(saveFilePath);
            if (lines.Length > 1)
            {
                data.Add(lines[1]);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
            Debug.Log("I am here at Line:450");
        }
        File.WriteAllLines(saveFilePath, data);
    }

    // Load house data from a string
    public void LoadHouseData(string data)
    {
        string[] parts = data.Split(',');
        houseIndex = int.Parse(parts[0]);
        Stage = int.Parse(parts[4]);
        Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        transform.position = position;
    }
}

using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;

public class Factory : MonoBehaviour
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
    [Tooltip("Current stage of the house. Leave it as 0, unless you know what you are doing!")]
    public int Stage;
    [Tooltip("Unique ID for this house. Auto generated upon construction.")]
    public string houseID;
    [Tooltip("Assign the Text Field that will display the House ID.")]
    public TextMeshProUGUI IDText;
    private string saveFilePath;
    [Tooltip("Check this box if this is a decoration or power plant.")]
    public bool IsDecorationOrPowerPlant;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    public bool isBlueprintRefundable;

    [Header("Building Stages")]
    [Tooltip("This is the UI, that is going to be enabled. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToEnable;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 0")]
    public GameObject EmptyPlot;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 1")]
    public GameObject Construction;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 2")]
    public GameObject Stage1;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 3")]
    public GameObject Construction12;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 4")]
    public GameObject Stage2;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 5")]
    public GameObject Construction23;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 6")]
    public GameObject Stage3;

    [Header("Upgrade Buttons")]
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button0;
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button1;
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button2;
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button3;
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button4;
    [Tooltip("Assign here the correct button, that enables construction or finishes construction.")]
    public GameObject Button5;
    private CashDisplay cashDisplay;

    [Header("Materials Needed")]
    public int NeddedWood01;
    public int NeddedWood12;
    public int NeddedWood23;
    public int NeddedWoodenBeam01;
    public int NeddedWoodenBeam12;
    public int NeddedWoodenBeam23;
    public int NeddedWoodenPanel01;
    public int NeddedWoodenPanel12;
    public int NeddedWoodenPanel23;
    public int NeddedNails01;
    public int NeddedNails12;
    public int NeddedNails23;
    public int NeddedBrick01;
    public int NeddedBrick12;
    public int NeddedBrick23;
    public int NeddedRoofTile01;
    public int NeddedRoofTile12;
    public int NeddedRoofTile23;
    public int NeddedConcrete01;
    public int NeddedConcrete12;
    public int NeddedConcrete23;
    public int NeddedTile01;
    public int NeddedTile12;
    public int NeddedTile23;
    public int NeddedSteel01;
    public int NeddedSteel12;
    public int NeddedSteel23;
    public int NeddedMetalSheet01;
    public int NeddedMetalSheet12;
    public int NeddedMetalSheet23;
    public int NeddedSteelRod01;
    public int NeddedSteelRod12;
    public int NeddedSteelRod23;
    public int NeddedScrew01;
    public int NeddedScrew12;
    public int NeddedScrew23;

    [Header("Construction Times")]
    public int TimeToConstruct01;
    public int TimeToConstruct12;
    public int TimeToConstruct23;

    [Header("Other Stuff")]
    public DateTime futureTime;
    [Tooltip("This is the 3D icon above the building that shows up, when the rent is ready")]
    public GameObject CashIcon;
    [Tooltip("This is the 3D icon above the building that shows up, when the construction is finished")]
    public GameObject ConstructionIcon;
    public TextMeshProUGUI timerText;
    public int ConstructionPlacementCost;
    public GameObject[] ProductsUI;
    public GameObject UnableToWorkText;

    [Header("Mood & Power")]
    [Tooltip("Check this box if this building GIVES power. Leave it unchecked if it TAKES power!")]
    public bool PowerGiver;
    [Tooltip("Check this box if this building GIVES mood. Leave it unchecked if it TAKES mood!")]
    public bool MoodGiver;
    public int MoodToTake01;
    public int PowerToTake01;
    public int MoodToTake2;
    public int PowerToTake2;
    public int MoodToTake3;
    public int PowerToTake3;
    public int MoodToGive01;
    public int PowerToGive01;
    public int MoodToGive2;
    public int PowerToGive2;
    public int MoodToGive3;
    public int PowerToGive3;
    public MoodManager Mood;
    public PowerManager Power;

    [Header("Something")]
    public bool CheckSPInstedOfWP = false;
    public BlueprintChecker blueprintChecker;
    public int HouseArrayID;
    public AudioClip clip;
    public TextMeshProUGUI MaterialsNeededText;
    public TextMeshProUGUI PEffects;
    public TextMeshProUGUI MEffects;
    public AudioMixer audioMixer;
    public bool CanBeClickedOn = false;
    private int productID;
    private int productAmmount;
    private int WPToSubtract;
    public FactoryButtonData factoryButtonData;
    public GameObject[] ProductionStarterButtons;
    public FactoryEnableCheck factoryenablecheck;
    public GameObject CancelButton;
    public TextMeshProUGUI Estimate;
    public TextMeshProUGUI MaxEstimate;
    public GameObject ButtonBlockers;
    public Button[] buttons;
    public int[] keys;
    public GameObject[] Collectiontxt;
    public GameObject DynamyCollectionObject;
    public TMP_Text dynamicCollectiontxt;

    [Header("Road Connection")]
    public bool isConnectedToRoad = false;
    public Vector3 frontOffset = new Vector3(0, 0, 1f);
    public Vector3 frontSize = new Vector3(1f, 1f, 0.5f);
    public Vector3 backOffset = new Vector3(0, 0, -1f);
    public Vector3 backSize = new Vector3(1f, 1f, 0.5f);
    public Vector3 leftOffset = new Vector3(-1f, 0, 0);
    public Vector3 leftSize = new Vector3(0.5f, 1f, 1f);
    public Vector3 rightOffset = new Vector3(1f, 0, 0);
    public Vector3 rightSize = new Vector3(0.5f, 1f, 1f);
    private bool[] sideResults = new bool[4];
    public GameObject NoRoadIcon;
    public GameObject ButtonBlockersForRoad;

    void Start()
    {
        cashDisplay = FindObjectOfType<CashDisplay>();
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        // Generate a unique ID if it doesn't already exist
        if (string.IsNullOrEmpty(houseID))
        {
            Stage = 0;
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
        //Debug.Log(filePath);
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

            if (datas.Length > 3)
            {
                ButtonBlockers.SetActive(true);
            }
            else
            {
                ButtonBlockers.SetActive(false);
            }
        }
        else
        {
            //Debug.LogError($"File not found: {filePath}");
        }

        switch(Stage)
        {
            case 1:
                {
                    Stage = 1;
                    EmptyPlot.SetActive(false);
                    Construction.SetActive(true);
                    //SaveHouseData();
                    break;
                }
            case 2:
                {
                    Stage = 2;
                    EmptyPlot.SetActive(false);
                    Construction.SetActive(false);
                    Stage1.SetActive(true);
                    //SaveHouseData();
                    break;
                }
            case 3:
                {
                    Stage = 3;
                    EmptyPlot.SetActive(false);
                    Construction12.SetActive(true);
                    break;
                }
            case 4:
                {
                    Stage = 4;
                    EmptyPlot.SetActive(false);
                    Stage2.SetActive(true);
                    break;
                }
            case 5:
                {
                    Stage = 5;
                    EmptyPlot.SetActive(false);
                    Construction23.SetActive(true);
                    break;
                }
            case 6:
                {
                    Stage = 6;
                    EmptyPlot.SetActive(false);
                    Stage3.SetActive(true);
                    break;
                }
        }
        //The following section replaces the call to LoadFutureTime()
        string filePathq = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        if (File.Exists(filePathq))
        {
            CanBeClickedOn = true;
            string[] lines = File.ReadAllLines(filePathq);
            //Debug.Log(lines.Length);
            if (lines.Length > 1)
            {
                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
                foreach (GameObject obj in ProductionStarterButtons)
                {
                    obj.SetActive(false);
                }
            }
        }
        else
        {
            //Debug.LogError($"House save file not found at: {filePathq}");
        }
        //----------Find and Assign Mood and Power Sliders------------
        Mood = FindObjectOfType<MoodManager>();
        Power = FindObjectOfType<PowerManager>();

        GameObject GblueprintChecker = GameObject.Find("Canvas (Has Sound Attached!)");
        blueprintChecker = GblueprintChecker.GetComponent<BlueprintChecker>();
        CheckRoadConnection();
        if (File.Exists(filePath))
        {
            string[] llines = File.ReadAllLines(filePath);
            if (llines.Length > 3)
            {
                string[] lines = File.ReadAllLines(filePath);
                int ammount = Int32.Parse(lines[3]);
                string PowerFilePath;
                PowerFilePath = Path.Combine(Application.persistentDataPath, "powerLevels.txt");
                float CurrentPower = ReadOverallFloat(PowerFilePath);
                string MoodFilePath;
                MoodFilePath = Path.Combine(Application.persistentDataPath, "moodLevels.txt");
                float CurrentMood = ReadOverallFloat(MoodFilePath);

                int level;

                if (Stage == 2)
                {
                    level = 1;
                }
                else if (Stage == 4)
                {
                    level = 2;
                }
                else if (Stage == 6)
                {
                    level = 4;
                }
                else
                {
                    level = 0;
                }

                Estimate.text = ((int)Math.Ceiling(ammount * CurrentPower * CurrentMood * level)).ToString();
                MaxEstimate.text = (ammount).ToString();
            }
        }
    }

    void Update()
    {
        if(isConnectedToRoad)
        {
            if (Stage == 2 || Stage == 4 || Stage == 6)
            {
                CheckTimeAndEnableObject();
                UpdateTimerText();
            }
            else if (Stage == 1)
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
            else if (Stage == 3)
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
                    Button3.SetActive(true);
                }
            }
            else if (Stage == 5)
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
                    Button5.SetActive(true);
                }
            }
        }
        else
        {
            timerText.text = "Road missing!";
        }

        if (Input.GetKeyDown(KeyCode.Escape) && objectToEnable.activeSelf == true)
        {
            objectToEnable.SetActive(false);
            publicBoolForPauseMenuOpen.isAnUIOpened = false;
        }
    }

    public void CheckRoadConnection()
    {
        isConnectedToRoad = false;

        // All 4 sides' centers and sizes
        Vector3[] offsets = { frontOffset, backOffset, leftOffset, rightOffset };
        Vector3[] sizes = { frontSize, backSize, leftSize, rightSize };

        for (int i = 0; i < 4; i++)
        {
            Vector3 boxCenter = transform.position + offsets[i];
            Collider[] hits = Physics.OverlapBox(boxCenter, sizes[i] / 2f, Quaternion.identity);

            sideResults[i] = false;

            foreach (var hit in hits)
            {
                if (hit.CompareTag("Road"))
                {
                    sideResults[i] = true;
                    isConnectedToRoad = true;
                    break;
                }
            }
        }
        if (isConnectedToRoad)
        {
            NoRoadIcon.SetActive(false);
            ButtonBlockersForRoad.SetActive(false);
        }
        else
        {
            NoRoadIcon.SetActive(true);
            ButtonBlockersForRoad.SetActive(true);
        }
        //Debug.Log($"{gameObject.name} road connection: {isConnectedToRoad}");
    }

    // Draw debug gizmos in Scene view
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Vector3[] offsets = { frontOffset, backOffset, leftOffset, rightOffset };
        Vector3[] sizes = { frontSize, backSize, leftSize, rightSize };

        for (int i = 0; i < 4; i++)
        {
            Vector3 boxCenter = transform.position + offsets[i];
            Gizmos.color = (sideResults[i]) ? Color.green : Color.red;
            Gizmos.DrawWireCube(boxCenter, sizes[i]);
        }
    }

    public void Calculate(int product)
    {
        string PowerFilePath;
        PowerFilePath = Path.Combine(Application.persistentDataPath, "powerLevels.txt");
        float CurrentPower = ReadOverallFloat(PowerFilePath);

        string MoodFilePath;
        MoodFilePath = Path.Combine(Application.persistentDataPath, "moodLevels.txt");
        float CurrentMood = ReadOverallFloat(MoodFilePath);

        int level;

        if(Stage == 2)
        {
            level = 1;
        }
        else if(Stage == 4)
        {
            level = 2;
        }
        else if(Stage == 6)
        {
            level = 4;
        }
        else
        {
            level = 0;
        }

        Estimate.text = ((int)Math.Ceiling(product * CurrentPower * CurrentMood * level)).ToString();
        MaxEstimate.text = (product * level).ToString();
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
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        string[] lines = File.ReadAllLines(filePath);
        DateTime currentTime = DateTime.Now;
        if(lines.Length > 1)
        {
            if (currentTime >= futureTime)
            {
                if (CashIcon != null && !CashIcon.activeSelf && !IsDecorationOrPowerPlant)
                {
                    CashIcon.SetActive(true);
                    //Debug.Log($"{CashIcon.name} has been enabled at {currentTime}.");
                }
            }
        }
    }

    public void SaveFutureTime(int TimeToGive) // The UI should have a button loaded with the number of seconds the production is going to take
    {
        if(Stage == 2 || Stage == 4 || Stage == 6)
        {
            DateTime currentTime = DateTime.Now;
            futureTime = currentTime.AddSeconds(TimeToGive);
            string futureTimeString = futureTime.ToString("yyyy-MM-dd HH:mm:ss");
            ButtonBlockers.SetActive(true);
            //Find the active product UI
            foreach (GameObject obj in ProductsUI)
            {
                if (obj.activeSelf) // Check if this GameObject is active.
                {
                    // Access the script attached to this GameObject.
                    factoryButtonData = obj.GetComponent<FactoryButtonData>();
                    if (factoryButtonData != null)
                    {
                        productID = factoryButtonData.ID(); // Call the desired function.
                        productAmmount = factoryButtonData.Ammount(); // Call the desired function.
                        WPToSubtract = factoryButtonData.WP(); // Call the desired function.

                        if(CheckSPInstedOfWP)
                        {
                            wpandsp.SubtracktSPoints(WPToSubtract);
                        }
                        else
                        {
                            wpandsp.SubtrackWPoints(WPToSubtract);
                        }
                    }
                }
            }

            // Define the path to the file
            string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                writer.WriteLine(productID);
                writer.WriteLine(productAmmount);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }
            foreach (GameObject obj in ProductionStarterButtons)
            {
                obj.SetActive(false);
            }
        }
    }

    public void CancelProduction()
    {
        // Define the path to the file
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        // Read all lines from the file
        string[] lines = File.ReadAllLines(filePath);
        using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
        {
            writer.WriteLine(lines[0]);
            //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
        }
        futureTime = DateTime.MinValue;
        timerText.text = "00:00:00";
        factoryenablecheck.OnEnable();

        ButtonBlockers.SetActive(false);
    }

    // Method to update the TMPro text with the remaining time
    void UpdateTimerText()
    {
        DateTime currentTime = DateTime.Now;
        if(futureTime != null)
        {
            TimeSpan remainingTime = futureTime - currentTime;

            if (remainingTime.TotalSeconds > 0)
            {
                // Format the remaining time as HH:MM:SS
                string timeString = string.Format("{0:D2}:{1:D2}:{2:D2}",
                remainingTime.Hours, remainingTime.Minutes, remainingTime.Seconds);
                timerText.text = timeString;
            }
            else
            {
                timerText.text = "00:00:00";
            }
        }
        else
        {
            timerText.text = "00:00:00";
        }
    }

    public void Upgrade01()
    {
        Stage = 1;
        UIClosing();
        CancelButton.SetActive(false);
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

        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "1";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
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

    public void UpgradeFinish01()
    {
        Stage = 2;
        UIClosing();
        objectToEnable.SetActive(false);
        EmptyPlot.SetActive(false);
        Construction.SetActive(false);
        Stage1.SetActive(true);
        ConstructionIcon.SetActive(false);
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        string[] liness = File.ReadAllLines(filePath);
        using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
        {
            writer.WriteLine(liness[0]);
            //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
        }
        SaveHouseData();
    }

    public void Upgrade12()
    {
        Stage = 3;
        UIClosing();
        CancelButton.SetActive(false);
        objectToEnable.SetActive(false);
        Stage1.SetActive(false);
        Construction12.SetActive(true);
        Button2.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct12);
        futureTime = currentTime.AddSeconds(TimeToConstruct12);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");
        if(PowerGiver)
        {
            Power.DecreasePositive(PowerToGive01);
            Power.IncreasePositive(PowerToGive2);
        }
        else if(!PowerGiver)
        {
            Power.DecreaseNegative(PowerToTake01);
            Power.IncreaseNegative(PowerToTake2);
        }

        if(MoodGiver)
        {
            Mood.DecreasePositive(MoodToGive01);
            Mood.IncreasePositive(MoodToGive2);
        }
        else if(!MoodGiver)
        {
            Mood.DecreaseNegative(MoodToTake01);
            Mood.IncreaseNegative(MoodToTake2);
        }
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");

        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "3";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
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

            Wood = Wood - NeddedWood12;
            WoodenBeam = WoodenBeam - NeddedWoodenBeam12;
            WoodenPanel = WoodenPanel - NeddedWoodenPanel12;
            Brick = Brick - NeddedBrick12;
            RoofTile = RoofTile - NeddedRoofTile12;
            Concrete = Concrete - NeddedConcrete12;
            Tile = Tile - NeddedTile12;
            Nail = Nail - NeddedNails12;
            Steel = Steel - NeddedSteel12;
            MetalSheet = MetalSheet - NeddedMetalSheet12;
            SteelRod = SteelRod - NeddedSteelRod12;
            Screw = Screw - NeddedScrew12;

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

    public void UpgradeFinish12()
    {
        Stage = 4;
        UIClosing();
        objectToEnable.SetActive(false);
        Construction12.SetActive(false);
        Stage2.SetActive(true);
        ConstructionIcon.SetActive(false);
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        string[] liness = File.ReadAllLines(filePath);
        using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
        {
            writer.WriteLine(liness[0]);
            //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
        }
        SaveHouseData();
    }

    public void Upgrade23()
    {
        Stage = 5;
        UIClosing();
        CancelButton.SetActive(false);
        objectToEnable.SetActive(false);
        Stage2.SetActive(false);
        Construction23.SetActive(true);
        Button4.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct23);
        futureTime = currentTime.AddSeconds(TimeToConstruct23);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        if (PowerGiver)
        {
            Power.DecreasePositive(PowerToGive2);
            Power.IncreasePositive(PowerToGive3);
        }
        else if (!PowerGiver)
        {
            Power.DecreaseNegative(PowerToTake2);
            Power.IncreaseNegative(PowerToTake3);
        }

        if (MoodGiver)
        {
            Mood.DecreasePositive(MoodToGive2);
            Mood.IncreasePositive(MoodToGive3);
        }
        else if (!MoodGiver)
        {
            Mood.DecreaseNegative(MoodToTake2);
            Mood.IncreaseNegative(MoodToTake3);
        }
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "5";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
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

            Wood = Wood - NeddedWood23;
            WoodenBeam = WoodenBeam - NeddedWoodenBeam23;
            WoodenPanel = WoodenPanel - NeddedWoodenPanel23;
            Brick = Brick - NeddedBrick23;
            RoofTile = RoofTile - NeddedRoofTile23;
            Concrete = Concrete - NeddedConcrete23;
            Tile = Tile - NeddedTile23;
            Nail = Nail - NeddedNails23;
            Steel = Steel - NeddedSteel23;
            MetalSheet = MetalSheet - NeddedMetalSheet23;
            SteelRod = SteelRod - NeddedSteelRod23;
            Screw = Screw - NeddedScrew23;

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

    public void UpgradeFinish23()
    {
        Stage = 6;
        UIClosing();
        objectToEnable.SetActive(false);
        Construction23.SetActive(false);
        Stage3.SetActive(true);
        ConstructionIcon.SetActive(false);
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        string[] liness = File.ReadAllLines(filePath);
        using (StreamWriter writer = new StreamWriter(filePath, false)) // 'true' enables appending
        {
            writer.WriteLine(liness[0]);
            //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
        }
        SaveHouseData();
    }

    public void UIClosing()
    {
        publicBoolForPauseMenuOpen.isAnUIOpened = false;
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        //Debug.Log(houseID);
        SaveHouseData();
        CheckRoadConnection();
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
        CancelButton.SetActive(false);
    }

    public bool CheckTech(int nodeToCheck)
    {
        string filePath = Path.Combine(Application.persistentDataPath, "TechTree.txt");
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (nodeToCheck < lines.Length && lines[nodeToCheck] == "1")
            {
                Debug.Log($"Tech node at line {nodeToCheck} is unlocked. Event triggered.");
                return true;
            }
            else
            {
                Debug.Log($"Tech node at line {nodeToCheck} is locked. Event not triggered.");
                return false;
            }
        }
        else
        {
            Debug.LogError($"Save file not found at: {filePath}");
            return false;
        }
    }

    // This method is triggered when the object is clicked
    public void OnClick()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        if (File.Exists(filePath))
        {
            string[] llines = File.ReadAllLines(filePath);
            if (llines.Length > 3)
            {
                ButtonBlockers.SetActive(true);
                int intake = Int32.Parse(llines[2]);

                for(int i = 0; i < keys.Length; i++)
                {
                    //Debug.Log(i + " and " + intake);
                    if(keys[i] == intake)
                    {
                        buttons[i].onClick.Invoke(); // This code part checks what is beeing produced and "simulates" a button press so that the UI will show the currently produced product
                        break;
                    }
                }
            }
            else
            {
                ButtonBlockers.SetActive(false);
            }
        }

        //Debug.Log("Click happened!");
        // Enable the specified GameObject
        if (objectToEnable != null && CanBeClickedOn)
        {
            if (CashIcon.activeSelf == false)
            {
                objectToEnable.SetActive(true);
                publicBoolForPauseMenuOpen.isAnUIOpened = true;
                Button0.SetActive(false);
                Button1.SetActive(false);
                Button2.SetActive(false);
                Button3.SetActive(false);
                Button4.SetActive(false);
                Button5.SetActive(false);

                switch(Stage)
                {
                    case 0:
                        {
                            foreach (GameObject obj in ProductionStarterButtons)
                            {
                                obj.SetActive(false);
                            }
                            CancelButton.SetActive(false);
                            UnableToWorkText.SetActive(true);
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
                            break;
                        }
                    case 1:
                        {
                            foreach (GameObject obj in ProductionStarterButtons)
                            {
                                obj.SetActive(false);
                            }
                            CancelButton.SetActive(false);
                            UnableToWorkText.SetActive(true);
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
                            break;
                        }
                    case 2:
                        {
                            CancelButton.SetActive(true);
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
                            break;
                        }
                    case 3:
                        {
                            foreach (GameObject obj in ProductionStarterButtons)
                            {
                                obj.SetActive(false);
                            }
                            CancelButton.SetActive(false);
                            UnableToWorkText.SetActive(true);
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive2 + " Power";
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake2 + " Power";
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive2 + " Mood";
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake2 + " Mood";
                            }
                            break;
                        }
                    case 4:
                        {
                            CancelButton.SetActive(true);
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive2 + " Power";
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake2 + " Power";
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive2 + " Mood";
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake2 + " Mood";
                            }
                            break;
                        }
                    case 5:
                        {
                            foreach (GameObject obj in ProductionStarterButtons)
                            {
                                obj.SetActive(false);
                            }
                            CancelButton.SetActive(false);
                            UnableToWorkText.SetActive(true);
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive3 + " Power";
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake3 + " Power";
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive3 + " Mood";
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake3 + " Mood";
                            }
                            break;
                        }
                    case 6:
                        {
                            CancelButton.SetActive(true);
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive3 + " Power";
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake3 + " Power";
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive3 + " Mood";
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake3 + " Mood";
                            }
                            break;
                        }
                }

                //--------------------------------------------------------------------------------------------------
                //The following part will load in the inventory and check if there is enough stuff for constructions
                //and based on that enable/disable the buttons.
                //--------------------------------------------------------------------------------------------------

                string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
                //OG: string data = File.ReadAllText(inventoryFilePath);
                string[] datas = File.ReadAllLines(inventoryFilePath);
                // Parsing the data from the file
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

                if (Stage == 0)
                {
                    if(Wood >= NeddedWood01 && WoodenBeam >= NeddedWoodenBeam01 && WoodenPanel >= NeddedWoodenPanel01 && Brick >= NeddedBrick01 && RoofTile >= NeddedRoofTile01 && Concrete >= NeddedConcrete01 && Tile >= NeddedTile01 && Nail >= NeddedNails01 && Steel >= NeddedSteel01 && MetalSheet >= NeddedMetalSheet01 && SteelRod >= NeddedSteelRod01 && Screw >= NeddedScrew01)
                    {
                        Button0.SetActive(true);
                    }
                    string materialtxt;
                    materialtxt = "The upgrade requies: ";
                    if(NeddedWood01 > 0)
                    {
                        materialtxt = materialtxt + NeddedWood01 + " Lumber. ";
                    }
                    if (NeddedWoodenBeam01 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenBeam01 + " Wooden Beam. ";
                    }
                    if (NeddedWoodenPanel01 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenPanel01 + " Wooden Panel. ";
                    }
                    if (NeddedBrick01 > 0)
                    {
                        materialtxt = materialtxt + NeddedBrick01 + " Brick. ";
                    }
                    if (NeddedRoofTile01 > 0)
                    {
                        materialtxt = materialtxt + NeddedRoofTile01 + " Roof Tile. ";
                    }
                    if (NeddedConcrete01 > 0)
                    {
                        materialtxt = materialtxt + NeddedConcrete01 + " Concrete.";
                    }
                    if (NeddedTile01 > 0)
                    {
                        materialtxt = materialtxt + NeddedTile01 + " Tile. ";
                    }
                    if (NeddedNails01 > 0)
                    {
                        materialtxt = materialtxt + NeddedNails01 + " Nail. ";
                    }
                    if (NeddedSteel01 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteel01 + " Steel Beam. ";
                    }
                    if (NeddedMetalSheet01 > 0)
                    {
                        materialtxt = materialtxt + NeddedMetalSheet01 + " Metal Sheet. ";
                    }
                    if (NeddedSteelRod01 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteelRod01 + " Steel Rod. ";
                    }
                    if (NeddedScrew01 > 0)
                    {
                        materialtxt = materialtxt + NeddedScrew01 + " Screw.";
                    }
                    MaterialsNeededText.text = materialtxt;
                }
                else if(Stage == 2)
                {
                    if (CheckTech(3) && Wood >= NeddedWood12 && WoodenBeam >= NeddedWoodenBeam12 && WoodenPanel >= NeddedWoodenPanel12 && Brick >= NeddedBrick12 && RoofTile >= NeddedRoofTile12 && Concrete >= NeddedConcrete12 && Tile >= NeddedTile12 && Nail >= NeddedNails12 && Steel >= NeddedSteel12 && MetalSheet >= NeddedMetalSheet12 && SteelRod >= NeddedSteelRod12 && Screw >= NeddedScrew12)
                    {
                        Button2.SetActive(true);
                    }
                    string materialtxt;
                    materialtxt = "The upgrade requies: ";
                    if (NeddedWood12 > 0)
                    {
                        materialtxt = materialtxt + NeddedWood12 + " Lumber. ";
                    }
                    if (NeddedWoodenBeam12 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenBeam12 + " Wooden Beam. ";
                    }
                    if (NeddedWoodenPanel12 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenPanel12 + " Wooden Panel. ";
                    }
                    if (NeddedBrick12 > 0)
                    {
                        materialtxt = materialtxt + NeddedBrick12 + " Brick. ";
                    }
                    if (NeddedRoofTile12 > 0)
                    {
                        materialtxt = materialtxt + NeddedRoofTile12 + " Roof Tile. ";
                    }
                    if (NeddedConcrete12 > 0)
                    {
                        materialtxt = materialtxt + NeddedConcrete12 + " Concrete.";
                    }
                    if (NeddedTile12 > 0)
                    {
                        materialtxt = materialtxt + NeddedTile12 + " Tile. ";
                    }
                    if (NeddedNails12 > 0)
                    {
                        materialtxt = materialtxt + NeddedNails12 + " Nail. ";
                    }
                    if (NeddedSteel12 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteel12 + " Steel Beam. ";
                    }
                    if (NeddedMetalSheet12 > 0)
                    {
                        materialtxt = materialtxt + NeddedMetalSheet12 + " Metal Sheet. ";
                    }
                    if (NeddedSteelRod12 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteelRod12 + " Steel Rod. ";
                    }
                    if (NeddedScrew12 > 0)
                    {
                        materialtxt = materialtxt + NeddedScrew12 + " Screw.";
                    }
                    MaterialsNeededText.text = materialtxt;
                }
                else if(Stage == 4)
                {
                    if (CheckTech(8) && Wood >= NeddedWood23 && WoodenBeam >= NeddedWoodenBeam23 && WoodenPanel >= NeddedWoodenPanel23 && Brick >= NeddedBrick23 && RoofTile >= NeddedRoofTile23 && Concrete >= NeddedConcrete23 && Tile >= NeddedTile23 && Nail >= NeddedNails23 && Steel >= NeddedSteel23 && MetalSheet >= NeddedMetalSheet23 && SteelRod >= NeddedSteelRod23 && Screw >= NeddedScrew23)
                    {
                        Button4.SetActive(true);
                    }
                    string materialtxt;
                    materialtxt = "The upgrade requies: ";
                    if (NeddedWood23 > 0)
                    {
                        materialtxt = materialtxt + NeddedWood23 + " Lumber. ";
                    }
                    if (NeddedWoodenBeam23 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenBeam23 + " Wooden Beam. ";
                    }
                    if (NeddedWoodenPanel23 > 0)
                    {
                        materialtxt = materialtxt + NeddedWoodenPanel23 + " Wooden Panel. ";
                    }
                    if (NeddedBrick23 > 0)
                    {
                        materialtxt = materialtxt + NeddedBrick23 + " Brick. ";
                    }
                    if (NeddedRoofTile23 > 0)
                    {
                        materialtxt = materialtxt + NeddedRoofTile23 + " Roof Tile. ";
                    }
                    if (NeddedConcrete23 > 0)
                    {
                        materialtxt = materialtxt + NeddedConcrete23 + " Concrete.";
                    }
                    if (NeddedTile23 > 0)
                    {
                        materialtxt = materialtxt + NeddedTile23 + " Tile. ";
                    }
                    if (NeddedNails23 > 0)
                    {
                        materialtxt = materialtxt + NeddedNails23 + " Nail. ";
                    }
                    if (NeddedSteel23 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteel23 + " Steel Beam. ";
                    }
                    if (NeddedMetalSheet23 > 0)
                    {
                        materialtxt = materialtxt + NeddedMetalSheet23 + " Metal Sheet. ";
                    }
                    if (NeddedSteelRod23 > 0)
                    {
                        materialtxt = materialtxt + NeddedSteelRod23 + " Steel Rod. ";
                    }
                    if (NeddedScrew23 > 0)
                    {
                        materialtxt = materialtxt + NeddedScrew23 + " Screw.";
                    }
                    MaterialsNeededText.text = materialtxt;
                }
                else if(Stage == 6)
                {
                    MaterialsNeededText.text = "Congratulations! This factory has been upgraded to the final level!";
                }
                else if(Stage == 1 || Stage == 3 || Stage == 5)
                {
                    MaterialsNeededText.text = "Construction in progress!";
                }
            }
            else if(CashIcon.activeSelf && !IsDecorationOrPowerPlant)
            {
                foreach (GameObject obj in ProductionStarterButtons)
                {
                    obj.SetActive(true);
                }
                CashIcon.SetActive(false);
                if (clip != null)
                {
                    string SFXFilePath;
                    SFXFilePath = Path.Combine(Application.persistentDataPath, "Audio.txt");
                    string[] datas = File.ReadAllLines(SFXFilePath);
                    float volume = float.Parse(datas[0]);
                    GameObject Cam = GameObject.Find("Main Camera");
                    AudioSource.PlayClipAtPoint(clip, Cam.transform.position, volume);
                }
                string PowerFilePath;
                PowerFilePath = Path.Combine(Application.persistentDataPath, "powerLevels.txt");
                float CurrentPower = ReadOverallFloat(PowerFilePath);

                string MoodFilePath;
                MoodFilePath = Path.Combine(Application.persistentDataPath, "moodLevels.txt");
                float CurrentMood = ReadOverallFloat(MoodFilePath);

                if (cashDisplay != null)
                {
                    //string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
                    if (File.Exists(filePath))
                    {
                        string[] llines = File.ReadAllLines(filePath);
                        if (llines.Length > 3)
                        {
                            string[] lines = File.ReadAllLines(filePath);
                            string ID = lines[2];
                            int IDint = Int32.Parse(ID);
                            string ammount = lines[3];
                            int ammountint = Int32.Parse(ammount);
                            //--Add the materials--
                            string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
                            string[] datas = File.ReadAllLines(inventoryFilePath);
                            string data = datas[IDint];
                            int change = Int32.Parse(data);

                            switch (Stage)
                            {
                                case 2:
                                    {
                                        ammountint = (int)Math.Ceiling(ammountint * CurrentMood * CurrentPower);
                                        change = change + ammountint;
                                        datas[IDint] = change.ToString();

                                        for (int i = 0; i < keys.Length; i++)
                                        {
                                            if (keys[i] == IDint)
                                            {
                                                dynamicCollectiontxt.text = "+ " + ammountint.ToString();
                                                DynamyCollectionObject.SetActive(false);
                                                DynamyCollectionObject.SetActive(true);
                                                Collectiontxt[i].SetActive(false);
                                                Collectiontxt[i].SetActive(true); // This code part checks what is beeing produced and "simulates" a button press so that the UI will show the currently produced product
                                                break;
                                            }
                                        }

                                        using (StreamWriter Wwriter = new StreamWriter(inventoryFilePath))
                                        {
                                            foreach (string element in datas)
                                            {
                                                Wwriter.WriteLine(element); // Write each element on a new line
                                            }
                                        }
                                        break;
                                    }
                                case 4:
                                    {
                                        ammountint = (int)Math.Ceiling(ammountint * CurrentMood * CurrentPower * 2);
                                        change = change + ammountint;
                                        datas[IDint] = change.ToString();

                                        for (int i = 0; i < keys.Length; i++)
                                        {
                                            if (keys[i] == IDint)
                                            {
                                                dynamicCollectiontxt.text = "+ " + ammountint.ToString();
                                                DynamyCollectionObject.SetActive(false);
                                                DynamyCollectionObject.SetActive(true);
                                                Collectiontxt[i].SetActive(false);
                                                Collectiontxt[i].SetActive(true); // This code part checks what is beeing produced and "simulates" a button press so that the UI will show the currently produced product
                                                break;
                                            }
                                        }

                                        using (StreamWriter Wwriter = new StreamWriter(inventoryFilePath))
                                        {
                                            foreach (string element in datas)
                                            {
                                                Wwriter.WriteLine(element); // Write each element on a new line
                                            }
                                        }
                                        break;
                                    }
                                case 6:
                                    {
                                        ammountint = (int)Math.Ceiling(ammountint * CurrentMood * CurrentPower * 4);
                                        change = change + ammountint;
                                        datas[IDint] = change.ToString();

                                        for (int i = 0; i < keys.Length; i++)
                                        {
                                            if (keys[i] == IDint)
                                            {
                                                dynamicCollectiontxt.text = "+ " + ammountint.ToString();
                                                DynamyCollectionObject.SetActive(false);
                                                DynamyCollectionObject.SetActive(true);
                                                Collectiontxt[i].SetActive(false);
                                                Collectiontxt[i].SetActive(true); // This code part checks what is beeing produced and "simulates" a button press so that the UI will show the currently produced product
                                                break;
                                            }
                                        }

                                        using (StreamWriter Wwriter = new StreamWriter(inventoryFilePath))
                                        {
                                            foreach (string element in datas)
                                            {
                                                Wwriter.WriteLine(element); // Write each element on a new line
                                            }
                                        }
                                        break;
                                    }
                            }

                            string[] liness = File.ReadAllLines(filePath);
                            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
                            {
                                writer.WriteLine(liness[0]);
                                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogError("CashDisplay script not found in the scene.");
                }
            }
        }
    }

    public void DestroyBuilding()
    {
        string path = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        if (File.Exists(path))
        {
            // Delete the folder and its contents
            File.Delete(path); // 'true' to delete recursively
        }
        else
        {
            Debug.LogWarning("File does not exist: " + path);
        }

        if (isBlueprintRefundable)
        {
            blueprintChecker.AddBlueprint(HouseArrayID);
        }

        switch (Stage)
        {
            case 0:
                {
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
                    break;
                }
            case 1:
                {
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
                    break;
                }
            case 2:
                {
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
                    break;
                }
            case 3:
                {
                    if (PowerGiver)
                    {
                        Power.DecreasePositive(PowerToGive2);
                    }
                    else if (!PowerGiver)
                    {
                        Power.DecreaseNegative(PowerToTake2);
                    }

                    if (MoodGiver)
                    {
                        Mood.DecreasePositive(MoodToGive2);
                    }
                    else if (!MoodGiver)
                    {
                        Mood.DecreaseNegative(MoodToTake2);
                    }
                    break;
                }
            case 4:
                {
                    if (PowerGiver)
                    {
                        Power.DecreasePositive(PowerToGive2);
                    }
                    else if (!PowerGiver)
                    {
                        Power.DecreaseNegative(PowerToTake2);
                    }

                    if (MoodGiver)
                    {
                        Mood.DecreasePositive(MoodToGive2);
                    }
                    else if (!MoodGiver)
                    {
                        Mood.DecreaseNegative(MoodToTake2);
                    }
                    break;
                }
            case 5:
                {
                    if (PowerGiver)
                    {
                        Power.DecreasePositive(PowerToGive3);
                    }
                    else if (!PowerGiver)
                    {
                        Power.DecreaseNegative(PowerToTake3);
                    }

                    if (MoodGiver)
                    {
                        Mood.DecreasePositive(MoodToGive3);
                    }
                    else if (!MoodGiver)
                    {
                        Mood.DecreaseNegative(MoodToTake3);
                    }
                    break;
                }
            case 6:
                {
                    if (PowerGiver)
                    {
                        Power.DecreasePositive(PowerToGive3);
                    }
                    else if (!PowerGiver)
                    {
                        Power.DecreaseNegative(PowerToTake3);
                    }

                    if (MoodGiver)
                    {
                        Mood.DecreasePositive(MoodToGive3);
                    }
                    else if (!MoodGiver)
                    {
                        Mood.DecreaseNegative(MoodToTake3);
                    }
                    break;
                }
        }

        objectToEnable.SetActive(false);
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
        data.Add($"{houseIndex},{transform.position.x},{transform.position.y},{transform.position.z},{Stage}");
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
        Stage = int.Parse(parts[4]);
        Vector3 position = new Vector3(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]));
        transform.position = position;
        //Debug.Log("Succesful loading log from building: " + houseID + "BuildingIndex: " + houseIndex + "Current stage: " + Stage + "Current loacation: " + position);
    }
}

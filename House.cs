using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.Audio;
using System.Linq;
using System.Collections.Generic;

public class House : MonoBehaviour
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
    [Tooltip("Resident type. Options: 0 - Workers; 1 - Investors; 2 - Scientists. Just leave it as 0. (Player can change it at any time.)")]
    public int ResidentType = 0;
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
    [Tooltip("This is the UI, that is going to be enabled, when the mouse hover over the building. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToHoverEnable;
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

    [Header("Rent Payments & XP")]
    [Tooltip("How much time it should take for the rent to be ready.")]
    public int TimeToGive;
    public int CashToGive1;
    public int XPToGive1;
    public int CashToGive2;
    public int XPToGive2;
    public int CashToGive3;
    public int XPToGive3;

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
    private CashDisplay cashDisplay;
    public int ConstructionPlacementCost;
    public TextMeshProUGUI InfoMoodText;
    public TextMeshProUGUI InfoPowerText;
    public TextMeshProUGUI InfoTimerText;
    public TextMeshProUGUI InfoRentText;
    public TextMeshProUGUI InfoWPText;
    public TextMeshProUGUI InfoSPText;

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
    public BlueprintChecker blueprintChecker;
    public int HouseArrayID;
    public AudioClip clip;
    public TextMeshProUGUI MaterialsNeededText;
    public TMP_Dropdown dropdown;
    public TextMeshProUGUI PEffects;
    public TextMeshProUGUI MEffects;
    public TextMeshProUGUI CashXP;
    public TextMeshProUGUI WSPoints;
    public AudioMixer audioMixer;
    public InventoryBasedDisabler inventoryBasedDisabler;
    public GameObject animatedFloatingTextUponCollection;
    public TMP_Text AFTUCtxt;
    private bool CanBeClickedOn = false;

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
    private string filePath;
    private string inventoryFilePath;

    void Start()
    {
        //mainCamera = Camera.main;
        cashDisplay = FindObjectOfType<CashDisplay>();
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        //Debug.Log($"Vector3: {offset}");
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

        if (!Directory.Exists(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building")))
        {
            Directory.CreateDirectory(Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building"));
        }
        saveFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building", houseID + ".txt");
        inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "inventory.txt");

        // Load house data from a file
        string directoryPath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building");
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Building", houseID + ".txt");

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
            ResidentType = int.Parse(parts[5]);
            dropdown.SetValueWithoutNotify(ResidentType);
            dropdown.RefreshShownValue();
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
                    Stage1.SetActive(true);
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

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            //Debug.Log(lines.Length);
            if (lines.Length > 1)
            {
                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
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
    }

    public void OnMouseHover()
    {
        objectToHoverEnable.SetActive(true);

        switch (Stage)
        {
            case 0:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive01;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake01;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive01;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake01;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 1:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive01;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake01;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive01;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake01;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 2:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive01;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake01;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive01;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake01;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 3:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive2;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake2;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive2;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake2;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 4:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive2;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake2;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive2;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake2;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 5:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive3;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake3;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive3;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake3;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
            case 6:
                {
                    if (PowerGiver)
                    {
                        InfoPowerText.text = "+ " + PowerToGive3;
                        InfoPowerText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!PowerGiver)
                    {
                        InfoPowerText.text = "- " + PowerToTake3;
                        InfoPowerText.color = new Color32(200, 0, 0, 255);
                    }

                    if (MoodGiver)
                    {
                        InfoMoodText.text = "+ " + MoodToGive3;
                        InfoMoodText.color = new Color32(0, 200, 0, 255);
                    }
                    else if (!MoodGiver)
                    {
                        InfoMoodText.text = "- " + MoodToTake3;
                        InfoMoodText.color = new Color32(200, 0, 0, 255);
                    }
                    break;
                }
        }
    }

    public void NoMouseHover()
    {
        objectToHoverEnable.SetActive(false);
    }

    public void ResidentTypeSet()
    {
        ResidentType = dropdown.value;
        switch (ResidentType)
        {
            case 0:
                {
                    WSPoints.text = "Current residents: Workers - Pays Worker Points (0,5x of the PAID rent)";
                    break;
                }
            case 1:
                {
                    WSPoints.text = "Current residents: Investors - Pays 2x the BASE rent. (Does not apply to the XP!)";
                    break;
                }
            case 2:
                {
                    WSPoints.text = "Current residents: Scientists - Pays Science Points (0,5x of the PAID rent)";
                    break;
                }
        }
        CitizenTypeChangerSubtractor();
        SaveHouseData();
    }

    public void CitizenTypeChangerSubtractor()
    {
        //Dear future me. Do not merge this into the end of ResidentTypeSet(). Since Start() changes the resident type via the UI, that makes the dropdown call that function, which results in subracting one CTC every time the game starts.
        //Scratch that. There is a SetValueWithoutNotify function for the dropdown, but I am leaving these comments here for fun.
        string[] datas = File.ReadAllLines(inventoryFilePath);
        int CitizenTypeChange = int.Parse(datas[34]);
        datas[34] = (CitizenTypeChange - 1).ToString();
        using (StreamWriter writer = new StreamWriter(inventoryFilePath))
        {
            foreach (string element in datas)
            {
                writer.WriteLine(element); // Write each element on a new line
            }
        }
        inventoryBasedDisabler.Start();
    }

    void Update()
    {
        if (isConnectedToRoad || IsDecorationOrPowerPlant)
        {
            if (Stage == 2 || Stage == 4 || Stage == 6)
            {
                CheckTimeAndEnableObject();
                UpdateTimerText();
            }
            else if (Stage == 1)
            {
                DateTime currentTime = DateTime.Now;

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
        //Debug.Log("I got checked!");
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

        if(isConnectedToRoad || IsDecorationOrPowerPlant)
        {
            NoRoadIcon.SetActive(false);
            timerText.text = "00:00:00";
        }
        else
        {
            NoRoadIcon.SetActive(true);
            timerText.text = "Road missing!";
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

    void LoadFutureTime()
    {
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
        if(Stage == 2 || Stage == 4 || Stage == 6)
        {
            DateTime currentTime = DateTime.Now;
            futureTime = currentTime.AddSeconds(TimeToGive);
            string futureTimeString = futureTime.ToString("yyyy-MM-dd HH:mm:ss");

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

    // Method to update the TMPro text with the remaining time
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
            InfoTimerText.text = timeString;
        }
        else
        {
            // If the time has elapsed, display 00:00:00
            timerText.text = "00:00:00";
            InfoTimerText.text = "00:00:00";
        }
    }

    public void Upgrade01()
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

        UIClosing();
        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "1";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]},{core[5]}";
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }

            //--Subtract the materials--
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
        SaveHouseData();
        SaveFutureTime();
    }

    public void Upgrade12()
    {
        Stage = 3;
        UIClosing();
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

        if (File.Exists(filePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);
            string[] core = lines[0].Split(',');
            core[4] = "3";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]},{core[5]}";
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }

            //--Subtract the materials--
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
        SaveHouseData();
        SaveFutureTime();
    }

    public void Upgrade23()
    {
        Stage = 5;
        UIClosing();
        objectToEnable.SetActive(false);
        Stage2.SetActive(false);
        Construction23.SetActive(true);
        Button4.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct23);
        futureTime = currentTime.AddSeconds(TimeToConstruct23);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");

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
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]},{core[5]}";
            using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
                //Debug.Log($"Construction time appended: {futureTimeString} to file: {filePath}");
            }

            //--Subtract the materials--
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

    public bool CheckTech(int nodeToCheck)
    { 
        string filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "TechTree.txt");
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
        // Enable the specified GameObject
        if (objectToEnable != null && CanBeClickedOn)
        {
            if (CashIcon.activeSelf == false)
            {
                objectToEnable.SetActive(true);
                publicBoolForPauseMenuOpen.isAnUIOpened = true;
                if(inventoryBasedDisabler != null)
                {
                    inventoryBasedDisabler.Start();
                }
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
                            if(PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive01;
                            }
                            else if(!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake01;
                            }

                            if(MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive01;
                            }
                            else if(!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake01;
                            }
                            CashXP.text = "Currently pays no rent!";
                            WSPoints.text = "Currently gives no Worker or Science Point!";
                            break;
                        }
                    case 1:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive01;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake01;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive01;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake01;
                            }
                            CashXP.text = "Under construction, pays no rent!";
                            WSPoints.text = "Under construction, gives no Worker or Science Point!";
                            break;
                        }
                    case 2:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive01;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake01;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive01;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake01;
                            }
                            CashXP.text = "Base rent: " + CashToGive1 + " $ and " + XPToGive1 + " XP";
                            switch(ResidentType)
                            {
                                case 0:
                                    {
                                        WSPoints.text = "Current residents: Workers - Pays Worker Points (0,5x of the PAID rent)";
                                        break;
                                    }
                                case 1:
                                    {
                                        WSPoints.text = "Current residents: Investors - Pays 2x the BASE rent. (Does not apply to the XP!)";
                                        break;
                                    }
                                case 2:
                                    {
                                        WSPoints.text = "Current residents: Scientists - Pays Science Points (0,5x of the PAID rent)";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 3:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive2;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake2;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive2;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake2;
                            }
                            CashXP.text = "Under construction, pays no rent!";
                            WSPoints.text = "Under construction, gives no Worker or Science Point!";
                            break;
                        }
                    case 4:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive2;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake2;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive2;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake2;
                            }
                            CashXP.text = "Base rent: " + CashToGive2 + " $ and " + XPToGive2 + " XP";
                            switch (ResidentType)
                            {
                                case 0:
                                    {
                                        WSPoints.text = "Current residents: Workers - Pays Worker Points (0,5x of the PAID rent)";
                                        break;
                                    }
                                case 1:
                                    {
                                        WSPoints.text = "Current residents: Investors - Pays 2x the BASE rent. (Does not apply to the XP!)";
                                        break;
                                    }
                                case 2:
                                    {
                                        WSPoints.text = "Current residents: Scientists - Pays Science Points (0,5x of the PAID rent)";
                                        break;
                                    }
                            }
                            break;
                        }
                    case 5:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive3;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake3;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive3;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake3;
                            }
                            CashXP.text = "Under construction, pays no rent!";
                            WSPoints.text = "Under construction, gives no Worker or Science Point!";
                            break;
                        }
                    case 6:
                        {
                            if (PowerGiver)
                            {
                                PEffects.text = "+ " + PowerToGive3;
                            }
                            else if (!PowerGiver)
                            {
                                PEffects.text = "- " + PowerToTake3;
                            }

                            if (MoodGiver)
                            {
                                MEffects.text = "+ " + MoodToGive3;
                            }
                            else if (!MoodGiver)
                            {
                                MEffects.text = "- " + MoodToTake3;
                            }
                            CashXP.text = "Base rent: " + CashToGive3 + " $ and " + XPToGive3 + " XP";
                            switch (ResidentType)
                            {
                                case 0:
                                    {
                                        WSPoints.text = "Current residents: Workers - Pays Worker Points (0,5x of the PAID rent)";
                                        break;
                                    }
                                case 1:
                                    {
                                        WSPoints.text = "Current residents: Investors - Pays 2x the BASE rent. (Does not apply to the XP!)";
                                        break;
                                    }
                                case 2:
                                    {
                                        WSPoints.text = "Current residents: Scientists - Pays Science Points (0,5x of the PAID rent)";
                                        break;
                                    }
                            }
                            break;
                        }
                }

                //--------------------------------------------------------------------------------------------------
                //The following part will load in the inventory and check if there is enough stuff for constructions
                //and based on that enable/disable the buttons.
                //--------------------------------------------------------------------------------------------------

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
                    MaterialsNeededText.text = "Congratulations! This house has been upgraded to the final level!";
                }
                else if(Stage == 1 || Stage == 3 || Stage == 5)
                {
                    MaterialsNeededText.text = "Construction in progress!";
                }
            }
            else if(CashIcon.activeSelf && !IsDecorationOrPowerPlant)
            {
                CashIcon.SetActive(false);
                SaveFutureTime();
                if (clip != null)
                {
                    string SFXFilePath;
                    SFXFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Audio.txt");
                    string[] datas = File.ReadAllLines(SFXFilePath);
                    float volume = float.Parse(datas[0]);
                    GameObject Cam = GameObject.Find("Main Camera");
                    AudioSource.PlayClipAtPoint(clip, Cam.transform.position, volume);
                }
                string PowerFilePath;
                PowerFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "powerLevels.txt");
                float CurrentPower = ReadOverallFloat(PowerFilePath);

                string MoodFilePath;
                MoodFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "moodLevels.txt");
                float CurrentMood = ReadOverallFloat(MoodFilePath);

                if (cashDisplay != null)
                {
                    switch (Stage)
                    {
                        case 2:
                            {
                                switch(ResidentType)
                                {
                                    case 0:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1));
                                            wpandsp.ChangeWPoints(Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "Worker Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 1:
                                        {
                                            cashDisplay.AddCash((Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1))*2);
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1))*2 + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1)) + "XP";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 2:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1));
                                            wpandsp.ChangeSPoints(Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive1 * CurrentMood * CurrentPower), XPToGive1)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive1 * CurrentMood * CurrentPower), CashToGive1)) + "Science Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                }
                                break;
                            }
                        case 4:
                            {
                                switch (ResidentType)
                                {
                                    case 0:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2));
                                            wpandsp.ChangeWPoints(Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) + "Worker Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 1:
                                        {
                                            cashDisplay.AddCash((Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) * 2);
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2)) + "XP";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 2:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2));
                                            wpandsp.ChangeSPoints(Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive2 * CurrentMood * CurrentPower), XPToGive2)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive2 * CurrentMood * CurrentPower), CashToGive2)) + "Science Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                }
                                break;
                            }
                        case 6:
                            {
                                switch (ResidentType)
                                {
                                    case 0:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3));
                                            wpandsp.ChangeWPoints(Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) + "Worker Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 1:
                                        {
                                            cashDisplay.AddCash((Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) * 2);
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3)) + "XP \n";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                    case 2:
                                        {
                                            cashDisplay.AddCash(Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3));
                                            xpManager.AddXP(Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3));
                                            wpandsp.ChangeSPoints(Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3));
                                            AFTUCtxt.text = "+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) + "$ \n+ " + (Mathf.Min(Mathf.CeilToInt(XPToGive3 * CurrentMood * CurrentPower), XPToGive3)) + "XP \n+ " + (Mathf.Min(Mathf.CeilToInt(CashToGive3 * CurrentMood * CurrentPower), CashToGive3)) + "Science Point";
                                            animatedFloatingTextUponCollection.SetActive(false);
                                            animatedFloatingTextUponCollection.SetActive(true);
                                            break;
                                        }
                                }
                                break;
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

        if(isBlueprintRefundable)
        {
            blueprintChecker.AddBlueprint(HouseArrayID);
        }

        switch(Stage)
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
        data.Add($"{houseIndex},{transform.position.x},{transform.position.y},{transform.position.z},{Stage},{ResidentType}");
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
            Debug.Log("I am here at Line:1723");
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
        ResidentType = int.Parse(parts[5]);
        //Debug.Log("Succesful loading log from building: " + houseID + "BuildingIndex: " + houseIndex + "Current stage: " + Stage + "Current loacation: " + position);
    }
}

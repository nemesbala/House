using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class CityHall : MonoBehaviour
{
    [Header("Construction Checker")]
    [Tooltip("You can move the area where the place will be checked for available space.")]
    public Vector3 offset;
    [Tooltip("You can resize the area where the place will be checked for available space.")]
    public Vector3 size;
    [Tooltip("The index of the house in the BuildingSystem array.")]
    public int houseIndex;
    [Tooltip("Current stage of the house. Leave it as 0, unless you know what you are doing!")]
    public int Stage;
    [Tooltip("Unique ID for this house. Auto generated upon construction.")]
    public string houseID;
    [Tooltip("Assign the Text Field that will display the House ID.")]
    public TextMeshProUGUI IDText;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    private string saveFilePath;
    private string inventoryFilePath;

    [Header("Building Stages")]
    [Tooltip("This is the UI, that is going to be enabled. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToEnable;
    [Tooltip("This is the UI, that is going to be enabled, when the mouse hover over the building. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToHoverEnable;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 0")]
    public GameObject EmptyPlot;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 1")]
    public GameObject Construction01;
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
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 7")]
    public GameObject Construction34;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 8")]
    public GameObject Stage4;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 9")]
    public GameObject Construction45;
    [Tooltip("Assign here the correct stages of construction. Saved as: stage = 10")]
    public GameObject Stage5;

    [Header("Upgrade Buttons")]
    [Tooltip("Assign here the correct button, that enables construction from empty to Stage 1.")]
    public GameObject Button0;
    [Tooltip("Assign here the correct button, that finishes construction from empty to Stage 1.")]
    public GameObject Button1;
    [Tooltip("Assign here the correct button, that enables construction from Stage 1 to Stage 2.")]
    public GameObject Button2;
    [Tooltip("Assign here the correct button, that finishes construction from Stage 1 to Stage 2.")]
    public GameObject Button3;
    [Tooltip("Assign here the correct button, that enables construction from Stage 2 to Stage 3.")]
    public GameObject Button4;
    [Tooltip("Assign here the correct button, that finishes construction from Stage 2 to Stage 3.")]
    public GameObject Button5;
    [Tooltip("Assign here the correct button, that enables construction from Stage 3 to Stage 4.")]
    public GameObject Button6;
    [Tooltip("Assign here the correct button, that finishes construction from Stage 3 to Stage 4.")]
    public GameObject Button7;
    [Tooltip("Assign here the correct button, that enables construction from Stage 4 to Stage 5.")]
    public GameObject Button8;
    [Tooltip("Assign here the correct button, that finishes construction from Stage 4 to Stage 5.")]
    public GameObject Button9;

    [Header("Materials Needed")]
    public int NeddedWood01;
    public int NeddedWood12;
    public int NeddedWood23;
    public int NeddedWood34;
    public int NeddedWood45;
    public int NeddedWoodenBeam01;
    public int NeddedWoodenBeam12;
    public int NeddedWoodenBeam23;
    public int NeddedWoodenBeam34;
    public int NeddedWoodenBeam45;
    public int NeddedWoodenPanel01;
    public int NeddedWoodenPanel12;
    public int NeddedWoodenPanel23;
    public int NeddedWoodenPanel34;
    public int NeddedWoodenPanel45;
    public int NeddedNails01;
    public int NeddedNails12;
    public int NeddedNails23;
    public int NeddedNails34;
    public int NeddedNails45;
    public int NeddedBrick01;
    public int NeddedBrick12;
    public int NeddedBrick23;
    public int NeddedBrick34;
    public int NeddedBrick45;
    public int NeddedRoofTile01;
    public int NeddedRoofTile12;
    public int NeddedRoofTile23;
    public int NeddedRoofTile34;
    public int NeddedRoofTile45;
    public int NeddedConcrete01;
    public int NeddedConcrete12;
    public int NeddedConcrete23;
    public int NeddedConcrete34;
    public int NeddedConcrete45;
    public int NeddedTile01;
    public int NeddedTile12;
    public int NeddedTile23;
    public int NeddedTile34;
    public int NeddedTile45;
    public int NeddedSteel01;
    public int NeddedSteel12;
    public int NeddedSteel23;
    public int NeddedSteel34;
    public int NeddedSteel45;
    public int NeddedMetalSheet01;
    public int NeddedMetalSheet12;
    public int NeddedMetalSheet23;
    public int NeddedMetalSheet34;
    public int NeddedMetalSheet45;
    public int NeddedSteelRod01;
    public int NeddedSteelRod12;
    public int NeddedSteelRod23;
    public int NeddedSteelRod34;
    public int NeddedSteelRod45;
    public int NeddedScrew01;
    public int NeddedScrew12;
    public int NeddedScrew23;
    public int NeddedScrew34;
    public int NeddedScrew45;

    [Header("Construction Times")]
    public int TimeToConstruct01;
    public int TimeToConstruct12;
    public int TimeToConstruct23;
    public int TimeToConstruct34;
    public int TimeToConstruct45;

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

    [Header("Other Stuff")]
    public DateTime futureTime;
    [Tooltip("This is the 3D icon above the building that shows up, when the construction is finished")]
    public GameObject ConstructionIcon;
    public TextMeshProUGUI timerText;
    public CashDisplay cashDisplay;
    public int ConstructionPlacementCost;
    public BlueprintChecker blueprintChecker;
    public int HouseArrayID;
    public AudioClip clip;
    public TextMeshProUGUI MaterialsNeededText;
    public AudioMixer audioMixer;
    private bool CanBeClickedOn = false;

    void Start()
    {
        cashDisplay = FindObjectOfType<CashDisplay>();
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        if (string.IsNullOrEmpty(houseID))
        {
            Stage = 0;
            houseID = "CityHall";
        }
        else
        {
            Debug.LogError("Error!");
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

        if (File.Exists(saveFilePath))
        {
            CanBeClickedOn = true;
            string[] datas = File.ReadAllLines(saveFilePath);
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
                    Construction01.SetActive(true);
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
            case 7:
                {
                    Stage = 7;
                    EmptyPlot.SetActive(false);
                    Construction34.SetActive(true);
                    break;
                }
            case 8:
                {
                    Stage = 8;
                    EmptyPlot.SetActive(false);
                    Stage4.SetActive(true);
                    break;
                }
            case 9:
                {
                    Stage = 9;
                    EmptyPlot.SetActive(false);
                    Construction45.SetActive(true);
                    break;
                }
            case 10:
                {
                    Stage = 10;
                    EmptyPlot.SetActive(false);
                    Stage5.SetActive(true);
                    break;
                }
        }
        //The following section replaces the call to LoadFutureTime()
        if (File.Exists(saveFilePath))
        {
            string[] lines = File.ReadAllLines(saveFilePath);
            if (lines.Length > 1)
            {
                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
            }
        }

        GameObject GblueprintChecker = GameObject.Find("Canvas (Has Sound Attached!)");
        blueprintChecker = GblueprintChecker.GetComponent<BlueprintChecker>();
        CheckRoadConnection();
    }

    public void OnMouseHover()
    {
        objectToHoverEnable.SetActive(true);
    }

    public void NoMouseHover()
    {
        objectToHoverEnable.SetActive(false);
    }

    void Update()
    {
        if (isConnectedToRoad)
        {
            if (Stage == 0 || Stage == 2 || Stage == 4 || Stage == 6 || Stage == 8 || Stage == 10)
            {
                UpdateTimerText();
            }
            else
            {
                DateTime currentTime = DateTime.Now;
                string[] lines = File.ReadAllLines(saveFilePath);
                string lastSavedTimeString = lines[1];
                DateTime futureTimeC = DateTime.Parse(lastSavedTimeString);
                UpdateTimerText();
                if (currentTime >= futureTimeC)
                {
                    ConstructionIcon.SetActive(true);

                    switch (Stage)
                    {
                        case 1:
                            {
                                Button1.SetActive(true);
                                break;
                            }
                        case 3:
                            {
                                Button3.SetActive(true);
                                break;
                            }
                        case 5:
                            {
                                Button5.SetActive(true);
                                break;
                            }
                        case 7:
                            {
                                Button7.SetActive(true);
                                break;
                            }
                        case 9:
                            {
                                Button9.SetActive(true);
                                break;
                            }
                    }
                }
            }
        }
        else
        {
            timerText.text = "Road missing!";
        }
    }

    public void CheckRoadConnection()
    {
        isConnectedToRoad = false;

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
        }
        else
        {
            NoRoadIcon.SetActive(true);
        }
    }

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
        if (File.Exists(saveFilePath))
        {
            string[] lines = File.ReadAllLines(saveFilePath); 
            if (lines.Length > 1)
            {
                string lastSavedTimeString = lines[1];
                futureTime = DateTime.Parse(lastSavedTimeString);
            }
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
        }
        else
        {
            // If the time has elapsed, display 00:00:00
            timerText.text = "00:00:00";
        }
    }

    public void Upgrade01()
    {
        Stage = 1;
        UIClosing();
        EmptyPlot.SetActive(false);
        Construction01.SetActive(true);
        Button0.SetActive(false);
        objectToEnable.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct01);
        futureTime = currentTime.AddSeconds(TimeToConstruct01);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");

        if (File.Exists(saveFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(saveFilePath);
            string[] core = lines[0].Split(',');
            core[4] = "1";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(saveFilePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
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
    }

    public void UpgradeFinish01()
    {
        Stage = 2;
        UIClosing();
        objectToEnable.SetActive(false);
        EmptyPlot.SetActive(false);
        Construction01.SetActive(false);
        Stage1.SetActive(true);
        ConstructionIcon.SetActive(false);
        SaveHouseData();
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

        if (File.Exists(saveFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(saveFilePath);
            string[] core = lines[0].Split(',');
            core[4] = "3";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(saveFilePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
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

        if (File.Exists(saveFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(saveFilePath);
            string[] core = lines[0].Split(',');
            core[4] = "5";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(saveFilePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
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
    }

    public void Upgrade34()
    {
        Stage = 7;
        UIClosing();
        objectToEnable.SetActive(false);
        Stage3.SetActive(false);
        Construction34.SetActive(true);
        Button6.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct34);
        futureTime = currentTime.AddSeconds(TimeToConstruct34);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");

        if (File.Exists(saveFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(saveFilePath);
            string[] core = lines[0].Split(',');
            core[4] = "7";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(saveFilePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
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

            Wood = Wood - NeddedWood34;
            WoodenBeam = WoodenBeam - NeddedWoodenBeam34;
            WoodenPanel = WoodenPanel - NeddedWoodenPanel34;
            Brick = Brick - NeddedBrick34;
            RoofTile = RoofTile - NeddedRoofTile34;
            Concrete = Concrete - NeddedConcrete34;
            Tile = Tile - NeddedTile34;
            Nail = Nail - NeddedNails34;
            Steel = Steel - NeddedSteel34;
            MetalSheet = MetalSheet - NeddedMetalSheet34;
            SteelRod = SteelRod - NeddedSteelRod34;
            Screw = Screw - NeddedScrew34;

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
    }

    public void UpgradeFinish34()
    {
        Stage = 8;
        UIClosing();
        objectToEnable.SetActive(false);
        Construction34.SetActive(false);
        Stage4.SetActive(true);
        ConstructionIcon.SetActive(false);
        SaveHouseData();
    }

    public void Upgrade45()
    {
        Stage = 9;
        UIClosing();
        objectToEnable.SetActive(false);
        Stage4.SetActive(false);
        Construction45.SetActive(true);
        Button8.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToConstruct45);
        futureTime = currentTime.AddSeconds(TimeToConstruct45);
        string futureTimeString = futureTimeC.ToString("yyyy-MM-dd HH:mm:ss");

        if (File.Exists(saveFilePath))
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(saveFilePath);
            string[] core = lines[0].Split(',');
            core[4] = "9";
            lines[0] = $"{core[0]},{core[1]},{core[2]},{core[3]},{core[4]}";
            using (StreamWriter writer = new StreamWriter(saveFilePath)) // 'true' enables appending
            {
                writer.WriteLine(lines[0]);
                writer.WriteLine(futureTimeString);
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

            Wood = Wood - NeddedWood45;
            WoodenBeam = WoodenBeam - NeddedWoodenBeam45;
            WoodenPanel = WoodenPanel - NeddedWoodenPanel45;
            Brick = Brick - NeddedBrick45;
            RoofTile = RoofTile - NeddedRoofTile45;
            Concrete = Concrete - NeddedConcrete45;
            Tile = Tile - NeddedTile45;
            Nail = Nail - NeddedNails45;
            Steel = Steel - NeddedSteel45;
            MetalSheet = MetalSheet - NeddedMetalSheet45;
            SteelRod = SteelRod - NeddedSteelRod45;
            Screw = Screw - NeddedScrew45;

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
    }

    public void UpgradeFinish45()
    {
        Stage = 10;
        UIClosing();
        objectToEnable.SetActive(false);
        Construction45.SetActive(false);
        Stage5.SetActive(true);
        ConstructionIcon.SetActive(false);
        SaveHouseData();
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        SaveHouseData();
        CheckRoadConnection();
        cashDisplay.SubtractCash(ConstructionPlacementCost);
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
            objectToEnable.SetActive(true);
            publicBoolForPauseMenuOpen.isAnUIOpened = true;
            Button0.SetActive(false);
            Button1.SetActive(false);
            Button2.SetActive(false);
            Button3.SetActive(false);
            Button4.SetActive(false);
            Button5.SetActive(false);
            Button6.SetActive(false);
            Button7.SetActive(false);
            Button8.SetActive(false);
            Button9.SetActive(false);

                //--------------------------------------------------------------------------------------------------
                //The following part will load in the inventory and check if there is enough stuff for constructions
                //and based on that enable/disable the buttons.
                //--------------------------------------------------------------------------------------------------

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
                    if (Wood >= NeddedWood01 && WoodenBeam >= NeddedWoodenBeam01 && WoodenPanel >= NeddedWoodenPanel01 && Brick >= NeddedBrick01 && RoofTile >= NeddedRoofTile01 && Concrete >= NeddedConcrete01 && Tile >= NeddedTile01 && Nail >= NeddedNails01 && Steel >= NeddedSteel01 && MetalSheet >= NeddedMetalSheet01 && SteelRod >= NeddedSteelRod01 && Screw >= NeddedScrew01)
                    {
                        Button0.SetActive(true);
                    }
                    string materialtxt;
                    materialtxt = "The upgrade requies: ";
                    if (NeddedWood01 > 0)
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
                else if (Stage == 2)
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
                else if (Stage == 4)
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
            else if (Stage == 6)
            {
                if (Wood >= NeddedWood34 && WoodenBeam >= NeddedWoodenBeam34 && WoodenPanel >= NeddedWoodenPanel34 && Brick >= NeddedBrick34 && RoofTile >= NeddedRoofTile34 && Concrete >= NeddedConcrete34 && Tile >= NeddedTile34 && Nail >= NeddedNails34 && Steel >= NeddedSteel34 && MetalSheet >= NeddedMetalSheet34 && SteelRod >= NeddedSteelRod34 && Screw >= NeddedScrew34)
                {
                    Button6.SetActive(true);
                }
                string materialtxt;
                materialtxt = "The upgrade requies: ";
                if (NeddedWood34 > 0)
                {
                    materialtxt = materialtxt + NeddedWood34 + " Lumber. ";
                }
                if (NeddedWoodenBeam34 > 0)
                {
                    materialtxt = materialtxt + NeddedWoodenBeam34 + " Wooden Beam. ";
                }
                if (NeddedWoodenPanel34 > 0)
                {
                    materialtxt = materialtxt + NeddedWoodenPanel34 + " Wooden Panel. ";
                }
                if (NeddedBrick34 > 0)
                {
                    materialtxt = materialtxt + NeddedBrick34 + " Brick. ";
                }
                if (NeddedRoofTile34 > 0)
                {
                    materialtxt = materialtxt + NeddedRoofTile34 + " Roof Tile. ";
                }
                if (NeddedConcrete34 > 0)
                {
                    materialtxt = materialtxt + NeddedConcrete34 + " Concrete.";
                }
                if (NeddedTile34 > 0)
                {
                    materialtxt = materialtxt + NeddedTile34 + " Tile. ";
                }
                if (NeddedNails34 > 0)
                {
                    materialtxt = materialtxt + NeddedNails34 + " Nail. ";
                }
                if (NeddedSteel34 > 0)
                {
                    materialtxt = materialtxt + NeddedSteel34 + " Steel Beam. ";
                }
                if (NeddedMetalSheet34 > 0)
                {
                    materialtxt = materialtxt + NeddedMetalSheet34 + " Metal Sheet. ";
                }
                if (NeddedSteelRod34 > 0)
                {
                    materialtxt = materialtxt + NeddedSteelRod34 + " Steel Rod. ";
                }
                if (NeddedScrew34 > 0)
                {
                    materialtxt = materialtxt + NeddedScrew34 + " Screw.";
                }
                MaterialsNeededText.text = materialtxt;
            }
            else if (Stage == 8)
            {
                if (Wood >= NeddedWood45 && WoodenBeam >= NeddedWoodenBeam45 && WoodenPanel >= NeddedWoodenPanel45 && Brick >= NeddedBrick45 && RoofTile >= NeddedRoofTile45 && Concrete >= NeddedConcrete45 && Tile >= NeddedTile45 && Nail >= NeddedNails45 && Steel >= NeddedSteel45 && MetalSheet >= NeddedMetalSheet45 && SteelRod >= NeddedSteelRod45 && Screw >= NeddedScrew45)
                {
                    Button8.SetActive(true);
                }
                string materialtxt;
                materialtxt = "The upgrade requies: ";
                if (NeddedWood45 > 0)
                {
                    materialtxt = materialtxt + NeddedWood45 + " Lumber. ";
                }
                if (NeddedWoodenBeam45 > 0)
                {
                    materialtxt = materialtxt + NeddedWoodenBeam45 + " Wooden Beam. ";
                }
                if (NeddedWoodenPanel45 > 0)
                {
                    materialtxt = materialtxt + NeddedWoodenPanel45 + " Wooden Panel. ";
                }
                if (NeddedBrick45 > 0)
                {
                    materialtxt = materialtxt + NeddedBrick45 + " Brick. ";
                }
                if (NeddedRoofTile45 > 0)
                {
                    materialtxt = materialtxt + NeddedRoofTile45 + " Roof Tile. ";
                }
                if (NeddedConcrete45 > 0)
                {
                    materialtxt = materialtxt + NeddedConcrete45 + " Concrete.";
                }
                if (NeddedTile45 > 0)
                {
                    materialtxt = materialtxt + NeddedTile45 + " Tile. ";
                }
                if (NeddedNails45 > 0)
                {
                    materialtxt = materialtxt + NeddedNails45 + " Nail. ";
                }
                if (NeddedSteel45 > 0)
                {
                    materialtxt = materialtxt + NeddedSteel45 + " Steel Beam. ";
                }
                if (NeddedMetalSheet45 > 0)
                {
                    materialtxt = materialtxt + NeddedMetalSheet45 + " Metal Sheet. ";
                }
                if (NeddedSteelRod45 > 0)
                {
                    materialtxt = materialtxt + NeddedSteelRod45 + " Steel Rod. ";
                }
                if (NeddedScrew45 > 0)
                {
                    materialtxt = materialtxt + NeddedScrew45 + " Screw.";
                }
                MaterialsNeededText.text = materialtxt;
            }
            else if (Stage == 1 || Stage == 3 || Stage == 5 || Stage == 7 || Stage == 9)
            {
                MaterialsNeededText.text = "Construction in progress!";
            }
            else if (Stage == 10)
            {
                MaterialsNeededText.text = "Congratulations! The City Hall has been upgraded to the final level!";
            }
        }
    }

    public void DestroyBuilding()
    {
        if (File.Exists(saveFilePath))
        {
            // Delete the folder and its contents
            File.Delete(saveFilePath); // 'true' to delete recursively
        }
        else
        {
            Debug.LogWarning("File does not exist: " + saveFilePath);
        }
        blueprintChecker.AddBlueprint(HouseArrayID);
        UIClosing();
        objectToEnable.SetActive(false);
        Destroy(gameObject);
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

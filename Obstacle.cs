using TMPro;
using System;
using System.IO;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;
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
    public GameObject CashIcon;
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
    public GameObject Construction;
    bool AboutToBeDeleted = false;

    [Header("Upgrade Buttons")]
    [Tooltip("Assign here the correct button, that enables construction from empty to Stage 1.")]
    public GameObject Button0;
    [Tooltip("Assign here the correct button, that finishes construction from empty to Stage 1.")]
    public GameObject Button1;

    [Header("Materials to give")]
    public int WPtoClear;
    public int NeddedWood01;
    public int NeddedWoodenBeam01;
    public int NeddedBrick01;
    public int NeddedConcrete01;
    public int XPToGive1;

    [Header("Other Stuff")]
    public DateTime futureTime;
    [Tooltip("This is the 3D icon above the building that shows up, when the construction is finished")]
    public TextMeshProUGUI timerText;
    public int HouseArrayID;
    public AudioClip clip;
    public AudioMixer audioMixer;
    private bool CanBeClickedOn = false;
    public TMP_Text AFTUCtxt;
    public GameObject animatedFloatingTextUponCollection;

    void Start()
    {
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
            if(Stage == 1)
            {
                EmptyPlot.SetActive(false);
                Construction.SetActive(true);
            }

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
            OnPlaced();
        }
    }

    void Update()
    {
        if (Stage == 1 && !AboutToBeDeleted)
        {
            DateTime currentTime = DateTime.Now;

            string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
            string[] lines = File.ReadAllLines(filePath);
            string lastSavedTimeString = lines[1];
            DateTime futureTimeC = DateTime.Parse(lastSavedTimeString);
            UpdateTimerText();
            if (currentTime >= futureTimeC)
            {
                CashIcon.SetActive(true);
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
            if (CashIcon != null && !CashIcon.activeSelf)
            {
                CashIcon.SetActive(true);
                //Debug.Log($"{CashIcon.name} has been enabled at {currentTime}.");
            }
        }
    }

    void SaveFutureTime()
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

    public void Upgrade01()
    {
        Stage = 1;
        EmptyPlot.SetActive(false);
        Construction.SetActive(true);
        Button0.SetActive(false);
        objectToEnable.SetActive(false);
        DateTime currentTime = DateTime.Now;
        DateTime futureTimeC = currentTime.AddSeconds(TimeToGive);
        futureTime = currentTime.AddSeconds(TimeToGive);
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

        }
        else
        {
            Debug.LogError($"File not found at: {filePath}");
        }
    }

    public void UpgradeFinish01()
    {
        AboutToBeDeleted = true;
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

        xpManager.AddXP(XPToGive1);
        AFTUCtxt.text = "+ " + (XPToGive1) + " XP";
        animatedFloatingTextUponCollection.SetActive(false);
        animatedFloatingTextUponCollection.SetActive(true);
        UIClosing();
        objectToEnable.SetActive(false);
        EmptyPlot.SetActive(false);
        Construction.SetActive(false);
        CashIcon.SetActive(false);

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

        StartCoroutine(CallAfterDelay(5f));
    }

    private IEnumerator CallAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        DestroyBuilding();
    }

    // This function will be called when the house is placed
    public void OnPlaced()
    {
        Stage = 0;
        SaveHouseData();
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

                if(WPtoClear <= wpandsp.Wpoints && Stage == 0)
                {
                    Button0.SetActive(true);
                }
            }
            else if (CashIcon.activeSelf)
            {
                UpgradeFinish01();
            }
        }
    }

    public void DestroyBuilding()
    {
        objectToEnable.SetActive(false);
        UIClosing();
        Destroy(gameObject);
    }

    // Save the house's data to a file
    public void SaveHouseData()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
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
            Debug.Log("I am here at Line:366 in an Obstacle.cs");
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

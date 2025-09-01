using System.IO;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private TMP_Dropdown inputDropdown;
    private string settingsFilePath;

    private void Start()
    {
        settingsFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "settings.txt");
        InitializeDropdowns();
        LoadSettings();
    }

    // Initialize dropdown options
    private void InitializeDropdowns()
    {
        displayModeDropdown.options.Clear();
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Windowed"));
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Fullscreen"));
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Borderless Fullscreen"));
        displayModeDropdown.options.Add(new TMP_Dropdown.OptionData("Maximized Window"));

        inputDropdown.options.Clear();
        inputDropdown.options.Add(new TMP_Dropdown.OptionData("Default"));
        inputDropdown.options.Add(new TMP_Dropdown.OptionData("Keyboard Only"));

        qualityDropdown.options.Clear();
        string[] qualityLevels = QualitySettings.names;
        foreach (string level in qualityLevels)
        {
            qualityDropdown.options.Add(new TMP_Dropdown.OptionData(level));
        }

        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        qualityDropdown.onValueChanged.AddListener(SetGraphicsQuality);
        inputDropdown.onValueChanged.AddListener(delegate { SaveSettings(); });
    }

    // Load saved settings at the start
    private void LoadSettings()
    {
        if (File.Exists(settingsFilePath))
        {
            string[] lines = File.ReadAllLines(settingsFilePath);
            if (lines.Length >= 2)
            {
                int displayMode = int.Parse(lines[0]);
                int qualityLevel = int.Parse(lines[1]);
                int KeayboardOnly = int.Parse(lines[2]);

                // Apply display mode setting
                SetDisplayMode(displayMode);
                displayModeDropdown.value = displayMode;

                // Apply quality setting
                QualitySettings.SetQualityLevel(qualityLevel);
                qualityDropdown.value = qualityLevel;
                SetGraphicsQuality(qualityLevel);

                inputDropdown.value = KeayboardOnly;
            }
        }
        else
        {
            int displayMode = 0;
            int qualityLevel = 0;
            int KeayboardOnly = 0;
            // Apply display mode setting
            SetDisplayMode(displayMode);
            displayModeDropdown.value = displayMode;

            // Apply quality setting
            QualitySettings.SetQualityLevel(qualityLevel);
            qualityDropdown.value = qualityLevel;
            SetGraphicsQuality(qualityLevel);
            SaveSettings();
        }
    }

    // Save settings to a file
    private void SaveSettings()
    {
        int displayMode = displayModeDropdown.value;
        int qualityLevel = qualityDropdown.value;
        int keyboardOnly = inputDropdown.value;

        using (StreamWriter writer = new StreamWriter(settingsFilePath)) // 'true' enables appending
        {
            writer.WriteLine(displayMode.ToString());
            writer.WriteLine(qualityLevel.ToString());
            writer.WriteLine(keyboardOnly.ToString());
        }

    }

    // Set display mode based on dropdown selection
    public void SetDisplayMode(int option)
    {
        switch (option)
        {
            case 0:
                Screen.fullScreenMode = FullScreenMode.Windowed;
                break;
            case 1:
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 2:
                Screen.fullScreenMode = FullScreenMode.FullScreenWindow; // Borderless fullscreen
                break;
            case 3:
                Screen.fullScreenMode = FullScreenMode.MaximizedWindow;
                break;
        }
        SaveSettings();
    }

    // Set graphics quality based on dropdown selection
    public void SetGraphicsQuality(int level)
    {
        QualitySettings.SetQualityLevel(level);
        SaveSettings();
    }
}
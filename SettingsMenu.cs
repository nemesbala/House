using System.IO;
using UnityEngine;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown displayModeDropdown;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private string settingsFilePath;

    private void Start()
    {
        settingsFilePath = Path.Combine(Application.persistentDataPath, "settings.txt");
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

        qualityDropdown.options.Clear();
        string[] qualityLevels = QualitySettings.names;
        foreach (string level in qualityLevels)
        {
            qualityDropdown.options.Add(new TMP_Dropdown.OptionData(level));
        }

        displayModeDropdown.onValueChanged.AddListener(SetDisplayMode);
        qualityDropdown.onValueChanged.AddListener(SetGraphicsQuality);
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

                // Apply display mode setting
                SetDisplayMode(displayMode);
                displayModeDropdown.value = displayMode;

                // Apply quality setting
                QualitySettings.SetQualityLevel(qualityLevel);
                qualityDropdown.value = qualityLevel;
                SetGraphicsQuality(qualityLevel);
            }
        }
        else
        {
            int displayMode = 0;
            int qualityLevel = 0;
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

        using (StreamWriter writer = new StreamWriter(settingsFilePath)) // 'true' enables appending
        {

            writer.WriteLine(displayMode.ToString());
            writer.WriteLine(qualityLevel.ToString());
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
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using System.Collections;

public class LocalizationManager : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;
    private string filePath;
    private readonly string defaultCityName = "Honeywood";

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "CityData.txt");
        StartCoroutine(InitializeDropdown());
    }

    IEnumerator InitializeDropdown()
    {
        yield return LocalizationSettings.InitializationOperation;

        EnsureCityDataFile();

        languageDropdown.ClearOptions();

        var locales = LocalizationSettings.AvailableLocales.Locales;
        foreach (var locale in locales)
        {
            languageDropdown.options.Add(new TMP_Dropdown.OptionData(locale.LocaleName));
        }

        int savedIndex = LoadLanguageIndex();
        languageDropdown.value = savedIndex;
        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[savedIndex];
    }

    void OnLanguageChanged(int index)
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
        SaveLanguageIndex(index);
    }

    void EnsureCityDataFile()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllLines(filePath, new[] { defaultCityName, "0" });
        }
        else
        {
            var lines = File.ReadAllLines(filePath);
            if (lines.Length < 2)
            {
                var newLines = new string[2];
                newLines[0] = lines.Length > 0 ? lines[0] : defaultCityName;
                newLines[1] = "0";
                File.WriteAllLines(filePath, newLines);
            }
        }
    }

    void SaveLanguageIndex(int index)
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length < 2)
        {
            EnsureCityDataFile();
            lines = File.ReadAllLines(filePath);
        }
        lines[1] = index.ToString();
        File.WriteAllLines(filePath, lines);
    }

    int LoadLanguageIndex()
    {
        var lines = File.ReadAllLines(filePath);
        if (lines.Length >= 2 && int.TryParse(lines[1], out int index))
        {
            return index;
        }
        return 0;
    }
}
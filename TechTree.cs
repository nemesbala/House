using TMPro;
using System.IO;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TechTree : MonoBehaviour
{
    [Header("UI References")]
    public TMP_InputField nameInputField;
    public TMP_Text displayNameText;
    public GameObject CityHallUI;
    private string fileName = "CityData.txt";
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    private string filePath;
    public string[] savedName;

    [Header("Tech Tree Stuff")]
    public GameObject TechTreeUI;

    public GameObject Converter;

    public void Awake()
    {
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), fileName);
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        if (File.Exists(filePath))
        {
            savedName = File.ReadAllLines(filePath);
            displayNameText.text = savedName[0];
            nameInputField.text = savedName[0];
        }
        else
        {
            displayNameText.text = "Honeywood";
            nameInputField.text = "Honeywood";
        }
    }

    public void SavePlayerName()
    {
        savedName[0] = nameInputField.text;

        if (!string.IsNullOrWhiteSpace(savedName[0]))
        {
            File.WriteAllLines(filePath, savedName);
            displayNameText.text = savedName[0];
        }
        else
        {
            Debug.LogWarning("Player name is empty, not saved.");
        }
    }

    public void OpenTechTree()
    {
        TechTreeUI.SetActive(true);
        publicBoolForPauseMenuOpen.isAnUIOpened = true;
    }

    public void OpenConverter()
    {
        Converter.SetActive(true);
        publicBoolForPauseMenuOpen.isAnUIOpened = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (TechTreeUI.activeSelf == true || Converter.activeSelf == true)
            {
                TechTreeUI.SetActive(false);
                Converter.SetActive(false);
            }
            else if(CityHallUI.activeSelf == true)
            {
                CityHallUI.SetActive(false);
                publicBoolForPauseMenuOpen.isAnUIOpened = false;
            }
        }
    }
}

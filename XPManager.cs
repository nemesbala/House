using System;
using System.IO;
using UnityEngine;
using TMPro;  // Assuming you want to display XP and Level using TextMeshPro
using UnityEngine.UI;  // Required for using Slider

public class XPManager : MonoBehaviour
{
    public TMP_Text xpText;   // TextMeshPro Text component to display XP
    public TMP_Text levelText;  // TextMeshPro Text component to display Level
    public Slider xpSlider;  // Slider to display XP progress towards the next level

    private int currentXP = 0;
    private int currentLevel = 1;
    private string filePath;
    public PublicBoolForPauseMenuOpen publicBoolForPauseMenuOpen;
    public GameObject[] RewardsSet;

    private const int baseXPToLevelUp = 20;  // Base XP needed for the first level up
    private const float xpMultiplier = 1.6f;  // Multiplier for each subsequent level

    void Start()
    {
        // Define the file path for saving XP data
        filePath = Path.Combine(Application.persistentDataPath, "XPData.txt");
        publicBoolForPauseMenuOpen = FindObjectOfType<PublicBoolForPauseMenuOpen>();
        // Check if the file exists; if not, create it with initial values
        if (!File.Exists(filePath))
        {
            InitializeXPFile();
        }

        // Load XP from the file
        LoadXP();

        // Update the UI with the current XP and level
        UpdateXPDisplay();
    }

    // Method to add XP and update the level
    public void AddXP(int amount)
    {
        LevelBlocker[] blockers = FindObjectsOfType<LevelBlocker>();
        currentXP += amount;  // Add the received XP to the current XP
        // Check if the player can level up
        CheckLevelUp();
        SaveXP();
        // Update the UI with the new XP and level
        UpdateXPDisplay();

        foreach (LevelBlocker blocker in blockers)
        {
            // Call a method on each LevelBlocker
            blocker.OnEnable(); // Replace this with the appropriate function
        }
    }

    // Calculate the XP required for the next level
    private int CalculateXPForNextLevel(int level)
    {
        return (int)(baseXPToLevelUp * Mathf.Pow(xpMultiplier, level - 1));
    }

    // Check and handle level-up
    private void CheckLevelUp()
    {
        int xpForNextLevel = CalculateXPForNextLevel(currentLevel);

        while (currentXP >= xpForNextLevel)
        {
            currentLevel++;  // Increase the level
            int WhatToActivate = currentLevel - 2;
            RewardsSet[WhatToActivate].SetActive(true);
            publicBoolForPauseMenuOpen.isAnUIOpened = true;
            currentXP -= xpForNextLevel;  // Subtract the required XP for the next level
            xpForNextLevel = CalculateXPForNextLevel(currentLevel);  // Recalculate XP for the next level
        }
    }

    // Save the current XP and level to a text file
    private void SaveXP()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(currentXP);  // Write current XP on the first line
            writer.WriteLine(currentLevel);  // Write current level on the second line
        }
    }

    // Load XP and level from a text file
    private void LoadXP()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length >= 2)
            {
                currentXP = int.Parse(lines[0]);  // Read XP from the first line
                currentLevel = int.Parse(lines[1]);  // Read level from the second line
            }
        }
    }

    // Initialize the XP file with 0 XP and level 1
    private void InitializeXPFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine(0);  // Initial XP value
            writer.WriteLine(1);  // Initial level value
        }
        Debug.Log("XP file initialized with 0 XP and level 1.");
    }

    // Update the UI to show the current XP, level, and slider progress
    private void UpdateXPDisplay()
    {
        int xpForNextLevel = CalculateXPForNextLevel(currentLevel);
        xpText.text = "XP: " + currentXP + " / " + xpForNextLevel;
        levelText.text = "Level: " + currentLevel;
        float progress = (float)currentXP / xpForNextLevel;
        //Debug.Log(progress + " + " + currentXP + " + " + xpForNextLevel);
        xpSlider.value = progress;
    }
}
using UnityEngine;
using System.IO;

public class TechNodeLoader : MonoBehaviour
{
    [Header("Save File")]
    public string fileName = "techtree.txt";
    private string filePath;
    private string[] saveData;

    [Header("Tech Node Settings")]
    public int techLineIndex;            // Which line this tech node uses in the save file
    public int[] prerequisiteLines;      // Lines this tech depends on to unlock

    [Header("UI Overlays")]
    public GameObject unlockedOverlay;   // Shown if this tech is already unlocked
    public GameObject lockedOverlay;     // Shown if it's locked and cannot yet be unlocked

    public void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, fileName);
        LoadSaveFile();
        EvaluateState();
    }

    void LoadSaveFile()
    {
        if (File.Exists(filePath))
        {
            saveData = File.ReadAllLines(filePath);
        }
        else
        {
            Debug.LogError($"Save file not found at: {filePath}");
        }
    }

    void EvaluateState()
    {
        if (saveData == null || techLineIndex >= saveData.Length)
        {
            Debug.LogError($"Invalid tech line index: {techLineIndex}");
            return;
        }

        bool isUnlocked = saveData[techLineIndex] == "1";

        if (isUnlocked)
        {
            ShowUnlockedOverlay();
            return;
        }

        bool prerequisitesMet = true;
        foreach (int lineIndex in prerequisiteLines)
        {
            if (lineIndex >= saveData.Length || saveData[lineIndex] != "1")
            {
                prerequisitesMet = false;
                break;
            }
        }

        if (!prerequisitesMet)
        {
            ShowLockedOverlay();
        }
        else if(prerequisitesMet)
        {
            if (lockedOverlay != null) lockedOverlay.SetActive(false);
        }
    }

    public void UnlockTech()
    {
        if (saveData == null || techLineIndex >= saveData.Length)
        {
            Debug.LogError("Cannot unlock: Invalid save file or tech index.");
            return;
        }

        saveData[techLineIndex] = "1";
        File.WriteAllLines(filePath, saveData);
        EvaluateState(); // Refresh the visuals
    }

    void ShowUnlockedOverlay()
    {
        if (unlockedOverlay != null) unlockedOverlay.SetActive(true);
        if (lockedOverlay != null) lockedOverlay.SetActive(false);
    }

    void ShowLockedOverlay()
    {
        if (unlockedOverlay != null) unlockedOverlay.SetActive(false);
        if (lockedOverlay != null) lockedOverlay.SetActive(true);
    }
}
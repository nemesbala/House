using UnityEngine;
using UnityEngine.UI;
using TMPro; // Add this to use TextMeshPro
using System.Diagnostics;
using System.IO;

public class SaveFolderExplorer : MonoBehaviour
{
    // References to the UI Button, TextMeshPro components, and GameObject to toggle
    public Button openFolderButton;
    public TextMeshProUGUI pathText; // TextMeshPro component to display the path
    public GameObject objectToToggle; // The GameObject to show/hide
    public GameObject DeleteWarningUI;

    private bool isObjectActive = false; // Track the current state of the GameObject

    void Start()
    {
        // Display the persistentDataPath in the TextMeshPro field
        DisplayPersistentPath();
    }

    public void OpenFolder()
    {
        string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir");

        if (Directory.Exists(path))
        {
            // Use Process.Start to open the folder in file explorer
            Process.Start(path);
        }
    }

    void DisplayPersistentPath()
    {
        // Display the path in the TextMeshPro field
        string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir");
        pathText.text = "Save Folder: " + path;
    }

    public void WarningDeleteFolder()
    {
        DeleteWarningUI.SetActive(true);
    }

    public void NoDeleteFolder()
    {
        DeleteWarningUI.SetActive(false);
    }

    public void YesDeleteFolder()
    {
        string path = Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir");
        if (Directory.Exists(path))
        {
            // Delete the folder and its contents
            Directory.Delete(path, true); // 'true' to delete recursively
            DeleteWarningUI.SetActive(false);
        }
        else
        {
            DeleteWarningUI.SetActive(false);
        }
    }

    public void ToggleGameObject()
    {
        // Toggle the active state of the GameObject
        isObjectActive = !isObjectActive;
        objectToToggle.SetActive(isObjectActive);
    }
}

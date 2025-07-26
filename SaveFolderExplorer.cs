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
        // Add a listener to the button to call both OpenFolder and ToggleGameObject when clicked
        openFolderButton.onClick.AddListener(OpenFolder);

        // Display the persistentDataPath in the TextMeshPro field
        DisplayPersistentPath();
    }

    void OpenFolder()
    {
        string path = Application.persistentDataPath;

        if (Directory.Exists(path))
        {
            // Use Process.Start to open the folder in file explorer
            Process.Start(path);
        }
    }

    void DisplayPersistentPath()
    {
        // Display the path in the TextMeshPro field
        string path = Application.persistentDataPath;
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
        string path = Application.persistentDataPath;
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

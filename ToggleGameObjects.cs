using UnityEngine;

public class ToggleGameObjects : MonoBehaviour
{
    public GameObject[] enableSet;  // Array of game objects to be enabled
    public GameObject[] disableSet; // Array of game objects to be disabled

    // This method is called when the UI button is clicked
    public void ToggleObjects()
    {
        // Enable the game objects in the enable set
        foreach (GameObject obj in enableSet)
        {
            obj.SetActive(true);
        }

        // Disable the game objects in the disable set
        foreach (GameObject obj in disableSet)
        {
            obj.SetActive(false);
        }
    }
}
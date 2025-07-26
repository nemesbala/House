using UnityEngine;
using UnityEngine.UI; // For working with Unity's UI

public class ConstructionStarterMenuRoll : MonoBehaviour
{
    public GameObject[] gameObjects; // Array to hold all GameObjects
    public int groupSize = 4;        // Number of GameObjects per group
    private int currentGroupIndex = 0; // Tracks the currently active group

    public Button leftButton;  // Button to go to the previous group
    public Button rightButton; // Button to go to the next group

    private void Start()
    {
        // Attach button listeners
        if (leftButton != null)
            leftButton.onClick.AddListener(ActivatePreviousGroup);

        if (rightButton != null)
            rightButton.onClick.AddListener(ActivateNextGroup);

        // Activate the initial group and update button states
        ActivateGroup(currentGroupIndex);
        UpdateButtonStates();
    }

    // Activates a specific group based on the index
    public void ActivateGroup(int groupIndex)
    {
        // Ensure the group index is within valid bounds
        currentGroupIndex = Mathf.Clamp(groupIndex, 0, GetMaxGroupIndex());

        for (int i = 0; i < gameObjects.Length; i++)
        {
            int startIndex = currentGroupIndex * groupSize;
            int endIndex = startIndex + groupSize;

            // Activate or deactivate based on the current group
            gameObjects[i].SetActive(i >= startIndex && i < endIndex);
        }

        // Update button states after changing the group
        UpdateButtonStates();
    }

    // Activates the next group
    public void ActivateNextGroup()
    {
        if (currentGroupIndex < GetMaxGroupIndex())
        {
            currentGroupIndex++;
            ActivateGroup(currentGroupIndex);
        }
    }

    // Activates the previous group
    public void ActivatePreviousGroup()
    {
        if (currentGroupIndex > 0)
        {
            currentGroupIndex--;
            ActivateGroup(currentGroupIndex);
        }
    }

    // Updates the state (enabled/disabled) of the navigation buttons
    private void UpdateButtonStates()
    {
        int maxGroupIndex = GetMaxGroupIndex();

        // Disable the left button if on the first group
        if (leftButton != null)
            leftButton.interactable = currentGroupIndex > 0;

        // Disable the right button if on the last group
        if (rightButton != null)
            rightButton.interactable = currentGroupIndex < maxGroupIndex;
    }

    // Helper to get the maximum group index
    private int GetMaxGroupIndex()
    {
        return Mathf.CeilToInt((float)gameObjects.Length / groupSize) - 1;
    }
}
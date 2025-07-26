using UnityEngine;

public class PurchaseHandler : MonoBehaviour
{
    public int purchaseCost; // The cost of the purchase (set in the Inspector)
    public GameObject[] objectsToEnable; // Objects to enable if the player can afford the purchase
    public GameObject[] objectsToDisable; // Objects to disable if the player can afford the purchase

    private CashDisplay cashDisplay; // Reference to the CashDisplay script

    void Start()
    {
        // Find the CashDisplay script in the scene (assuming there's only one)
        cashDisplay = FindObjectOfType<CashDisplay>();

        if (cashDisplay != null)
        {
            // Check if the player has enough cash
            if (HasEnoughCash())
            {
                // Enable and disable objects based on the cash check
                SetObjectsActive(objectsToEnable, true);
                SetObjectsActive(objectsToDisable, false);
            }
            else
            {
                // If not enough cash, reverse the enabling/disabling
                SetObjectsActive(objectsToEnable, false);
                SetObjectsActive(objectsToDisable, true);
            }
        }
        else
        {
            Debug.LogError("CashDisplay script not found in the scene.");
        }
    }

    // Method to check if the player has enough cash
    private bool HasEnoughCash()
    {
        int currentCash = int.Parse(cashDisplay.cashText.text.Split('$')[1]);
        return currentCash >= purchaseCost;
    }

    // Method to deduct the cash and update the display
    private void DeductCash(int amount)
    {
        // Subtract the purchase cost from the current cash amount
        cashDisplay.SubtractCash(amount);

        // Update the saved cash file
        cashDisplay.SaveCashToFile();

        // Update the display
        cashDisplay.UpdateCashDisplay();
    }

    // Method to enable or disable a set of GameObjects
    private void SetObjectsActive(GameObject[] objects, bool isActive)
    {
        foreach (GameObject obj in objects)
        {
            obj.SetActive(isActive);
        }
    }
}

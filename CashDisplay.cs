using System.IO;
using TMPro;
using UnityEngine;

public class CashDisplay : MonoBehaviour
{
    public TextMeshProUGUI cashText; // Reference to the TextMeshPro UI component
    private string cashFilePath; // Path to the Cash.txt file
    public int cashAmount; // Variable to store the cash amount

    public void Start()
    {
        // Define the file path
        cashFilePath = Path.Combine(Application.persistentDataPath, "Cash.txt");

        // Check if the Cash.txt file exists
        if (File.Exists(cashFilePath))
        {
            // Read the cash amount from the file
            string cashString = File.ReadAllText(cashFilePath);
            if (int.TryParse(cashString, out cashAmount))
            {
                // Successfully parsed cash amount
                //Debug.Log($"Loaded cash amount: {cashAmount}");
            }
            else
            {
                // If the file content is invalid, set cash to 0
                //Debug.LogWarning("Invalid cash data. Resetting cash to 0.");
                cashAmount = 100;
                SaveCashToFile();
            }
        }
        else
        {
            cashAmount = 185;
            //Debug.Log("Hi!");
            SaveCashToFile();
        }

        // Display the cash amount
        UpdateCashDisplay();
    }

    // Method to update the TextMeshPro text with the current cash amount
    public void UpdateCashDisplay()
    {
        if (cashText != null)
        {
            cashText.text = $"{cashAmount}$";
        }
    }

    // Method to save the current cash amount to the Cash.txt file
    public void SaveCashToFile()
    {
        File.WriteAllText(cashFilePath, cashAmount.ToString());
        //Debug.Log($"Saved cash amount: {cashAmount} to {cashFilePath}");
    }

    // Example method to add cash (this can be called from other scripts)
    public void AddCash(int amount)
    {
        Start();
        cashAmount += amount;
        SaveCashToFile();
        UpdateCashDisplay();
        Debug.Log("Called");
    }

    // Example method to subtract cash (this can be called from other scripts)
    public void SubtractCash(int amount)
    {
        Start();
        cashAmount -= amount;
        SaveCashToFile();
        UpdateCashDisplay();
    }
}

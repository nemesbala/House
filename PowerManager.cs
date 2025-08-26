using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour
{
    public int Positive { get; private set; }
    public int Negative { get; private set; }
    public float Overall { get; private set; }

    [SerializeField] private Slider powerSlider; // Reference to the Slider UI

    private string filePath;

    private void Start()
    {
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "powerLevels.txt");
        LoadPowerLevels();
    }

    private void UpdateOverallPower()
    {
        if (Negative == 0)
        {
            Overall = Positive; // Avoid division by zero; assume Overall = Positive if Negative is zero.
        }
        else
        {
            Overall = (float)Positive / Negative;
        }

        // Cap Overall at 1 and update slider
        Overall = Mathf.Min(Overall, 1);
        powerSlider.value = Overall;

        SavePowerLevels();
    }

    public void IncreasePositive(int amount)
    {
        Positive += amount;
        UpdateOverallPower();
    }

    public void DecreasePositive(int amount)
    {
        Positive = Mathf.Max(0, Positive - amount);
        UpdateOverallPower();
    }

    public void IncreaseNegative(int amount)
    {
        Negative += amount;
        UpdateOverallPower();
    }

    public void DecreaseNegative(int amount)
    {
        Negative = Mathf.Max(1, Negative - amount); // Ensure Negative doesn't reach zero.
        UpdateOverallPower();
    }

    private void SavePowerLevels()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            writer.WriteLine("Positive=" + Positive);
            writer.WriteLine("Negative=" + Negative);
            writer.WriteLine("Overall=" + Overall);
        }
    }

    private void LoadPowerLevels()
    {
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] parts = line.Split('=');
                    if (parts.Length < 2) continue;

                    string key = parts[0].Trim();
                    int value;
                    float overallValue;

                    switch (key)
                    {
                        case "Positive":
                            if (int.TryParse(parts[1], out value)) Positive = value;
                            break;
                        case "Negative":
                            if (int.TryParse(parts[1], out value)) Negative = value;
                            break;
                        case "Overall":
                            if (float.TryParse(parts[1], out overallValue)) Overall = overallValue;
                            break;
                    }
                }
            }
            UpdateOverallPower();
        }
        else
        {
            Positive = 10;
            Negative = 20;
            Overall = 0;
            SavePowerLevels();
            LoadPowerLevels();
        }
    }
}

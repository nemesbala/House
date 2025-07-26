using UnityEngine;
using System.IO;

public class TechUnlockCostChecker : MonoBehaviour
{
    [Header("File Names")]
    public string cashFile = "Cash.txt";
    public string inventoryFile = "inventory.txt";
    public string wsPointsFile = "WSPoints.txt";

    [Header("Unlock Requirements")]
    public int requiredCash;
    public int inventoryLineID;
    public int requiredInventoryAmount;
    public int requiredWSPoints;

    [Header("Overlay if Cannot Unlock")]
    public GameObject cannotUnlockOverlay;

    private MoodManager Mood;
    private PowerManager Power;
    private CashDisplay cashDisplay;
    private WPandSP wpandsp;
    private string cashPath;
    private string inventoryPath;
    private string wsPointsPath; // need to construct this in the cash way

    public void Awake()
    {
        inventoryPath = Path.Combine(Application.persistentDataPath, inventoryFile);
        wsPointsPath = Path.Combine(Application.persistentDataPath, wsPointsFile);
        cashPath = Path.Combine(Application.persistentDataPath, cashFile);
        cashDisplay = FindObjectOfType<CashDisplay>();
        Mood = FindObjectOfType<MoodManager>();
        Power = FindObjectOfType<PowerManager>();
        wpandsp = FindObjectOfType<WPandSP>();
        CanUnlock();
    }

    public void AddWP(int WPToGive)
    {
        wpandsp.ChangeWPoints(WPToGive);
    }

    public void IncreaseMood(int MoodToGive)
    {
        Mood.IncreasePositive(MoodToGive);
    }

    public void IncreasePower(int PowerToGive)
    {
        Power.IncreasePositive(PowerToGive);
    }

    public void IncreaseCash(int CashChange)
    {
        cashDisplay.AddCash(CashChange);
    }

    public bool CanUnlock()
    {
        int cash = ReadIntFromFile(cashPath);
        int inventoryAmount = ReadInventoryAmount(inventoryPath, inventoryLineID);
        int wsPoints = ReadWSPoints(wsPointsPath);

        bool canUnlock = cash >= requiredCash && inventoryAmount >= requiredInventoryAmount && wsPoints >= requiredWSPoints;

        if (cannotUnlockOverlay != null)
            cannotUnlockOverlay.SetActive(!canUnlock);

        return canUnlock;
    }

    public void SubtractUnlockCosts()
    {
        if (!CanUnlock()) return;

        cashDisplay.SubtractCash(requiredCash);
        UpdateInventoryFile(inventoryPath, inventoryLineID, -requiredInventoryAmount);
        wpandsp.SubtracktSPoints(requiredWSPoints);
        //Debug.Log("Resources subtracted for unlock.");
    }

    private int ReadIntFromFile(string path)
    {
        if (!File.Exists(path)) return 0;
        string content = File.ReadAllText(path);
        int.TryParse(content, out int result);
        return result;
    }

    private int ReadInventoryAmount(string path, int lineIndex)
    {
        if (!File.Exists(path)) return 0;
        string[] lines = File.ReadAllLines(path);
        if (lineIndex >= lines.Length) return 0;

        int.TryParse(lines[lineIndex], out int result);
        return result;
    }

    private int ReadWSPoints(string path)
    {
        if (!File.Exists(path)) return 0;
        string line = File.ReadAllLines(path)[0];
        string[] values = line.Split(',');
        if (values.Length < 2) return 0;

        int.TryParse(values[1], out int result);
        return result;
    }

    private void UpdateIntFile(string path, int change)
    {
        int current = ReadIntFromFile(path);
        File.WriteAllText(path, (current + change).ToString());
    }

    private void UpdateInventoryFile(string path, int lineIndex, int change)
    {
        string[] lines = File.ReadAllLines(path);
        if (lineIndex >= lines.Length) return;

        int current = 0;
        int.TryParse(lines[lineIndex], out current);
        lines[lineIndex] = (current + change).ToString();

        File.WriteAllLines(path, lines);
    }

    private void UpdateWSPointsFile(string path, int change)
    {
        string[] lines = File.ReadAllLines(path);
        string[] values = lines[0].Split(',');
        if (values.Length < 2) return;

        int.TryParse(values[1], out int current);
        values[1] = (current + change).ToString();

        lines[0] = string.Join(",", values);
        File.WriteAllLines(path, lines);
    }
}
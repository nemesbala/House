using System;
using System.IO;
using UnityEngine;
using TMPro;

public class WPSPConverter : MonoBehaviour
{
    private string filePath;

    [Header("UI References")]
    public TMP_InputField WtoSInput;
    public TMP_InputField StoWInput;
    public GameObject Button;
    public TextMeshProUGUI WtoSOutputText;
    public TextMeshProUGUI StoWOutputText;

    [Header("Conversion Rates")]
    public float WtoSConversionRate = 1f; // Example: 1 WP gives 1 SP
    public float StoWConversionRate = 1f; // Example: 1 SP gives 1 WP
    private WPandSP wpandsp;
    private int Wpoints;
    private int Spoints;

    void Start()
    {
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "WSPoints.txt");
        UpdatePoints();
        UpdateWtoSPreview();
        UpdateStoWPreview();
        wpandsp = FindObjectOfType<WPandSP>();
        WtoSInput.onValueChanged.AddListener(delegate { UpdateWtoSPreview(); });
        StoWInput.onValueChanged.AddListener(delegate { UpdateStoWPreview(); });
    }

    private void UpdatePoints()
    {
        if (!File.Exists(filePath))
        {
            Debug.LogError($"WSPoints file missing: {filePath}");
            return;
        }

        string pointsString = File.ReadAllText(filePath);
        string[] data = pointsString.Split(',');
        Wpoints = int.Parse(data[0]);
        Spoints = int.Parse(data[1]);
    }

    public void UpdateWtoSPreview()
    {
        UpdatePoints();

        if (int.TryParse(WtoSInput.text, out int amount) && amount > 0)
        {
            if (Wpoints >= amount)
            {
                int result = Mathf.RoundToInt(amount * WtoSConversionRate);
                WtoSOutputText.text = $"+ {result} SP";
                Button.SetActive(true);
            }
            else
            {
                WtoSOutputText.text = "Not enough WP!";
                Button.SetActive(false);
            }
        }
        else
        {
            WtoSOutputText.text = "";
        }
    }

    public void UpdateStoWPreview()
    {
        UpdatePoints();

        if (int.TryParse(StoWInput.text, out int amount) && amount > 0)
        {
            if (Spoints >= amount)
            {
                int result = Mathf.RoundToInt(amount * StoWConversionRate);
                StoWOutputText.text = $"+ {result} WP";
                Button.SetActive(true);
            }
            else
            {
                StoWOutputText.text = "Not enough SP!";
                Button.SetActive(false);
            }
        }
        else
        {
            StoWOutputText.text = "";
        }
    }

    public void ConvertWtoS()
    {
        UpdatePoints();

        if (int.TryParse(WtoSInput.text, out int amount) && amount > 0 && Wpoints >= amount)
        {
            Wpoints -= amount;
            Spoints += Mathf.RoundToInt(amount * WtoSConversionRate);
            wpandsp.ChangeSPoints(Mathf.RoundToInt(amount * WtoSConversionRate));
            wpandsp.SubtrackWPoints(amount);
            SaveAndRefresh();
        }
    }

    public void ConvertStoW()
    {
        UpdatePoints();

        if (int.TryParse(StoWInput.text, out int amount) && amount > 0 && Spoints >= amount)
        {
            Spoints -= amount;
            Wpoints += Mathf.RoundToInt(amount * StoWConversionRate);
            wpandsp.ChangeWPoints(Mathf.RoundToInt(amount * StoWConversionRate));
            wpandsp.SubtracktSPoints(amount);
            SaveAndRefresh();
        }
    }

    private void SaveAndRefresh()
    {
        UpdateWtoSPreview();
        UpdateStoWPreview();
    }
}
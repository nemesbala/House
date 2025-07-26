using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class NewMarket : MonoBehaviour
{
    public GameObject TheButton;
    public int TradeCash = 0;
    public CashDisplay cashDisplay;

    [Header("Integer IDs to Select")]
    public int LumberID;
    public int WoodenBeamID;
    public int WoodenPanelID;
    public int BrickID;
    public int RoofTileID;
    public int ConcreteID;
    public int TileID;
    public int NailID;
    public int SteelBeamID;
    public int MetalSheetID;
    public int SteelRodID;
    public int ScrewID;
    public int TomatoID;
    public int PotatoID;
    public int WheatID;
    public int HopID;
    public int EggID;
    public int MilkID;
    public int BreadID;
    public int BeerID;
    public int DoughnutID;
    public int CheeseID;

    [Header("Prices")]
    public int LumberCost;
    public int WoodenBeamCost;
    public int WoodenPanelCost;
    public int BrickCost;
    public int RoofTileCost;
    public int ConcreteCost;
    public int TileCost;
    public int NailCost;
    public int SteelBeamCost;
    public int MetalSheetCost;
    public int SteelRodCost;
    public int ScrewCost;
    public int TomatoCost;
    public int PotatoCost;
    public int WheatCost;
    public int HopCost;
    public int EggCost;
    public int MilkCost;
    public int BreadCost;
    public int BeerCost;
    public int DoughnutCost;
    public int CheeseCost;
    public TMP_Text TradeCostTXT;

    [Header("Output Fields")]
    public TMP_Text LumberOutput;
    public TMP_Text WoodenBeamOutput;
    public TMP_Text WoodenPanelOutput;
    public TMP_Text BrickOutput;
    public TMP_Text RoofTileOutput;
    public TMP_Text ConcreteOutput;
    public TMP_Text TileOutput;
    public TMP_Text NailOutput;
    public TMP_Text SteelBeamOutput;
    public TMP_Text MetalSheetOutput;
    public TMP_Text SteelRodOutput;
    public TMP_Text ScrewOutput;
    public TMP_Text TomatoOutput;
    public TMP_Text PotatoOutput;
    public TMP_Text WheatOutput;
    public TMP_Text HopOutput;
    public TMP_Text EggOutput;
    public TMP_Text MilkOutput;
    public TMP_Text BreadOutput;
    public TMP_Text BeerOutput;
    public TMP_Text DoughnutOutput;
    public TMP_Text CheeseOutput;

    [Header("Input Fields")]
    public TMP_InputField LumberInput;
    public TMP_InputField WoodenBeamInput;
    public TMP_InputField WoodenPanelInput;
    public TMP_InputField BrickInput;
    public TMP_InputField RoofTileInput;
    public TMP_InputField ConcreteInput;
    public TMP_InputField TileInput;
    public TMP_InputField NailInput;
    public TMP_InputField SteelBeamInput;
    public TMP_InputField MetalSheetInput;
    public TMP_InputField SteelRodInput;
    public TMP_InputField ScrewInput;
    public TMP_InputField TomatoInput;
    public TMP_InputField PotatoInput;
    public TMP_InputField WheatInput;
    public TMP_InputField HopInput;
    public TMP_InputField EggInput;
    public TMP_InputField MilkInput;
    public TMP_InputField BreadInput;
    public TMP_InputField BeerInput;
    public TMP_InputField DoughnutInput;
    public TMP_InputField CheeseInput;

    private string fullPath;
    private List<int> selectedIntegers = new List<int>();

    public void Start()
    {
        fullPath = Path.Combine(Application.persistentDataPath, "inventory.txt");
        cashDisplay = FindObjectOfType<CashDisplay>();
        EnsureFileExists();
        selectedIntegers = GetSelectedIntegers();
        UpdateUI(selectedIntegers);
        TradeCostTXT.text = TradeCash.ToString() + "$";
    }

    private void EnsureFileExists()
    {
        if (!File.Exists(fullPath))
        {
            Debug.LogWarning("File not found. Creating default file.");
            using (StreamWriter writer = new StreamWriter(fullPath))
            {
                for (int i = 0; i < 100; i++)
                {
                    writer.WriteLine("0"); // Default to 0 for all items
                }
            }
        }
    }

    private List<int> GetSelectedIntegers()
    {
        List<int> integers = new List<int>();

        string[] lines = File.ReadAllLines(fullPath);

        List<int> idsToSelect = new List<int>
        {
            LumberID, WoodenBeamID, WoodenPanelID, BrickID, RoofTileID, ConcreteID, TileID, NailID,
            SteelBeamID, MetalSheetID, SteelRodID, ScrewID, TomatoID, PotatoID, WheatID, HopID,
            EggID, MilkID, BreadID, BeerID, DoughnutID, CheeseID
        };

        foreach (int id in idsToSelect)
        {
            if (id >= 0 && id < lines.Length)
            {
                if (int.TryParse(lines[id], out int value))
                {
                    integers.Add(value);
                }
                else
                {
                    Debug.LogWarning($"Invalid integer at line {id}: {lines[id]}");
                    integers.Add(0);
                }
            }
            else
            {
                Debug.LogWarning($"ID {id} is out of bounds for the file.");
                integers.Add(0);
            }
        }

        return integers;
    }

    public void FinalizeAddition()
    {
        List<int> inputValues = GetInputValues();
        List<int> updatedValues = new List<int>();
        cashDisplay.SubtractCash(TradeCash);
        for (int i = 0; i < selectedIntegers.Count; i++)
        {
            int result = selectedIntegers[i] + inputValues[i];
            if (result >= 0)
            {
                updatedValues.Add(result);
            }
            else
            {
                Debug.LogWarning($"Final value for item at index {i} cannot go below 0. Skipping addition.");
                updatedValues.Add(selectedIntegers[i]); // Keep original value if addition is invalid
            }
        }

        selectedIntegers = updatedValues;
        SaveUpdatedValues(updatedValues);
        UpdateUI(updatedValues);
        ClearInputFields();
    }

    private List<int> GetInputValues()
    {
        return new List<int>
        {
            ParseInputField(LumberInput), ParseInputField(WoodenBeamInput), ParseInputField(WoodenPanelInput),
            ParseInputField(BrickInput), ParseInputField(RoofTileInput), ParseInputField(ConcreteInput),
            ParseInputField(TileInput), ParseInputField(NailInput), ParseInputField(SteelBeamInput),
            ParseInputField(MetalSheetInput), ParseInputField(SteelRodInput), ParseInputField(ScrewInput),
            ParseInputField(TomatoInput), ParseInputField(PotatoInput), ParseInputField(WheatInput),
            ParseInputField(HopInput), ParseInputField(EggInput), ParseInputField(MilkInput),
            ParseInputField(BreadInput), ParseInputField(BeerInput), ParseInputField(DoughnutInput),
            ParseInputField(CheeseInput)
        };
    }

    private int ParseInputField(TMP_InputField inputField)
    {
        if (int.TryParse(inputField.text, out int value))
        {
            return value;
        }

        Debug.LogWarning($"Invalid input in field {inputField.name}. Defaulting to 0.");
        inputField.text = "0"; // Reset invalid input to 0
        return 0;
    }

    private void HighlightInvalidInput(TMP_InputField inputField)
    {
        inputField.textComponent.color = Color.red;
    }

    private void ResetInputFieldColor(TMP_InputField inputField)
    {
        inputField.textComponent.color = Color.black;
    }

    private void SaveUpdatedValues(List<int> updatedValues)
    {
        string[] lines = File.ReadAllLines(fullPath);

        List<int> idsToSelect = new List<int>
        {
            LumberID, WoodenBeamID, WoodenPanelID, BrickID, RoofTileID, ConcreteID, TileID, NailID,
            SteelBeamID, MetalSheetID, SteelRodID, ScrewID, TomatoID, PotatoID, WheatID, HopID,
            EggID, MilkID, BreadID, BeerID, DoughnutID, CheeseID
        };

        for (int i = 0; i < idsToSelect.Count; i++)
        {
            int id = idsToSelect[i];
            if (id >= 0 && id < lines.Length)
            {
                lines[id] = updatedValues[i].ToString();
            }
        }

        File.WriteAllLines(fullPath, lines);
        Debug.Log("Values saved to inventory.txt.");
    }

    private void UpdateUI(List<int> integers)
    {
        LumberOutput.text = integers[0].ToString();
        WoodenBeamOutput.text = integers[1].ToString();
        WoodenPanelOutput.text = integers[2].ToString();
        BrickOutput.text = integers[3].ToString();
        RoofTileOutput.text = integers[4].ToString();
        ConcreteOutput.text = integers[5].ToString();
        TileOutput.text = integers[6].ToString();
        NailOutput.text = integers[7].ToString();
        SteelBeamOutput.text = integers[8].ToString();
        MetalSheetOutput.text = integers[9].ToString();
        SteelRodOutput.text = integers[10].ToString();
        ScrewOutput.text = integers[11].ToString();
        TomatoOutput.text = integers[12].ToString();
        PotatoOutput.text = integers[13].ToString();
        WheatOutput.text = integers[14].ToString();
        HopOutput.text = integers[15].ToString();
        EggOutput.text = integers[16].ToString();
        MilkOutput.text = integers[17].ToString();
        BreadOutput.text = integers[18].ToString();
        BeerOutput.text = integers[19].ToString();
        DoughnutOutput.text = integers[20].ToString();
        CheeseOutput.text = integers[21].ToString();
    }

    private void ClearInputFields()
    {
        LumberInput.text = "0";
        WoodenBeamInput.text = "0";
        WoodenPanelInput.text = "0";
        BrickInput.text = "0";
        RoofTileInput.text = "0";
        ConcreteInput.text = "0";
        TileInput.text = "0";
        NailInput.text = "0";
        SteelBeamInput.text = "0";
        MetalSheetInput.text = "0";
        SteelRodInput.text = "0";
        ScrewInput.text = "0";
        TomatoInput.text = "0";
        PotatoInput.text = "0";
        WheatInput.text = "0";
        HopInput.text = "0";
        EggInput.text = "0";
        MilkInput.text = "0";
        BreadInput.text = "0";
        BeerInput.text = "0";
        DoughnutInput.text = "0";
        CheeseInput.text = "0";
    }

    public void AllCheck()
    {
        TradeCash = 0;

        if ((ValidateBeerInput() && ValidateBreadInput() && ValidateBrickInput() && ValidateCheeseInput() && ValidateConcreteInput() && ValidateDoughnutInput() && ValidateEggInput() && ValidateHopInput() && ValidateLumberInput() && ValidateMetalSheetInput() && ValidateMilkInput() && ValidateNailInput() && ValidatePotatoInput() && ValidateRoofTileInput() && ValidateScrewInput() && ValidateSteelBeamInput() && ValidateSteelRodInput() && ValidateTileInput() && ValidateTomatoInput() && ValidateWheatInput() && ValidateWoodenBeamInput() && ValidateWoodenPanelInput()) && (cashDisplay.cashAmount >= TradeCash))
        {
            TheButton.SetActive(true);
        }
        else
        {
            TheButton.SetActive(false);
        }

        if(TradeCash > 0)
        {
            TradeCostTXT.color = Color.red;
        }
        else
        {
            TradeCostTXT.color = new Color(0f, 0.6f, 0f);
        }
        TradeCostTXT.text = TradeCash.ToString() + "$";
    }

    public bool ValidateLumberInput()
    {
        if (int.TryParse(LumberInput.text, out int value))
        {
            int finalValue = selectedIntegers[0] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(LumberInput);
                TradeCash += value * LumberCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(LumberInput);
                TradeCash += value * LumberCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(LumberInput);
            return false;
        }
    }

    public bool ValidateWoodenBeamInput()
    {
        if (int.TryParse(WoodenBeamInput.text, out int value))
        {
            int finalValue = selectedIntegers[1] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(WoodenBeamInput);
                TradeCash += value * WoodenBeamCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(WoodenBeamInput);
                TradeCash += value * WoodenBeamCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(WoodenBeamInput);
            return false;
        }
    }

    public bool ValidateWoodenPanelInput()
    {
        if (int.TryParse(WoodenPanelInput.text, out int value))
        {
            int finalValue = selectedIntegers[2] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(WoodenPanelInput);
                TradeCash += value * WoodenPanelCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(WoodenPanelInput);
                TradeCash += value * WoodenPanelCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(WoodenPanelInput);
            return false;
        }
    }

    public bool ValidateBrickInput()
    {
        if (int.TryParse(BrickInput.text, out int value))
        {
            int finalValue = selectedIntegers[3] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(BrickInput);
                TradeCash += value * BrickCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(BrickInput);
                TradeCash += value * BrickCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(BrickInput);
            return false;
        }
    }

    public bool ValidateRoofTileInput()
    {
        if (int.TryParse(RoofTileInput.text, out int value))
        {
            int finalValue = selectedIntegers[4] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(RoofTileInput);
                TradeCash += value * RoofTileCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(RoofTileInput);
                TradeCash += value * RoofTileCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(RoofTileInput);
            return false;
        }
    }

    public bool ValidateConcreteInput()
    {
        if (int.TryParse(ConcreteInput.text, out int value))
        {
            int finalValue = selectedIntegers[5] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(ConcreteInput);
                TradeCash += value * ConcreteCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(ConcreteInput);
                TradeCash += value * ConcreteCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(ConcreteInput);
            return false;
        }
    }

    public bool ValidateTileInput()
    {
        if (int.TryParse(TileInput.text, out int value))
        {
            int finalValue = selectedIntegers[6] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(TileInput);
                TradeCash += value * TileCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(TileInput);
                TradeCash += value * TileCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(TileInput);
            return false;
        }
    }

    public bool ValidateNailInput()
    {
        if (int.TryParse(NailInput.text, out int value))
        {
            int finalValue = selectedIntegers[7] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(NailInput);
                TradeCash += value * NailCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(NailInput);
                TradeCash += value * NailCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(NailInput);
            return false;
        }
    }

    public bool ValidateSteelBeamInput()
    {
        if (int.TryParse(SteelBeamInput.text, out int value))
        {
            int finalValue = selectedIntegers[8] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(SteelBeamInput);
                TradeCash += value * SteelBeamCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(SteelBeamInput);
                TradeCash += value * SteelBeamCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(SteelBeamInput);
            return false;
        }
    }

    public bool ValidateMetalSheetInput()
    {
        if (int.TryParse(MetalSheetInput.text, out int value))
        {
            int finalValue = selectedIntegers[9] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(MetalSheetInput);
                TradeCash += value * MetalSheetCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(MetalSheetInput);
                TradeCash += value * MetalSheetCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(MetalSheetInput);
            return false;
        }
    }

    public bool ValidateSteelRodInput()
    {
        if (int.TryParse(SteelRodInput.text, out int value))
        {
            int finalValue = selectedIntegers[10] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(SteelRodInput);
                TradeCash += value * SteelRodCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(SteelRodInput);
                TradeCash += value * SteelRodCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(SteelRodInput);
            return false;
        }
    }

    public bool ValidateScrewInput()
    {
        if (int.TryParse(ScrewInput.text, out int value))
        {
            int finalValue = selectedIntegers[11] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(ScrewInput);
                TradeCash += value * ScrewCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(ScrewInput);
                TradeCash += value * ScrewCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(ScrewInput);
            return false;
        }
    }

    public bool ValidateTomatoInput()
    {
        if (int.TryParse(TomatoInput.text, out int value))
        {
            int finalValue = selectedIntegers[12] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(TomatoInput);
                TradeCash += value * TomatoCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(TomatoInput);
                TradeCash += value * TomatoCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(TomatoInput);
            return false;
        }
    }

    public bool ValidatePotatoInput()
    {
        if (int.TryParse(PotatoInput.text, out int value))
        {
            int finalValue = selectedIntegers[13] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(PotatoInput);
                TradeCash += value * PotatoCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(PotatoInput);
                TradeCash += value * PotatoCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(PotatoInput);
            return false;
        }
    }

    public bool ValidateWheatInput()
    {
        if (int.TryParse(WheatInput.text, out int value))
        {
            int finalValue = selectedIntegers[14] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(WheatInput);
                TradeCash += value * WheatCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(WheatInput);
                TradeCash += value * WheatCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(WheatInput);
            return false;
        }
    }

    public bool ValidateHopInput()
    {
        if (int.TryParse(HopInput.text, out int value))
        {
            int finalValue = selectedIntegers[15] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(HopInput);
                TradeCash += value * HopCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(HopInput);
                TradeCash += value * HopCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(HopInput);
            return false;
        }
    }

    public bool ValidateEggInput()
    {
        if (int.TryParse(EggInput.text, out int value))
        {
            int finalValue = selectedIntegers[16] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(EggInput);
                TradeCash += value * EggCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(EggInput);
                TradeCash += value * EggCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(EggInput);
            return false;
        }
    }

    public bool ValidateMilkInput()
    {
        if (int.TryParse(MilkInput.text, out int value))
        {
            int finalValue = selectedIntegers[17] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(MilkInput);
                TradeCash += value * MilkCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(MilkInput);
                TradeCash += value * MilkCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(MilkInput);
            return false;
        }
    }

    public bool ValidateBreadInput()
    {
        if (int.TryParse(BreadInput.text, out int value))
        {
            int finalValue = selectedIntegers[18] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(BreadInput);
                TradeCash += value * BreadCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(BreadInput);
                TradeCash += value * BreadCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(BreadInput);
            return false;
        }
    }

    public bool ValidateBeerInput()
    {
        if (int.TryParse(BeerInput.text, out int value))
        {
            int finalValue = selectedIntegers[19] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(BeerInput);
                TradeCash += value * BeerCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(BeerInput);
                TradeCash += value * BeerCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(BeerInput);
            return false;
        }
    }

    public bool ValidateDoughnutInput()
    {
        if (int.TryParse(DoughnutInput.text, out int value))
        {
            int finalValue = selectedIntegers[20] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(DoughnutInput);
                TradeCash += value * DoughnutCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(DoughnutInput);
                TradeCash += value * DoughnutCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(DoughnutInput);
            return false;
        }
    }

    public bool ValidateCheeseInput()
    {
        if (int.TryParse(CheeseInput.text, out int value))
        {
            int finalValue = selectedIntegers[21] + value;

            if (finalValue < 0)
            {
                HighlightInvalidInput(CheeseInput);
                TradeCash += value * CheeseCost;
                return false;
            }
            else
            {
                ResetInputFieldColor(CheeseInput);
                TradeCash += value * CheeseCost;
                return true;
            }
        }
        else
        {
            HighlightInvalidInput(CheeseInput);
            return false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;

public class LevelUpRewarder : MonoBehaviour
{
    public CashDisplay cashdisplay;
    public int CashToGive;
    public GameObject ThisUI;
    public int IDForInventory;
    public int NumberOfInventory;

    public int IDForBlueprint;
    public int NumberOfBlueprints;

    public void ButtonPressed()
    {
        cashdisplay.AddCash(CashToGive);
        string inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "inventory.txt");
        string[] lines = File.ReadAllLines(inventoryFilePath);
        int[] data = lines.Select(int.Parse).ToArray();
        data[IDForInventory] = NumberOfInventory + data[IDForInventory];

        using (StreamWriter writer = new StreamWriter(inventoryFilePath))
        {
            foreach (int element in data)
            {
                writer.WriteLine(element); // Write each element on a new line
            }
        }

        inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Blueprints.txt");
        string[] line = File.ReadAllLines(inventoryFilePath);
        string A = line[0];
        string[] B = A.Split(',');
        int C = int.Parse(B[IDForBlueprint]);
        C = C + NumberOfBlueprints;
        B[IDForBlueprint] = C.ToString();
        A = string.Join(",", B);

        File.WriteAllText(inventoryFilePath, A);

        ThisUI.SetActive(false);
    }
}

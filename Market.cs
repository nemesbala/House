using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class Market : MonoBehaviour
{
    private CashDisplay cashDisplay;
    private SimplifiedInventory simplifiedInventory;
    public int price;
    public bool bWood = false;
    public bool bNail = false;
    public bool bGlass = false;
    public bool bBrick = false;
    public bool bPlank = false;
    public bool bSteel = false;
    public bool Sell = false;
    public bool Buy = false;

    private string inventoryFilePath;

    // Start is called before the first frame update
    public void Start()
    {
        cashDisplay = FindObjectOfType<CashDisplay>();
        simplifiedInventory = FindObjectOfType<SimplifiedInventory>();
        inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "inventory.txt");
    }

    public void Trade()
    {
        cashDisplay.SubtractCash(price);

        //--Change the materials--
        string[] datas = File.ReadAllLines(inventoryFilePath);
        string data = datas[0];
        string[] parts = data.Split(',');

        int Wood = int.Parse(parts[0]);
        int Nail = int.Parse(parts[1]);
        int Glass = int.Parse(parts[2]);
        int Brick = int.Parse(parts[3]);
        int Plank = int.Parse(parts[4]);
        int Steel = int.Parse(parts[5]);

        if(bWood)
        {
            if(Sell)
            {
                Wood = Wood - 1;
            }
            else if(Buy)
            {
                Wood = Wood + 1;
            }
        }
        else if(bNail)
        {
            if (Sell)
            {
                Nail = Nail - 1;
            }
            else if (Buy)
            {
                Nail = Nail + 1;
            }
        }
        else if (bGlass)
        {
            if (Sell)
            {
                Glass = Glass - 1;
            }
            else if (Buy)
            {
                Glass = Glass + 1;
            }
        }
        else if (bBrick)
        {
            if (Sell)
            {
                Brick = Brick - 1;
            }
            else if (Buy)
            {
                Brick = Brick + 1;
            }
        }
        else if (bPlank)
        {
            if (Sell)
            {
                Plank = Plank - 1;
            }
            else if (Buy)
            {
                Plank = Plank + 1;
            }
        }
        else if (bSteel)
        {
            if (Sell)
            {
                Steel = Steel - 1;
            }
            else if (Buy)
            {
                Steel = Steel + 1;
            }
        }

        string Wdata = $"{Wood},{Nail},{Glass},{Brick},{Plank},{Steel}";
        using (StreamWriter Wwriter = new StreamWriter(inventoryFilePath)) // 'true' enables appending
        {
            Wwriter.WriteLine(Wdata);
        }
    }
}

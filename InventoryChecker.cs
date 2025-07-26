using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryChecker : MonoBehaviour
{
    public GameObject Button0;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;
    public GameObject Button5;

    // Start is called before the first frame update
    public void Start()
    {
        string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
        string[] datas = File.ReadAllLines(inventoryFilePath);
        string data = datas[0];
        string[] parts = data.Split(',');

        int Wood = int.Parse(parts[0]);
        int Nail = int.Parse(parts[1]);
        int Glass = int.Parse(parts[2]);
        int Brick = int.Parse(parts[3]);
        int Plank = int.Parse(parts[4]);
        int Steel = int.Parse(parts[5]);

        if(Wood <= 0)
        {
            Button0.SetActive(false);
        }

        if (Nail <= 0)
        {
            Button1.SetActive(false);
        }

        if (Glass <= 0)
        {
            Button2.SetActive(false);
        }

        if (Brick <= 0)
        {
            Button3.SetActive(false);
        }

        if (Plank <= 0)
        {
            Button4.SetActive(false);
        }

        if (Steel <= 0)
        {
            Button5.SetActive(false);
        }
    }
}

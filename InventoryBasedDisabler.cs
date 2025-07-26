using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;

public class InventoryBasedDisabler : MonoBehaviour
{
    public GameObject ChangeBlocker;
    public TextMeshProUGUI NumberOfChanger;
    // Start is called before the first frame update
    public void Start()
    {
        string inventoryFilePath = Path.Combine(Application.persistentDataPath, "inventory.txt");
        string[] datas = File.ReadAllLines(inventoryFilePath);
        int CitizenTypeChange = int.Parse(datas[34]);
        NumberOfChanger.text = CitizenTypeChange.ToString();
        if (CitizenTypeChange > 0)
        {
            ChangeBlocker.SetActive(false);
        }
        else
        {
            ChangeBlocker.SetActive(true);
        }
    }
}

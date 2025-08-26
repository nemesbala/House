using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using TMPro;

public class CashChecker : MonoBehaviour
{
    private string cashFilePath;
    public int AmmountToCheck;
    public int blueprintCount;
    public int Building_ID;
    public BlueprintChecker blueprintChecker;
    public TextMeshProUGUI blueprintText;
    public bool CheckBlueprints = true;
    public GameObject[] Enable;
    public GameObject[] Disable;

    // Start is called before the first frame update
    public void Start()
    {
        StartCall(Building_ID);
    }

    public void StartCall(int buildingID)
    {
        if(CheckBlueprints)
        {
            blueprintCount = blueprintChecker.GetBlueprintCount(buildingID);
            blueprintText.text = blueprintCount.ToString();
        }
        else
        {
            blueprintCount = 1;
        }

        cashFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Cash.txt");
        int Cash = Int32.Parse(File.ReadAllText(cashFilePath));

        if(Cash < AmmountToCheck || blueprintCount <= 0)
        {
            // Enable the game objects in the enable set
            foreach (GameObject obj in Enable)
            {
                obj.SetActive(true);
            }

            // Disable the game objects in the disable set
            foreach (GameObject obj in Disable)
            {
                obj.SetActive(false);
            }
        }
        else
        {
            // Enable the game objects in the enable set
            foreach (GameObject obj in Enable)
            {
                obj.SetActive(false);
            }

            // Disable the game objects in the disable set
            foreach (GameObject obj in Disable)
            {
                obj.SetActive(true);
            }
        }
    }
}

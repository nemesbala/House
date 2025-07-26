using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlueprintCountDisplayer : MonoBehaviour
{
    public int buildingID;
    public BlueprintChecker blueprintChecker;
    public TextMeshProUGUI blueprintText;
    public GameObject ConstructionButton;
    // Start is called before the first frame update
    public void Start()
    {
        int blueprintCount = blueprintChecker.GetBlueprintCount(buildingID);
        blueprintText.text = blueprintCount.ToString();
        Debug.Log(blueprintCount + "nbr of bluprnt");
        if (blueprintCount > 0)
        {
            ConstructionButton.SetActive(true);
        }
        else
        {
            ConstructionButton.SetActive(false);
        }
    }
}
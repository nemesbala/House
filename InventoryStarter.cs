using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class InventoryStarter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string inventoryFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "inventory.txt");
        int[] data = new int[37];
        // Check if the file exists, otherwise initialize with 0s
        if (!File.Exists(inventoryFilePath))
        {
            //Debug.Log("File does not exist. Initializing with 34 zeros.");
            data[0] = 10;
            data[14] = 10;
            data[34] = 3;

            using (StreamWriter writer = new StreamWriter(inventoryFilePath))
            {
                foreach (int element in data)
                {
                    writer.WriteLine(element); // Write each element on a new line
                }
            }
        }
    }
}

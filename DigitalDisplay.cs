using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;

public class DigitalDisplay : MonoBehaviour
{
    public TMP_Text TextA1;
    public TMP_Text TextB1;
    public TMP_Text TextA12;
    public TMP_Text TextB12;
    public TMP_Text TextA2;
    public TMP_Text TextB2;
    public TMP_Text TextA23;
    public TMP_Text TextB23;
    public TMP_Text TextA3;
    public TMP_Text TextB3;
    public TMP_InputField Input;
    public House houseScript;
    public string ID;

    // Start is called before the first frame update
    public void Start()
    {
        string houseID = houseScript.houseID.ToString();
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        if(File.Exists(filePath))
        {
            string[] datas = File.ReadAllLines(filePath);
            //Debug.Log(lines.Length);
            if (datas.Length > 1)
            {
                string data = datas[1];
                Input.text = data;
                TextA1.text = Input.text;
                TextB1.text = Input.text;
                TextA12.text = Input.text;
                TextB12.text = Input.text;
                TextA2.text = Input.text;
                TextB2.text = Input.text;
                TextA23.text = Input.text;
                TextB23.text = Input.text;
                TextA3.text = Input.text;
                TextB3.text = Input.text;
            }
        }
    }

    public void NewText()
    {
        TextA1.text = Input.text;
        TextB1.text = Input.text;
        TextA12.text = Input.text;
        TextB12.text = Input.text;
        TextA2.text = Input.text;
        TextB2.text = Input.text;
        TextA23.text = Input.text;
        TextB23.text = Input.text;
        TextA3.text = Input.text;
        TextB3.text = Input.text;

        // Read all lines from the file
        string houseID = houseScript.houseID.ToString();
        string filePath = Path.Combine(Application.persistentDataPath, "Building", houseID + ".txt");
        string[] lines = File.ReadAllLines(filePath);

        using (StreamWriter writer = new StreamWriter(filePath)) // 'true' enables appending
        {
            writer.WriteLine(lines[0]);
            writer.WriteLine(Input.text);
        }
    }
}

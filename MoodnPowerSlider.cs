using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class MoodnPowerSlider : MonoBehaviour
{
    public Slider MoodSlider;
    public Slider PowerSlider;

    // Start is called before the first frame update
    void Start()
    {
        string PowerMoodFilePath;
        PowerMoodFilePath = Path.Combine(Application.persistentDataPath, "PowerMood.txt");
        if (!File.Exists(PowerMoodFilePath))
        {
            using (StreamWriter writer = new StreamWriter(PowerMoodFilePath))
            {
                writer.WriteLine(50);  // Initial Power value
                writer.WriteLine(50);  // Initial Mood value
            }
            MoodSlider.value = 50;
            PowerSlider.value = 50;
        }
        else
        {
            string[] lines = File.ReadAllLines(PowerMoodFilePath);
            PowerSlider.value = int.Parse(lines[0]);  // Read Power from the first line
            MoodSlider.value = int.Parse(lines[1]);
        }
    }
}

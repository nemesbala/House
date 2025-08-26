using UnityEngine;
using UnityEngine.Events;
using System.IO;

public class TechNodeEventGate : MonoBehaviour
{
    [Header("File Settings")]
    public string fileName = "techtree.txt";
    public int techLineIndex;

    [Header("Events")]
    public UnityEvent onUnlockedEvent;

    private string filePath;

    void Awake()
    {
        filePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), fileName);
        TryRunEvent();
    }

    public void TryRunEvent()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (techLineIndex < lines.Length && lines[techLineIndex] == "1")
            {
                onUnlockedEvent.Invoke();
                //Debug.Log($"Tech node at line {techLineIndex} is unlocked. Event triggered.");
            }
            else
            {
                //Debug.Log($"Tech node at line {techLineIndex} is locked. Event not triggered.");
            }
        }
        else
        {
            Debug.LogError($"Save file not found at: {filePath}");
        }
    }
}
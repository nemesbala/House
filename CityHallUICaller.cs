using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CityHallUICaller : MonoBehaviour
{
    public string targetName = "CityHall(Clone)";
    [SerializeField] private AudioSource uiAudioSource;
    public AudioClip InvalidPlaceClip;
    public GameObject ErrorUI;

    public void OnButtonClick()
    {
        // Try to find the GameObject in the scene
        GameObject target = GameObject.Find(targetName);

        if (target != null)
        {
            // Call the function on the found GameObject
            target.GetComponent<CityHall>().OnClick();
        }
        else
        {
            // Fallback behavior if the GameObject was not found
            ErrorUI.SetActive(false);
            ErrorUI.SetActive(true);
            if (InvalidPlaceClip != null)
            {
                string SFXFilePath;
                SFXFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Audio.txt");
                string[] datas = File.ReadAllLines(SFXFilePath);
                float volume = float.Parse(datas[0]);
                GameObject Cam = GameObject.Find("Main Camera");
                uiAudioSource.PlayOneShot(InvalidPlaceClip, volume);
            }
        }
    }
}

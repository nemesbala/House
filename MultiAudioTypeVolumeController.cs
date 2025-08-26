using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;
using UnityEngine.Audio;

public class MultiAudioTypeVolumeController : MonoBehaviour
{
    [Header("Volume Sliders")]
    [SerializeField] private Slider sfxSlider;      // Slider for SFX volume
    [SerializeField] private Slider musicSlider;    // Slider for Music volume
    [SerializeField] private Slider ambianceSlider; // Slider for Ambiance volume
    public AudioMixer audioMixer;
    [Header("Audio Sources")]
    [SerializeField] private List<AudioSource> sfxSources;      // List of SFX audio sources
    [SerializeField] private List<AudioSource> musicSources;    // List of Music audio sources
    [SerializeField] private List<AudioSource> ambianceSources; // List of Ambiance audio sources
    private string FilePath;

    void Start()
    {
        FilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Audio.txt");

        if (File.Exists(FilePath))
        {
            string[] datas = File.ReadAllLines(FilePath);
            sfxSlider.value = float.Parse(datas[0]);
            musicSlider.value = float.Parse(datas[1]);
            ambianceSlider.value = float.Parse(datas[2]);
            SetAmbianceVolume(float.Parse(datas[2]));
            SetMusicVolume(float.Parse(datas[1]));
            SetSFXVolume(float.Parse(datas[0]));
        }
        else
        {
            sfxSlider.value = 0.4f;
            musicSlider.value = 0;
            ambianceSlider.value = 0.4f;
            using (StreamWriter writer = new StreamWriter(FilePath)) // 'true' enables appending
            {
                writer.WriteLine(1);
                writer.WriteLine(1);
                writer.WriteLine(1);
            }
            SetAmbianceVolume(0.4f);
            SetMusicVolume(0);
            SetSFXVolume(0.4f);
        }

        // Add listeners to detect slider value changes
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        ambianceSlider.onValueChanged.AddListener(SetAmbianceVolume);
    }

    // Set volume for all SFX audio sources
    private void SetSFXVolume(float volume)
    {
        string[] datas = File.ReadAllLines(FilePath);

        foreach (var audioSource in sfxSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        using (StreamWriter writer = new StreamWriter(FilePath)) // 'true' enables appending
        {
            writer.WriteLine(volume);
            writer.WriteLine(datas[1]);
            writer.WriteLine(datas[2]);
        }
    }

    // Set volume for all Music audio sources
    private void SetMusicVolume(float volume)
    {
        string[] datas = File.ReadAllLines(FilePath);

        foreach (var audioSource in musicSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        using (StreamWriter writer = new StreamWriter(FilePath)) // 'true' enables appending
        {
            writer.WriteLine(datas[0]);
            writer.WriteLine(volume);
            writer.WriteLine(datas[2]);
        }
    }

    // Set volume for all Ambiance audio sources
    private void SetAmbianceVolume(float volume)
    {
        string[] datas = File.ReadAllLines(FilePath);

        foreach (var audioSource in ambianceSources)
        {
            if (audioSource != null)
            {
                audioSource.volume = volume;
            }
        }

        using (StreamWriter writer = new StreamWriter(FilePath)) // 'true' enables appending
        {
            writer.WriteLine(datas[0]);
            writer.WriteLine(datas[1]);
            writer.WriteLine(volume);
        }
    }
}


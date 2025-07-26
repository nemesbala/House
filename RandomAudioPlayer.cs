using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomAudioPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips; // Array to hold audio clips
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        PlayRandomClip(); // Start playing a random clip at the beginning
    }

    // Play a random audio clip and schedule the next clip to play after this one ends
    private void PlayRandomClip()
    {
        if (audioClips.Length == 0)
        {
            Debug.LogWarning("No audio clips assigned.");
            return;
        }

        // Select a random clip from the array
        AudioClip randomClip = audioClips[Random.Range(0, audioClips.Length)];

        // Play the selected audio clip
        audioSource.clip = randomClip;
        audioSource.Play();

        // Schedule the next clip to play when this one finishes
        Invoke(nameof(PlayRandomClip), randomClip.length);
    }
}

using UnityEngine;

public class PlayAudio : MonoBehaviour
{
    // Declare a public AudioClip variable
    public AudioClip audioClip;

    void Start()
    {
        // Check if the audio clip is assigned
        if (audioClip == null)
        {
            Debug.LogError("No audio clip assigned!");
            return;
        }

        // Create an AudioSource component if it doesn't exist
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Assign the loaded audio clip to the AudioSource
        audioSource.clip = audioClip;

        // Play the audio clip
        audioSource.Play();
    }
}

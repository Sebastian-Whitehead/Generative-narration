using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayerScript : MonoBehaviour
{
    public AudioClip[] audioClips;
    public float[] delayValues = { 1f, 2f, 3f, 4f, 5f }; // Five delay values for 0-9
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Checking numpad keypresses
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Keypad0 + i))
            {
                PlayAudio(i);
                break;
            }
        }
    }

    void PlayAudio(int index)
    {
        // Check if index is within the bounds of the audioClips array
        if (index >= 0 && index < audioClips.Length)
        {
            // Set the audio clip
            AudioClip clipToPlay = audioClips[index];

            // Set the delay before playing the audio clip
            float delay = delayValues[index % delayValues.Length]; // Use modulo to ensure index wraps around for delayValues

            // Play the audio clip after the specified delay
            Invoke("PlayDelayedAudio", delay);

            // Play the selected audio clip immediately
            audioSource.clip = clipToPlay;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("Index out of range for audioClips array.");
        }
    }

    void PlayDelayedAudio()
    {
        // No action needed here as audio was already played immediately.
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class RandomSoundTrigger : MonoBehaviour, ITrigger
{
    public List<AudioClip> soundClips = new List<AudioClip>(); // List of sound clips to choose from
    public bool isNarration;  
    private AudioSource _audioSource;

    // Interface variables
    public float Duration { get; set; }
    public bool waitForCompletion = false;
    public Fx Type => isNarration ? Fx.Narration : Fx.Sound;
    
    public bool WaitForCompletion => waitForCompletion;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        // Check if there are any sound clips in the list
        if (soundClips.Count !> 0)
        {
            Debug.LogError("No sound clips assigned to RandomSoundTrigger on " + gameObject.name);
        }
    }
    
    // Called by a trigger function
    public void Trigger()
    {
        // Pick a random sound clip from the list
        AudioClip randomClip = soundClips[Random.Range(0, soundClips.Count)];
        _audioSource.clip = randomClip;
        Duration = _audioSource.clip.length;
        
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }
    
    // Stops the playing audio source if called
    public void StopSound() 
    {
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
        }
    }
}
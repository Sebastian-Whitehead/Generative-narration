using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class DelayController : MonoBehaviour
{   
    [Tooltip("Button that triggers actions.")]
    public Button button;
    
    [Tooltip("Rate of change for changing the input delay.")]
    public float rateOfChange;

    public AudioClip tickUp, tickDown;
    public float tickInterval;
    
    // Pre transfer delay variable
    private float _delay;
    private float _lastDelay; 
    private float _lastTick;
    private AudioSource _source;
    

    private void Start()
    {
        // Copy starting value from the button script
        _delay = _lastDelay = button.triggerDelay;
        _source = GetComponent<AudioSource>();
    }

    void Update()
    {   
        // Print the current delay value
        if (Input.GetKeyUp("p"))
        {
            print($"The Current Delay Value is: {button.triggerDelay}");
        }
        
        // Increase input delay
        if (Input.GetKey("r"))
        {
            _delay += rateOfChange;
            _source.clip = tickUp;
        }
        
        // Decrease input delay
        if (Input.GetKey("f")) {
            _delay -= rateOfChange;
            _source.clip = tickDown;
        }
        
        // Set the input delay for the bound button
        if (_lastDelay != _delay)
        {
            button.triggerDelay = _delay;
          if (Mathf.Abs(_lastTick- _delay) >= tickInterval){
            _source.Play();
            _lastTick = _delay;
          }
            // print(_delay);
            _lastDelay = _delay;
        }
    }
}

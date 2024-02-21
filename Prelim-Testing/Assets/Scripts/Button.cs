using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Button : MonoBehaviour
{
    [Tooltip("Maximum distance for the button interaction.")]
    public float maxDistance = 5f;

    [Tooltip("List of objects bound to this button.")]
    public MonoBehaviour boundObject;

    [Tooltip("Sound played when the button is pressed.")]
    public AudioClip buttonSound;

    [Tooltip("Key code to trigger the button.")]
    public KeyCode inputKey;

    [Tooltip("Delay between trigger and action.")]
    public float triggerDelay;

    private ITrigger _mBoundTrigger;
    private AudioSource _audioSource;

    void Start()
    {
        _mBoundTrigger = boundObject.GetComponent<ITrigger>();
        _audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, maxDistance) && hit.transform == this.transform && Input.GetKeyDown(inputKey))
        {
            _audioSource.PlayOneShot(buttonSound);

            // Start the TriggerBoundObjects coroutine without waiting for completion
            StartCoroutine(TriggerBoundObjects());
        }
    }

    IEnumerator TriggerBoundObjects()
    {
        //Delay trigger
        yield return new WaitForSeconds(triggerDelay);
        
        _mBoundTrigger.Trigger();
    }
}

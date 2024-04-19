using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Descriptors))]
public class GrabExtension : MonoBehaviour
{
    public PipelineManager manager;
    public XRGrabInteractable xrGrab;

    private string ActionDescription = "picked up";

    private void Start()
    {
        manager = FindObjectOfType<PipelineManager>();
        xrGrab = GetComponent<XRGrabInteractable>();
        xrGrab.selectEntered.AddListener(PassObjToPipeline);
    }

    public void PassObjToPipeline(SelectEnterEventArgs args) { 
        manager.trigger(this.gameObject, ActionDescription);
    }
       
}

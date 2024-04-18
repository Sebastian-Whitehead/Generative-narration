using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Descriptors))]
public class GrabExtension : MonoBehaviour
{
    public PipelineManager manager;
    private string ActionDescription = "picked up";

    private void Start()
    {
        manager = FindObjectOfType<PipelineManager>();
    }

    public void PassObjToPipeline() { 
        manager.trigger(this.gameObject, ActionDescription);
    }
       
}

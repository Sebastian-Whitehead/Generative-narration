using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lookat : MonoBehaviour
{
    public GameObject camera;
    private float time = 0f;
    private float time2 = 0f;
    private LayerMask _lookable;
    private string output = "Looked at";
    private Transform previousHit;
    public PipelineManager manager;
    // Start is called before the first frame update
    void Start()
    {
        _lookable = LayerMask.GetMask("Lookable");
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit hit;
        Ray ray = new Ray (camera.transform.position, camera.transform.forward);

        Debug.DrawRay(ray.origin, ray.direction * 2, Color.red);

        if (Physics.Raycast(ray,out hit, 2, _lookable))
        {
            time += Time.deltaTime;
            Transform objectHit = hit.transform;

            if (time > 0.5f)
            {
                //Debug.Log("Looked at object" + objectHit + "and triggered text gen" );
                manager.trigger(objectHit.gameObject, output);
            }


            if (previousHit != objectHit)
            {
                time = 0f;
                previousHit = objectHit;

            }
            //Debug.Log("time" + time);

        }
        else
        {
            
            time2 += Time.deltaTime;
            //Debug.Log("Time2 " + time2);

            if (time2 > 0.5f)
            {
                time = 0f;
                time2 = 0f;
                previousHit = null;
            }
        }
    }

    
}

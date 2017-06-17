using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    private string targetTag = "Player";
    private GameObject targetObject;

    // Use this for initialization
	void Start () {
		targetObject = GameObject.FindGameObjectWithTag(targetTag);
	    if (targetObject != null)
	    {
	        //transform.parent = targetObject.transform;
	        transform.position = targetObject.transform.position + new Vector3(0, 100, 0);
	    }
	    else
	    {
	        Debug.LogError("MapCamera: no suitable target found");
	    }
	}

    void Update()
    {
        if (targetObject != null)
        {
            transform.position = targetObject.transform.position + new Vector3(0, 100, 0);
        }
    }
}

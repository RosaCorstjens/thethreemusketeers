using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerGizmo : MonoBehaviour {

    protected string targetTag = "Player";
    protected GameObject targetObject;
    [SerializeField] protected float triggerDistance = 10.0f;
    protected bool Triggered = false;
    protected float heigth = 12.0f;

    [SerializeField]
    private GameObject GizmoPrefab;

    // Use this for initialization
    public virtual void Start () {
        targetObject = GameObject.FindGameObjectWithTag(targetTag);
    }

    // Update is called once per frame
    public virtual void Update ()
	{
	    if (!Triggered && CheckTrigger())
	    {
	        ShowGizmo();
	    }
	}

    protected virtual GameObject ShowGizmo()
    {
        return Instantiate(GizmoPrefab, transform.position + new Vector3(0, heigth, 0), Quaternion.identity, transform);
    }

    protected virtual bool CheckTrigger()
    {
        if ((transform.position - targetObject.transform.position).magnitude <= triggerDistance)
        { 
            Triggered = true;
            return true;
        }
        return false;
    }
}

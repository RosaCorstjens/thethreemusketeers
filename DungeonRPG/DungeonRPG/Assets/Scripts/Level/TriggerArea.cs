using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TriggerArea : MonoBehaviour
{
    public delegate void OnTriggerAction();
    public OnTriggerAction onTriggerAction;

    public delegate void OffTriggerAction();
    public OffTriggerAction offTriggerAction;

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            onTriggerAction();
        }
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            offTriggerAction();
        }
    }
}

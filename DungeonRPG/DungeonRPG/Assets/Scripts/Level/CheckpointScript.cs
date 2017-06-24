using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointScript : MonoBehaviour
{
    private bool triggered;

    public void Initialize()
    {
        triggered = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" && !triggered)
        {
            triggered = true;
            GameManager.Instance.DungeonManager.CurrentDungeon.StartPosition = this.transform.position;
            GameManager.Instance.UIManager.YouReachedCheckpoint();
        }
    }
}

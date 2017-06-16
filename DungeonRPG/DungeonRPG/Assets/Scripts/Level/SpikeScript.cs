using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour
{
    void Initialize() { }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            col.GetComponent<PlayerController>().GotHit(GameManager.Instance.ActiveCharacterInformation.Stats.MaxDeterminedHealth / 100 * 5);
        }
    }
}

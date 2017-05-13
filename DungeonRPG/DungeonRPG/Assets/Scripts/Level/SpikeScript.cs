using UnityEngine;
using System.Collections;

public class SpikeScript : MonoBehaviour
{
    TriggerArea trigger;
    private Transform targetTransform;
    private PlayerController targetScript;
    Coroutine activateSpikes;

    void Awake()
    {
        trigger = GetComponent<TriggerArea>();

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        trigger.onTriggerAction = PlayerInRange;
    }

    public void PlayerInRange()
    {
        targetScript.GotHit(5);
    }
}

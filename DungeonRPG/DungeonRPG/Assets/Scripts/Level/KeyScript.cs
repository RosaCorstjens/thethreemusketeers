using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    TriggerArea trigger;
    private Transform targetTransform;
    private PlayerController targetScript;

    int keyId;

    private void Awake()
    {
        Initialize(0);
    }

    void Initialize (int id)
    {
        keyId = id;

        trigger = GetComponent<TriggerArea>();

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        this.GetComponent<MeshRenderer>().material.color = LockScript.GetColor(keyId);

        trigger.onTriggerAction = PlayerInRange;
    }

    public void PlayerInRange()
    {
        targetScript.ObtainKey(keyId);
        gameObject.SetActive(false);
    }
}

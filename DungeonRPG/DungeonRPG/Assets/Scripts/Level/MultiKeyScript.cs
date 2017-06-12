using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiKeyScript : MonoBehaviour
{
    TriggerArea trigger;
    private Transform targetTransform;
    private PlayerController targetScript;

    int keyId;

    private void Awake()
    {
        Initialize(1);
    }

    void Initialize(int id)
    {
        keyId = id;

        trigger = GetComponent<TriggerArea>();

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        this.GetComponent<MeshRenderer>().material.color = MultiLockScript.GetColor(keyId);

        trigger.onTriggerAction = PlayerInRange;
    }

    public void PlayerInRange()
    {
        targetScript.ObtainMultiKey(keyId);
        gameObject.SetActive(false);
    }
}

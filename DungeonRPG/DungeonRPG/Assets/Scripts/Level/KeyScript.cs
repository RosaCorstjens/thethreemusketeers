using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    Normal = 0,
    Multi, 
    Final
}

public class KeyScript : MonoBehaviour
{
    [SerializeField] private KeyType type;

    TriggerArea trigger;
    private Transform targetTransform;
    private PlayerController targetScript;

    int keyId;

    public void Initialize (int id)
    {
        keyId = id;
        type = LockScript.GetType(id);

        trigger = GetComponent<TriggerArea>();

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            mesh.material.color = LockScript.GetColor(keyId);
        }

        trigger.onTriggerAction = PlayerInRange;
    }

    public void PlayerInRange()
    {
        targetScript.ObtainKey(keyId, type);
        gameObject.SetActive(false);
    }
}

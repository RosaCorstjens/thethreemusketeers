using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    TriggerArea trigger;
    private Transform targetTransform;
    private PlayerController targetScript;

    int keyId;

    public void Initialize (int id)
    {
        keyId = id;

        trigger = GetComponent<TriggerArea>();

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        MeshRenderer[] meshes = GetComponentsInChildren<MeshRenderer>();
        foreach (var mesh in meshes)
        {
            mesh.material.color = LockScript.GetColor(keyId);
            /*mesh.material.EnableKeyword("_EMISSION");
            mesh.material.SetColor("_EMISSION", new Color(0.125f, 0.125f, 0.125f));*/
        }

        trigger.onTriggerAction = PlayerInRange;
    }

    public void PlayerInRange()
    {
        targetScript.ObtainKey(keyId);
        gameObject.SetActive(false);
    }
}

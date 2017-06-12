using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockScript : MonoBehaviour
{
    TriggerArea trigger;

    private Transform targetTransform;
    private PlayerController targetScript;

    Coroutine unlock;
    MeshRenderer[] children;

    int lockId;
    Animator anim;

    private void Awake()
    {
        Initialize(0);
    }

    void Initialize(int id)
    {
        lockId = id;

        trigger = GetComponent<TriggerArea>();
        
        children = transform.GetComponentsInChildren<MeshRenderer>();

        Color doorColor = GetColor(lockId);


        foreach (var child in children)
        {
            child.material.color = doorColor;
        }

        trigger.onTriggerAction = PlayerInRange;
        trigger.offTriggerAction = PlayerOutOfRange;

        targetTransform = GameManager.Instance.ActiveCharacter.transform;
        targetScript = targetTransform.gameObject.GetComponent<PlayerController>();

        anim = GetComponent<Animator>();
    }

    public static Color GetColor(int id)
    {
        switch (id)
        {
            case 0:
                return Color.blue;
                break;
            case 1:
                return Color.cyan;
                break;
            case 2:
                return Color.gray;
                break;
            case 3:
                return Color.green;
                break;
            case 4:
                return Color.magenta;
                break;
            case 5:
                return Color.red;
                break;
            case 6:
                return Color.white;
                break;
            case 7:
                return Color.yellow;
                break;
            default:
                return Color.black;
                break;
        }
    }

    public void PlayerInRange()
    {
        unlock = StartCoroutine(Unlock());
    }

    public void PlayerOutOfRange()
    {
        if (unlock != null) StopCoroutine(unlock);
    }

    public IEnumerator Unlock()
    {
        while (!targetScript.HasKey(lockId))
        {
            yield return null;
        }

        anim.SetTrigger("Unlock");
        yield return new WaitForSeconds(2f);

        trigger.gameObject.SetActive(false);

        yield break;
    }
}
